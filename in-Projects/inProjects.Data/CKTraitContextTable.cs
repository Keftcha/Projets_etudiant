using CK.Setup;
using CK.SqlServer.Setup;


namespace inProjects.Data.Res.Model
{
    [SqlTable( "tCKTraitContext", Package = typeof( Package ) ), Versions( "1.0.0" )]

    public abstract  class TraitContextTable : CK.DB.SqlCKTrait.CKTraitContextTable
    {
        void StObjConstruct( )
        {

        }

    }
}
