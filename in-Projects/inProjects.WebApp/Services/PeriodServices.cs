using CK.Auth;
using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data;
using inProjects.Data.Data.Group;
using inProjects.Data.Data.Period;
using inProjects.Data.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Services
{
    public class PeriodServices
    {


        public async Task<bool> CheckInPeriod( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo )
        {
            var sqlDatabase = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var group = stObjMap.StObjs.Obtain<CustomGroupTable>();
            int userId = authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext())
            {
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDatabase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );
                PeriodData periodData = await timedPeriodQueries.GetCurrentPeriod( groupData.ZoneId );

                DateTime currentDateTime = DateTime.UtcNow;
                int shoulbBeLowerOrEqual = DateTime.Compare( periodData.BegDate, currentDateTime );
                int shouldBeGreaterOrEqual = DateTime.Compare( periodData.EndDate, currentDateTime );

                if(shoulbBeLowerOrEqual > 0 || shouldBeGreaterOrEqual <0)
                {
                    return false; 
                }
                else
                {
                    return true;
                }
            } 

        }

        public async Task<bool> CheckPeriodGivenDate( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo, DateTime begDate, DateTime endDate )
        {
            var sqlDatabase = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var group = stObjMap.StObjs.Obtain<CustomGroupTable>();
            int userId = authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext() )
            {
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDatabase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );
                PeriodData periodData = await timedPeriodQueries.GetCurrentPeriod( groupData.ZoneId );

               int shouldBeGreater = begDate.CompareTo( periodData.BegDate );
               int shouldBeLower = endDate.CompareTo( periodData.EndDate );

                if(shouldBeGreater >= 0 && shouldBeLower <= 0 )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
}
