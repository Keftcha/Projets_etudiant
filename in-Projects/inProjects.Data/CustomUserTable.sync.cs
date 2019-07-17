using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace inProjects.Data
{
    public abstract partial class CustomUserTable
    {
        [SqlProcedure( "transform:sUserCreate" )]
        public abstract int CreateUser( ISqlCallContext context, int actorId, string userName, string firstName, string lastName );
    }
}
