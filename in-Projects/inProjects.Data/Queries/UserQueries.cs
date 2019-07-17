using CK.SqlServer;
using CK.SqlServer.Setup;
using Dapper;
using inProjects.Data.Data.User;
using System.Threading.Tasks;
using inProjects.Data.Data.TimedUser;
using inProjects.Data.Data.ProjectStudent;
using System.Collections.Generic;

namespace inProjects.Data.Queries
{
    public class UserQueries
    {
        private ISqlConnectionController _controller;

        public UserQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController(sqlDefaultDatabase);        
        }

        public async Task<UserData> GetUserByName( string firstName, string lastName )
        {
            return await _controller.QueryFirstAsync<UserData>(
                "SELECT * FROM CK.tUser u WHERE u.FirstName = @FirstName AND u.LastName = @LastName",
                new { FirstName = firstName, LastName = lastName }
            );
        }

        public async Task<UserData> GetUserByEmail(string address)
        {
            return await _controller.QuerySingleAsync<UserData>(
                "select UserId, FirstName, LastName from CK.vUser where PrimaryEMail = @Address",
                new {Address = address}
            );
        }

        public async Task<int> VerifyGroupUser(int idUser,int idGroup )
        {
            int isExist =  await _controller.QuerySingleOrDefaultAsync<int>( "select ActorId from CK.tActorProfile where GroupId = @GroupId and ActorId = @ActorId;", new { GroupId = idGroup, ActorId = idUser } );
            return isExist;
        }

        public async Task<int> CheckEmail(string mail )
        {
            int exists =  await _controller.QuerySingleOrDefaultAsync<int>( " SELECT ActorId FROM CK.tActorEMail WHERE EMail = @Mail", new { Mail = mail } );
            return exists;
        }

        public async Task<TimedUserData> CheckIfTimedUserExists(int idUser, int idPeriod )
        {
            TimedUserData exists = await _controller.QuerySingleOrDefaultAsync<TimedUserData>( "SELECT * FROM IPR.tTimedUser tu WHERE tu.UserId = @UserId AND tu.TimePeriodId = @PeriodId", new { UserId = idUser, PeriodId = idPeriod } );
            return exists;
        }

        public async Task<UserData> GetInfosUserById(int idUser )
        {
           return await _controller.QuerySingleOrDefaultAsync<UserData>( "SELECT u.FirstName, u.LastName, am.EMail, EmailSecondary = (SELECT EMail FROM CK.tActorEMail am where am.ActorId = @UserId and am.IsPrimary = 0)" +
                " FROM CK.tUser u" +
                " join CK.tActorEMail am on u.UserId = am.ActorId" +
                " where u.UserId = @UserId and am.IsPrimary = 1; ", new {UserId = idUser } );
        }

        public async Task<List<ProjectUserFavData>> GetProjectsFavUser( int idUser )
        {
            IEnumerable<ProjectUserFavData> usersFav = await _controller.QueryAsync<ProjectUserFavData>
                (" select g.GroupName, ps.Logo,ps.ProjectStudentId as ProjectId from IPR.tUserFavProject ufp " +
                " join IPR.tProjectStudent ps on ufp.ProjectId = ps.ProjectStudentId" +
                " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                " where ufp.UserId = @UserId", new { UserId = idUser } );

            return usersFav.AsList();
        }

        public async Task<IEnumerable<UserByProjectData>> GetUserByProject(int projectId )
        {
            IEnumerable<UserByProjectData> userByProjects = await _controller.QueryAsync<UserByProjectData>( "SELECT u.UserId, u.FirstName, u.LastName, tu.TimedUserId, tp.BegDate, tp.EndDate FROM CK.tGroup g JOIN CK.tActorProfile ap ON ap.GroupId = g.GroupId JOIN CK.tUser u ON u.UserId = ap.ActorId AND g.GroupId = @ProjectId JOIN IPR.tTimedUser tu ON tu.UserId = u.UserId AND g.ZoneId = tu.TimePeriodId JOIN IPR.tTimePeriod tp ON tp.TimePeriodId = tu.TimePeriodId", new { ProjectId = projectId } );
            return userByProjects;
        }

        public async Task<int> GetJuryId(int userId, int timePeriodId )
        {
            return await _controller.QueryFirstOrDefaultAsync<int>(
                " SELECT e.JuryId" +
                " FROM IPR.tEvaluates e " +
                " JOIN CK.tActorProfile ap on ap.GroupId = e.JuryId" +
                " JOIN CK.tGroup g on g.GroupId = e.ProjectId" +
                " JOIN IPR.tProjectStudent ps on ps.ProjectStudentId = e.ProjectId" +
                " where ap.ActorId = @UserId and g.ZoneId = @TimePeriodId", new { UserId = userId, TimePeriodId = timePeriodId } );
        }
    }
}
