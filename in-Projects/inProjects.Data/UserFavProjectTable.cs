using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data
{
    [SqlTable( "tUserFavProject", Package = typeof( Package ) )]
    [Versions( "1.0.0" )]

    public abstract class UserFavProjectTable : SqlTable
    {
        void StObjConstruct( CK.DB.Actor.UserTable userTable, ProjectStudentTable projectStudentTable)
        {

        }

        [SqlProcedure( "sUserFavUnfavProject" )]

        public abstract Task FavOrUnfavProject(ISqlCallContext ctx, int userId, int ProjectId );
    }
}
