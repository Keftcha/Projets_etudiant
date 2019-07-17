using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.Data.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data
{
    [SqlTable( "tUser", Package = typeof( Package ) ), Versions( "1.0.0" )]
    public abstract partial class EventSchoolTable : SqlTable
    {
        void StObjConstruct(CustomGroupTable groupTable)
        {

        }

        [SqlProcedure( "sCreateEventSchool" )]
        public abstract Task<EventStruct> CreateEvent( ISqlCallContext ctx, string name, DateTime begDate, DateTime endDate, int timePeriodId, int actorId);

        [SqlProcedure( "sDeleteEventSchool" )]
        public abstract Task<EventStruct> DeleteEvent( ISqlCallContext ctx, int actorId, int eventId, bool forceDestroy = true );



    }
}
