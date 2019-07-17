using CK.Auth;
using CK.AspNet.Auth;
using CK.Core;
using CK.DB.Auth;
using CK.DB.Actor.ActorEMail;
using CK.SqlServer;
using inProjects.Data;
using inProjects.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using inProjects.Data.Queries;
using CK.SqlServer.Setup;
using inProjects.Data.Data.User;
using inProjects.Data.Data.Group;
using inProjects.Data.Data.Period;
using inProjects.Data.Data.TimedUser;
using System.Collections.Generic;
using inProjects.Data.Data.ProjectStudent;
using inProjects.WebApp.Services;

namespace inProjects.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class AccountController : Controller
    {
        readonly IOptions<SpaOptions> _spaOptions;
        readonly IAuthenticationTypeSystem _typeSystem;
        readonly IStObjMap _stObjMap;
        readonly IWebFrontAuthLoginService _loginService;
        readonly IAuthenticationInfo _authenticationInfo;

        public AccountController( IOptions<SpaOptions> spaOptions, IAuthenticationTypeSystem typeSystem, IStObjMap stObjMap, IWebFrontAuthLoginService loginService, IAuthenticationInfo authenticationInfo )
        {
            _spaOptions = spaOptions;
            _typeSystem = typeSystem;
            _stObjMap = stObjMap;
            _loginService = loginService;
            _authenticationInfo = authenticationInfo;
        }

        [HttpPost( "register" )]
        [AllowAnonymous]
        public async Task<string> Register( [FromBody]RegisterModel model )
        {
            var userTable = _stObjMap.StObjs.Obtain<CustomUserTable>();
            var actorEmail = _stObjMap.StObjs.Obtain<ActorEMailTable>();
            var basic = _stObjMap.StObjs.Obtain<IBasicAuthenticationProvider>();

            using( var ctx = new SqlStandardCallContext() )
            {
                var loginId = Guid.NewGuid().ToString();
                var userId = await userTable.CreateUserAsync( ctx, 1,
                    loginId, model.FirstName, model.LastName );

                if( userId != 0 && userId != 1 )
                {
                    await basic.CreateOrUpdatePasswordUserAsync( ctx, 1, userId, model.Password );
                    await actorEmail.AddEMailAsync( ctx, 1, userId, model.Email, true, false );

                    return loginId;
                }
                return null;
            }
        }

        [HttpPost( "getUserName" )]
        [AllowAnonymous]
        public async Task<UserInfosModel> GetUserName( [FromBody] LoginModel model )
        {
            var userTable = _stObjMap.StObjs.Obtain<CustomUserTable>();

            try
            {
                return await userTable.GetUserName( new SqlStandardCallContext(), model.Email );
            }
            catch( Exception e )
            {
                return null;
            }
        }

        [HttpPost( "changePassword" )]
        public async Task<IActionResult> ChangePassword( [FromBody] PasswordChangeModel model )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var userTable = _stObjMap.StObjs.Obtain<CustomUserTable>();
            var basic = _stObjMap.StObjs.Obtain<IBasicAuthenticationProvider>();

            using( var ctx = new SqlStandardCallContext() )
            {
                LoginResult login = await basic.LoginUserAsync( ctx, userId, model.oldPassword, false );
                
                if(!login.IsSuccess)
                {
                    Result result = new Result( Status.Unauthorized, "L'ancien mot de passe ne correspond pas " );
                    return this.CreateResult( result );
                }

                await basic.SetPasswordAsync( ctx, userId, userId, model.newPassword);

                return Ok();
            }

        }

        [HttpGet( "getInfosAccount" )]
        public async Task<IActionResult> GetInfosOwnAccount()
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                AccountUserData accountUserData = new AccountUserData();

                PeriodServices periodService = new PeriodServices();
                UserQueries userQueries = new UserQueries( ctx, sqlDataBase );
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDataBase );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, sqlDataBase );

                //Donnée de l'utilisateur
                UserData userData = await userQueries.GetInfosUserById( userId );

                //Derniere PeriodId de l'user et timedUserId
                TimedUserData timedUser = await timedUserQueries.GetLastTimedUserId( userId );

                if(timedUser == null )
                {
                    accountUserData.UserData = userData;
                    accountUserData.Group = "User";
                    return Ok( accountUserData );

                }

                //Recuperer Groupe du timedUser
                List<string> listGroupUser = await groupQueries.GetAllGroupOfTimedUserByPeriod( timedUser.TimePeriodId, timedUser.TimedUserId );

                string group = listGroupUser.Find( x => x.StartsWith( "S0" ) || x.StartsWith( "S1" ) || x == "Teacher" || x == "Administration" );
                string spec = listGroupUser.Find( x => x == "IL" || x == "SR" );

                if( spec != null ) group += "-" + spec;

                accountUserData.UserData = userData;
                accountUserData.Group = group;

                if( await periodService.CheckInPeriod( _stObjMap, _authenticationInfo ) ) accountUserData.IsActual = true;
                else accountUserData.IsActual = false;

                return Ok( accountUserData );
            }
        }

        [HttpPost( "changeInfosAccount" )]
        public async Task<IActionResult> changeInfosUser( [FromBody] AccountUserData account )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var userTable = _stObjMap.StObjs.Obtain<CustomUserTable>();
            var actorEmail = _stObjMap.StObjs.Obtain<ActorEMailTable>();
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {

                int status = await userTable.UpdateUserInfos( ctx,account.UserData.FirstName, account.UserData.LastName, account.UserData.Email, account.UserData.EmailSecondary, userId );

                if(status == 1 )
                {
                    Result result = new Result( Status.Unauthorized, "Email principale deja existant" );
                    return this.CreateResult( result );
                }

                if(status == 2 )
                {
                    Result result = new Result( Status.Unauthorized, "Email principale ne peut pas etre egal à l'email secondaire" );
                    return this.CreateResult( result );
                }
                return Ok();
            }

        }

        [HttpGet( "getProjectsFav" )]
        public async Task<IActionResult> GetProjectsFav()
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();


            using( var ctx = new SqlStandardCallContext() )
            {
                UserQueries userQueries = new UserQueries( ctx, sqlDataBase );

                List<ProjectUserFavData> userFavProjects = await userQueries.GetProjectsFavUser( userId );


                //UserFavProjectData test = userFavProjects[0];

                //userFavProjects.Add( test );
                //userFavProjects.Add( test );
                //userFavProjects.Add( test );

                return Ok( userFavProjects );
            }
        }

        [HttpGet( "getProjects" )]
        public async Task<IActionResult> GetProjects()
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();


            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDataBase );

                List<ProjectData> userProjects = await projectQueries.GetProjectsUser( userId );

                //ProjectData test = userProjects[0];

                //userProjects.Add( test );
                //userProjects.Add( test );
                //userProjects.Add( test );

                return Ok( userProjects );
            }
        }

    }
}
