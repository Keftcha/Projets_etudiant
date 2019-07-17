using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data
{
    [SqlTable( "tForumInfos", Package = typeof( Package ) )]
    [Versions( "1.0.0" )]
    [SqlObjectItem("vForumProjectInfos")]
    public abstract class ForumInfosTable : SqlTable
    {
        void StObjConstruct( ProjectStudentTable projectStudentTable )
        {

        }

        [SqlProcedure( "sCreateForumInfo" )]
        public abstract Task CreateForumInfo( ISqlCallContext context, int projectId, string classRoom, int coordinatesX, int coordinatesY, int width, int height, int forumNumber );

    }
}
