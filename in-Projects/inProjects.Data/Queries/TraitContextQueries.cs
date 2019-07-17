using CK.SqlServer;
using CK.SqlServer.Setup;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data.Queries
{
    public class TraitContextQueries
    {
        private ISqlConnectionController _controller;

        public TraitContextQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );

        }
        public async Task<int> GetTraitContextId( string contextName )
        {
            return await _controller.QueryFirstAsync<int>(
                "SELECT tc.CKTraitContextId FROM CK.tCKTraitContext tc WHERE tc.ContextName = @contextName",
                new { contextName = contextName }
            );
        }
    }
}
