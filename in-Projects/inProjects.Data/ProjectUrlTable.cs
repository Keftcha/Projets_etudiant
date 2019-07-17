using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data
{
    [SqlTable( "tProjectUrl", Package = typeof( Package ) )]
    [Versions( "1.0.0" )]
    public abstract class ProjectUrlTable : SqlTable
    {
        void StObjConstruct(ProjectStudentTable projectStudentTable)
        {
           
        }

        [SqlProcedure( "sCreateOrUpdateProjectUrl" )]
        public abstract Task<int> CreateOrUpdateProjectUrl(ISqlCallContext ctx, int projectId, string url, string urlType );

    }
}
