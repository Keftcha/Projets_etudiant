using CK.AspNet.Auth;
using CK.Auth;
using CK.Core;
using CK.DB.Actor.ActorEMail;
using CK.DB.Auth;
using CK.DB.User.UserOidc;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data;
using inProjects.Data.Queries;
using System;
using System.Threading.Tasks;

namespace inProjects.WebApp.Services
{
    public class AutoCreateAccountService : IWebFrontAuthAutoCreateAccountService
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationDatabaseService _dbAuth;
        readonly IAuthenticationTypeSystem _typeSystem;

        public AutoCreateAccountService(
            IAuthenticationDatabaseService dbAuth,
            IStObjMap stObjMap,
            IAuthenticationTypeSystem typeSystem
           )
        {
            _stObjMap = stObjMap;
            _dbAuth = dbAuth;
            _typeSystem = typeSystem;
        }

        public async Task<UserLoginResult> CreateAccountAndLoginAsync( IActivityMonitor monitor, IWebFrontAuthAutoCreateAccountContext context )
        {
            var userTable = _stObjMap.StObjs.Obtain<CustomUserTable>();
            var actorEmail = _stObjMap.StObjs.Obtain<ActorEMailTable>();
            var oidcTable = _stObjMap.StObjs.Obtain<UserOidcTable>();

            ICustomUserOidcInfos infos = (ICustomUserOidcInfos)context.Payload;
            ISqlCallContext ctx = context.HttpContext.RequestServices.GetService<ISqlCallContext>();

            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            using( var sqlCtx = new SqlStandardCallContext() )
            {
                UserQueries userQueries = new UserQueries( sqlCtx, db );
                int exists = await userQueries.CheckEmail( infos.Email );
                if(exists == 0)
                {
                    int userId = await userTable.CreateUserAsync( ctx, 1, Guid.NewGuid().ToString(),
                        infos.FirstName,infos.LastName);

                    await actorEmail.AddEMailAsync( ctx, 1, userId, infos.Email, true, false );

                    UCLResult result = await oidcTable.CreateOrUpdateOidcUserAsync(
                        ctx, 1, userId, infos, UCLMode.CreateOrUpdate | UCLMode.WithActualLogin );

                    if( result.OperationResult != UCResult.Created ) return null;

                    return await _dbAuth.CreateUserLoginResultFromDatabase( ctx, _typeSystem, result.LoginResult );
                }
                else
                {
                    UCLResult result = await oidcTable.CreateOrUpdateOidcUserAsync(
                        ctx, 1, exists, infos, UCLMode.CreateOrUpdate | UCLMode.WithActualLogin );
                    return await _dbAuth.CreateUserLoginResultFromDatabase( ctx, _typeSystem, result.LoginResult );
                }
            }
        }
    }
}
