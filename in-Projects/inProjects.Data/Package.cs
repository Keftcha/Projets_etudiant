using CK.Setup;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data
{
    [SqlPackage(
    ResourcePath = "Res",
    Schema = "IPR",
    Database = typeof( SqlDefaultDatabase ),
    ResourceType = typeof( Package ) ),
    Versions( "1.0.0" )]
    public abstract class Package : SqlPackage
    {

        void StObjConstruct( CK.DB.User.UserPassword.Package userPasswordPackage)
        {

        }
    }
}
