using CK.Setup;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data
{
    [SqlTable( "tAcl", Package = typeof( Package ) ), Versions( "1.0.0" )]

    public abstract class CustomAclTable  : CK.DB.Acl.AclTable
    {
        void StObjConstruct(CustomGroupTable customGroupTable)
        {

        }

    }
}
