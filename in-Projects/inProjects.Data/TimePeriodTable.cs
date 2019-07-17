using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data
{
    [SqlTable( "tTimePeriod", Package = typeof( Package ) )]
    [Versions( "1.0.0" )]
    public abstract class TimePeriodTable : SqlTable
    {
        void StObjConstruct( CK.DB.HZone.ZoneTable zoneTable )
        {

        }

        [SqlProcedure( "sCreateTimePeriod" )]
        public abstract Task<int> CreateTimePeriodAsync( ISqlCallContext context, int actorId, DateTime begDate, DateTime endDate, String kind, int parentId = 0 );

    }
}
