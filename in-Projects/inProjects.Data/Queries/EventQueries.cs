using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.Data.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace inProjects.Data.Queries
{
    public class EventQueries
    {
        private ISqlConnectionController _controller;

        public EventQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );

        }


        public async Task<List<EventData>> GetAllEventsByIdSchool(int idSchool )
        {
            IEnumerable<EventData> result = await _controller.QueryAsync<EventData>(
               "SELECT es.EventId,g.GroupName as Name,es.BegDate,es.EndDate" +
               " FROM IPR.tEventSchool es" +
               " JOIN CK.tGroup g on g.GroupId = es.EventId" +
               " JOIN CK.vZoneDirectChildren zdc on zdc.ChildId = g.ZoneId and zdc.ZoneId = @ZoneID" +
               " ORDER BY es.EventId ASC", new { ZoneID = idSchool } );

            return result.AsList();
        }

        public async Task UpdateEvent(int EventId, string name, DateTime begDate, DateTime endDate)
        {
            await _controller.ExecuteAsync( "update IPR.tEventSchool set BegDate=@BegDate, EndDate=@EndDate where EventId=@Id"
                                            , new { BegDate = begDate, EndDate = endDate, Id = EventId } );

            await _controller.ExecuteAsync( "update CK.tGroup set GroupName = @Name where GroupId = @Id"
                                           , new { Name = name,Id = EventId } );
        }

        public async Task<EventData> GetEventSchoolBylNameAndPeriodId( string name, int periodId )
        {
            EventData result = await _controller.QueryFirstOrDefaultAsync<EventData>(
               "SELECT *" +
               " FROM IPR.tEventSchool es" +
               " JOIN CK.tGroup g on g.GroupId = es.EventId" +
               " where g.ZoneId = @TimePeriodID and g.GroupName like CONCAT(@Name,'%')", new { TimePeriodID = periodId, Name = name} );
            return result;
        }


    }
}
