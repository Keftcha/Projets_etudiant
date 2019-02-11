using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ViWork.DAL
{
    public class SchemaGateaway
    {
        readonly string _connectionString;

        public SchemaGateaway(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task<IEnumerable<SchemaData>> FindById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<SchemaData>(
                    "select s.SchemaId, s.SchemaName, s.GroupId, g.GroupName " +
                    "from viw.vSchema s " +
                    "join viw.vGroup g on g.GroupId = s.GroupId " +
                    "join viw.tOwnGroup o on o.GroupId = g.GroupId " +
                    "where UserId = @UserId",
                    new { UserId = userId});
            }
        }

        public async Task<SchemaData> FindByName(string schemaName)
        {
            using (SqlConnection con = new SqlConnection(_connectionString)) 
            {
                    return await con.QueryFirstOrDefault(
                        "Select * from viw.tSchema where SchemaId = @SchemaId",
                        new { SchemaName = schemaName });
            }            
        }

        public async Task<GroupData> FindSchemaGroup(int schemaId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefault(
                    "Select g.GroupName , g.GroupId from viw.tSchema s " +
                    "Join viw.tLnkSchema l on s.SchemaID = s.SchemaId" +
                    "Join viw.tGroup g on l.groupId = g.GroupId" +
                    "Where s.SchemaId = @SchemaId",
                    new { SchemaId = schemaId });
            }
        }

        public async Task<Result> AddSchema(string name, int groupId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@SchemaName", name);
                p.Add("@GroupId", groupId);
                p.Add("@Statut", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await con.ExecuteAsync("viw.sSchemaAdd", p, commandType: CommandType.StoredProcedure);
                int status = p.Get<int>("@Statut");
                if (status == 1) return Result.Failure<int>(Status.BadRequest, "An schema with this name already exists.");

                Debug.Assert(status == 0);
                return Result.Success();
            }
        }

        public async Task<Result> DeleteSchema(int schemaId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
               await con.ExecuteAsync("viw.sSchemaDelete", new { SchemaId = schemaId }, commandType: CommandType.StoredProcedure);
                return Result.Success();
            }
        }
        
    }
}
