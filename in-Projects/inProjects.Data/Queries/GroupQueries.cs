

using CK.SqlServer;
using CK.SqlServer.Setup;
using Dapper;
using inProjects.Data.Data.Group;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data.Queries
{
   public class GroupQueries
    {
        private ISqlConnectionController _controller;

        public GroupQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );

        }

        public GroupQueries(ISqlConnectionController controller )
        {
            _controller = controller;
        }

        public async Task<int> GetSpecificIdGroupByZoneIdAndGroupName(int zoneId,string groupName )
        {
            int result =  await _controller.QuerySingleOrDefaultAsync<int>( "SELECT GroupId FROM CK.tGroup where GroupName = @GroupName AND ZoneId =@ZoneId;", new { ZoneId = zoneId, GroupName = groupName } );
            return result;
        }

        public async Task<List<string>> GetAllGroupByZoneId(int zoneID )
        {
            IEnumerable<string> list = await _controller.QueryAsync<string>( "select GroupName from CK.vGroup where ZoneId = 0 AND IsZone = 0 OR ZoneId = @ZoneId AND IsZone = 0 group by GroupName;", new { ZoneId = zoneID } );
            return list.AsList();
        }

        public async Task<GroupData> GetIdSchoolByPeriodId(int zoneId)
        {
            GroupData data = await _controller.QueryFirstAsync<GroupData>( "select * from CK.vZone where ZoneId = @ZoneId", new { ZoneId = zoneId } );
            return data;
        }

        public async Task<IEnumerable<GroupData>> GetAllGroupByPeriod(int zoneId)
        {
            IEnumerable<GroupData> result = await _controller.QueryAsync<GroupData>( "SELECT * FROM CK.vGroup WHERE ZoneId = @ZoneId AND IsZone = 0 AND UserCount <> 0;", new { ZoneId = zoneId } );
            return result;
        }

        public async Task<int> GetIdSchoolByName(string schoolName)
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "SELECT SchoolId FROM IPR.tSchool WHERE[Name] = @SchoolName;", new { SchoolName = schoolName } );
            return result;
        }

        public async Task<GroupData> GetIdSchoolByConnectUser(int userId)
        {
            GroupData result = await _controller.QuerySingleOrDefaultAsync<GroupData>( "SELECT TOP(1) * FROM CK.tActor a JOIN CK.tActorProfile ap ON a.ActorId = ap.ActorId AND a.ActorId = @UserId JOIN CK.vGroup g ON g.GroupId = ap.GroupId AND g.IsZone = 0 JOIN CK.vZone z ON z.ZoneId = g.ZoneId ORDER BY g.ZoneId DESC;", new { UserId = userId } );
            return result;
        }

        //Recupere la liste de nom de tous les groupe de l'utilisateur par sa periodId et son timePeriodId
        public async Task<List<string>> GetAllGroupOfTimedUserByPeriod( int periodId, int timedUserID )
        {
            IEnumerable<string> result = await _controller.QueryAsync<string>( "select g.GroupName from CK.tGroup g join CK.tActorProfile ac on ac.GroupId = g.GroupId AND g.ZoneId = @TimePeriodId join IPR.tTimedUser tu on tu.UserId = ac.ActorId AND tu.TimedUserId = @TimedUserId ", new { TimePeriodId = periodId, TimedUserId = timedUserID } );
            return result.AsList();
        }

        public async Task<int> GetIdGroupByNameAndPeriod(string groupName, int periodId )
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "SELECT GroupId FROM CK.tGroup WHERE GroupName = @GroupName AND ZoneId = @ZoneId;", new { GroupName = groupName, ZoneId = periodId } );
            return result;
        }

        public async Task<int> GetGroupByNamePeriodUser(string groupName, int actorId, int zoneId )
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "SELECT * FROM CK.vGroup g JOIN CK.tActorProfile ap ON ap.GroupId = g.GroupId AND g.GroupName = @GroupName AND ap.ActorId = @UserId AND g.ZoneId = @ZoneId", new { GroupName = groupName, @UserId = actorId, @ZoneId = zoneId } );
            return result;
        }
            
       public async Task<IEnumerable<GroupData>>GetAllGroupByTimedUser(int timedUserId )
        {
            IEnumerable<GroupData> result = await _controller.QueryAsync<GroupData>( "SELECT g.GroupId, g.GroupName FROM IPR.tTimedUser tu JOIN CK.tUser u ON u.UserId = tu.UserId AND tu.TimedUserId = @TimedUserId JOIN CK.tActorProfile ap ON ap.ActorId = u.UserId JOIN CK.tGroup g ON g.GroupId = ap.GroupId ", new { TimedUserId = timedUserId } );
            return result;
        }

    }
}
