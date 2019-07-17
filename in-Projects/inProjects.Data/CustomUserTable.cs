using System.Threading.Tasks;
using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using inProjects.ViewModels;
using CK.SqlServer.Setup;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace inProjects.Data
{
    [SqlTable("tUser", Package = typeof( Package ) ), Versions( "1.0.0" )]
    [SqlObjectItem("transform:vUser")]
    public abstract partial class CustomUserTable : UserTable
    {
        void StObjConstruct(UserTable userTable)
        {
            
        }

        [SqlProcedure( "transform:sUserCreate" )]
        public abstract Task<int> CreateUserAsync( ISqlCallContext context, int actorId, string userName, string firstName, string lastName );

        [SqlProcedure( "sUpdateUserInfos" )]
        public abstract Task<int> UpdateUserInfos( ISqlCallContext ctx, string firstName, string lastName, string primaryEmail, string secondaryEmail, int userId );

        public async Task<UserInfosModel> GetUserName ( ISqlCallContext ctx, string email )
        {
            return await ctx[Database].Connection.QueryFirstAsync<UserInfosModel>(
                @"SELECT UserName
                  FROM CK.vUser
                  WHERE PrimaryEMail = @PrimaryEMail", new { PrimaryEMail = email }
                );
        }
    }
}
