using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;



namespace ViWork.DAL
{
    public class GroupGateaway
    {
        readonly string _connectionString;

        public GroupGateaway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<GroupData>> FindGroupById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<GroupData>(
                    "select g.GroupName, m.GroupId, m.UserId from viw.tOwnGroup m join viw.tGroup g on g.GroupId = m.GroupId where m.UserId = @UserId",
                    new { UserId = userId });
            }
        }


        public async Task<IEnumerable<GroupData>> FindAllGroupById(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<GroupData>(
                    "select g.GroupName, m.GroupId from viw.tMemberList m join viw.tGroup g on g.GroupId = m.GroupId where m.UserId = @UserId",
                    new { UserId = userId });
            }
        }

        public async Task<UserData> FindGroupOwnerById(int groupId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<UserData>(
                    "select u.UserID, u.FirstName, u.LastName, u.Email g.GroupName from viw.vGroup g " +
                    "Join viw.vOwnGroup o on @GroupId = o.GroupId " +
                    "Join viw.vUser u on o.UserId = u.UserID ",
                    new { GroupId = groupId });
            }
        }

        public async Task<IEnumerable<UserData>> FindUserList(int groupId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<UserData>(
                    " select u.FirstName, u.LastName from viw.tUserList " +
                    "join viw.tUser on m.UserId = u.UserId" +
                    "where m.GroupId = @GroupId",
                    new { GroupId = groupId });
            }
        }

        public async Task<Result> AddGroup(int userId ,string groupName)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@GroupName", groupName);
                p.Add("@UserID", userId);
                p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await con.ExecuteAsync("viw.sGroupAdd", p, commandType: CommandType.StoredProcedure);
                int status = p.Get<int>("@Status");
                if (status == 1) return Result.Failure<int>(Status.BadRequest, "A group with this name already exists.");

                Debug.Assert(status == 0);
                return Result.Success();
            }
        }

        public async Task<Result<int>> AddUserGroup(int groupId, int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@GroupId", groupId);
                p.Add("@UserId", userId);
                p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await con.ExecuteAsync("sUserGroupAdd", p, commandType: CommandType.StoredProcedure);

                int status = p.Get<int>("@Staus");
                if (status == 1) return Result.Failure<int>(Status.BadRequest, "A group with this name already exists.");
                Debug.Assert(status == 0);
                return Result.Success(p.Get<int>("@GroupId"));
            }
        }

        public async Task<Result> UpdateGroupName(int groupId, string groupName)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync(
                    "viw.sGroupNameUpdate",
                    new { GroupId = groupId, GroupName = groupName },
                    commandType: CommandType.StoredProcedure);
                return Result.Success();
            }
        }

        public async Task<Result> DeleteGroup(int groupId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync(
                    "viw.sGroupDelete",
                    new { GroupId = groupId },
                    commandType: CommandType.StoredProcedure);
                return Result.Success();
            }
        }

        public async Task<Result> DeleteUserInGroup(int groupID, int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.ExecuteAsync(
                    "viw.GroupUserDelete",
                    new { GroupId = groupID, UserId = userId },
                    commandType: CommandType.StoredProcedure);
                return Result.Success();
            }
        }

        //public async Task<GroupData> GetGroupData(int groupId)
        //{
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        return await con.QueryAsync<GroupData>( " select g.GroupName, g.GroupId from viw.tGroup where g.GroupId = @GroupId",
        //            new { GroupId = groupId });
        //    }
        //}

        public async Task<GroupData> GetGroupData(int groupId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<GroupData>(
                    "select g.GroupName, g.GroupId from viw.tGroup g " +
                    "where g.GroupId = @GroupId ",
                    new { GroupId = groupId });
            }
        }

    }
}
