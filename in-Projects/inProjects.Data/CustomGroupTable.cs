using CK.DB.Zone;
using CK.Setup;
using CK.SqlServer.Setup;

namespace inProjects.Data
{

    [SqlTable( "tGroup", Package = typeof( Package ) ), Versions( "1.0.0" )]

    public abstract class CustomGroupTable : GroupTable
    {
        CK.DB.Zone.SimpleNaming.Package _zoneNaming;
           
        void StObjConstruct( GroupTable groupTable, SchoolTable schoolTable, CK.DB.Zone.SimpleNaming.Package zN )
        {
            _zoneNaming = zN;
        }

        public CK.DB.Zone.SimpleNaming.Package Naming => _zoneNaming;

        //[InjectContract]
        //public CK.DB.Zone.SimpleNaming.Package ZZ { get; private set; }

    }
}
