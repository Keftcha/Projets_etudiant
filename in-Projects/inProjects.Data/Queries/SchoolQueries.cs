using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.Data.School;
using System;
using System.Collections.Generic;
using Dapper;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data.Queries
{
   public class SchoolQueries
   {
        private ISqlConnectionController _controller;


        public SchoolQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );
        }

        public async Task<List<SchoolData>> GetAllSchool( )
        {
            IEnumerable<SchoolData> result = await _controller.QueryAsync<SchoolData>( "SELECT SchoolId, [Name] FROM IPR.tSchool ORDER BY SchoolId;");
            return result.AsList();
        }
    }
}
