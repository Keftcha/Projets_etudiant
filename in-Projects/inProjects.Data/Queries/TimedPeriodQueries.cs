
using CK.SqlServer;
using CK.SqlServer.Setup;
using Dapper;
using inProjects.Data.Data.Period;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inProjects.Data.Queries
{
    public class TimedPeriodQueries
    {
            private ISqlConnectionController _controller;

            public TimedPeriodQueries(ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
            {
                _controller = ctx.GetConnectionController( sqlDefaultDatabase );

            }

        public async Task<PeriodData> GetLastPeriodBySchool( int schoolId )
        {
            PeriodData result = await _controller.QuerySingleOrDefaultAsync<PeriodData>( "SELECT TOP(1) OrderedChild = ChildOrderByKey.ToString(), ChildId, ZoneId, BegDate, EndDate FROM CK.vZoneDirectChildren JOIN IPR.tTimePeriod  on ChildId = TimePeriodId WHERE ZoneId = @ZoneId ORDER BY ChildOrderByKey DESC; ", new { ZoneId = schoolId } );
            return result;
        }

        public async Task<List<PeriodData>> GetNumberWishPeriodBySchool( int schoolId, int numberWish )
        {
            IEnumerable<PeriodData> result = await _controller.QueryAsync<PeriodData>( "SELECT TOP(@NumberWish) OrderedChild = ChildOrderByKey.ToString(), ChildId, ZoneId, BegDate,EndDate FROM CK.vZoneDirectChildren JOIN IPR.tTimePeriod  on ChildId = TimePeriodId WHERE ZoneId = @ZoneId ORDER BY ChildOrderByKey DESC; ", new { ZoneId = schoolId, NumberWish = numberWish } );
            return result.AsList();
        }

        public async Task<List<PeriodData>> GetAllPeriodsBySchool( int schoolId )
        {
            IEnumerable<PeriodData> result = await _controller.QueryAsync<PeriodData>
                ( "SELECT  OrderedChild = ChildOrderByKey.ToString(), ChildId, ckz.ZoneId, BegDate,EndDate, g.GroupName FROM CK.vZoneDirectChildren ckz JOIN IPR.tTimePeriod  on ChildId = TimePeriodId JOIN CK.tGroup g on ckz.ChildId = g.GroupId WHERE ckz.ZoneId = @ZoneId ORDER BY ChildOrderByKey",
                new { ZoneId = schoolId } );
            return result.AsList();
        }

        public async Task UpdatePeriodDate( int periodID, DateTime begDate, DateTime endDate )
        {
            await _controller.ExecuteAsync( "UPDATE IPR.tTimePeriod set BegDate=@BegDate, EndDate=@EndDate where TimePeriodId=@TimePeriodId ", new {BegDate = begDate, EndDate = endDate, TimePeriodId = periodID } );
        }

        public async Task<PeriodData> GetPeriodById( int periodId )
        {
            PeriodData result = await _controller.QuerySingleOrDefaultAsync<PeriodData>( "SELECT OrderedChild = ChildOrderByKey.ToString(), ChildId, ZoneId, BegDate, EndDate FROM CK.vZoneDirectChildren  JOIN IPR.tTimePeriod  on ChildId = TimePeriodId WHERE TimePeriodId = @TimePeriodId ORDER BY ChildOrderByKey DESC; ", new { TimePeriodId = periodId } );
            return result;
        }

        public async Task<PeriodData> GetCurrentPeriod(int zoneId )
        {
            PeriodData result = await _controller.QuerySingleOrDefaultAsync<PeriodData>( "SELECT * FROM IPR.tTimePeriod WHERE TimePeriodId = @ZoneId;", new { ZoneId = zoneId } );
            return result;
        }

        public async Task<string> GetGroupNameActualPeriod( int timePeriodId, int zoneId )
        {
            return await _controller.QuerySingleOrDefaultAsync<string>(
                  "select GroupName from CK.tGroup where GroupId = @TimePeriodId and ZoneId = @ZoneId ", new { TimePeriodId = timePeriodId,ZoneId = zoneId  } );

        }

        public async Task<DateTime> GetBegDateOfPeriod(int periodId )
        {
            return await _controller.QuerySingleOrDefaultAsync<DateTime>( "SELECT BegDate FROM IPR.tTimePeriod WHERE TimePeriodId = @TimePeriod", new { TimePeriod = periodId } );

        }
    }
}
