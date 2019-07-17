using CK.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using CK.SqlServer.Setup;
using System.Threading.Tasks;
using inProjects.Data.Data.TimedUser;
using System.Collections;

namespace inProjects.Data.Queries
{
    [SqlObjectItem( "vAnswer" )]

    public class TimedUserQueries
    {
        private ISqlConnectionController _controller;

        public TimedUserQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase  )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );
        }

        public async Task<TimedUserData> GetAllTimedUserByUserId(int userId)
        {  
            TimedUserData result =  await _controller.QuerySingleOrDefaultAsync<TimedUserData>( "select * from IPR.tTimedUser tu where tu.UserId = @UserId", new { UserId = userId } );
            return result;
        }

        public async Task<TimedUserData> GetLastTimedUserId( int userId )
        {
            TimedUserData result = await _controller.QuerySingleOrDefaultAsync<TimedUserData>( "select TOP( 1 ) * from IPR.tTimedUser tu where tu.UserId = @UserId order by tu.TimePeriodId DESC;", new { UserId = userId } );
            return result;
        }

        public async Task<IEnumerable<TimedStudentData>> GetAllStudentInfosByGroup(int groupId, string userTimedTable, string specificTimedUserId )
        {
            IEnumerable<TimedStudentData> result = await _controller.QueryAsync<TimedStudentData>( "SELECT * FROM CK.tGroup g JOIN CK.tActor a ON a.ActorId = g.GroupId AND g.GroupId = @GroupId JOIN CK.tActorProfile ap ON ap.GroupId = g.GroupId AND ap.ActorId <> ap.GroupId JOIN CK.tUser u ON u.UserId = ap.ActorId  JOIN IPR.tTimedUser tu ON tu.UserId = u.UserId  AND tu.TimePeriodId = g.ZoneId JOIN IPR.t" + userTimedTable + " ts ON ts.Timed" + specificTimedUserId + " = tu.TimedUserId AND ts.Timed" + specificTimedUserId + " is not null;", new { GroupId = groupId } );
            return result;
        }

        public async Task<TimedUserData> GetTimedUserById( int timedUserId )
        {
            TimedUserData result = await _controller.QuerySingleOrDefaultAsync<TimedUserData>( "select * from IPR.tTimedUser tu where tu.TimedUserId = @TimedUserId", new { TimedUserId = timedUserId } );
            return result;
        }

        public async Task<bool> VerifyUserInSpecificGroup( int timedUserId,string typeGroup )
        {
            TimedUserData user = await GetTimedUserById( timedUserId );

            if(user == null  )
            {
                return false;
            }

            //verify group exist
            GroupQueries groupQueries = new GroupQueries( _controller );
            int idGroup = await groupQueries.GetSpecificIdGroupByZoneIdAndGroupName( user.TimePeriodId, typeGroup );

            if(idGroup == 0 )
            {
                return false;
            }

            //verify user is in the group
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "SELECT ActorId FROM CK.tActorProfile where ActorId = @UserId AND GroupId = @GroupId", new { UserId = user.UserId, GroupId = idGroup } );

            if(result != 0 )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<List<string>> getWhichCat( int timedUserId, List<string> list )
        {
            if( await this.IsJury( timedUserId ) == true )
            {
                list.Add( "Jury" );
            }
            if( await this.IsStaffMember( timedUserId ) == true )
            {
                list.Add( "StaffMember" );
            }

            if( await this.IsStudent( timedUserId ) == true )
            {
                list.Add( "Student" );

            }

            return list;

        }

        public async Task<bool> IsJury( int timedUserId )
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "select * from IPR.tTimedJury tu where tu.TimedJuryId = @TimedUserId", new { TimedUserId = timedUserId } );

            return result != 0 ? true : false;
        }

        public async Task<bool> IsStudent( int timedUserId )
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "select * from IPR.tTimedStudent tu where tu.TimedStudentId = @TimedUserId", new { TimedUserId = timedUserId} );

            return result != 0 ? true : false;
        }

        public async Task<bool> IsStaffMember( int timedUserId )
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( " select * from IPR.tTimedStaffMember tu where tu.TimedStaffMemberId = @TimedUserId   ", new { TimedUserId = timedUserId } );

            return result != 0 ? true : false;
        }

        public async Task<TimedUserData> GetTimedUser( int userId, int periodId )
        {
            TimedUserData result = await _controller.QuerySingleOrDefaultAsync<TimedUserData>( "select * from IPR.tTimedUser tu where tu.UserId = @UserId AND tu.TimePeriodId = @TimePeriodId", new { UserId = userId, TimePeriodId = periodId } );
            return result;
        }

    }
}
