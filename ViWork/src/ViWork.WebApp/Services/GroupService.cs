using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViWork.DAL;

namespace ViWork.WebApp.Services
{
    public class GroupService
    {
        readonly GroupGateaway _groupGateway;

        public GroupService(GroupGateaway groupGateway)
        {
            _groupGateway = groupGateway;
        }

        public async Task<UserData> FindGroupOwnnerById(int groupId)
        {
            UserData user = await _groupGateway.FindGroupOwnerById(groupId);
            if (user != null )
            {
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<UserData>> FindUserList(int groupId )
        {
            var userList = await _groupGateway.FindUserList(groupId);
            if (userList != null)
            {
                return userList;
            }
            return null;
        }

        public async Task<Result<int>> AddUsersGroup(int groupId, int userId)
        {
            return await _groupGateway.AddUserGroup(groupId, userId);
        }

        public async Task UpdateGroupName(int groupId, string groupName)
        {
             await _groupGateway.UpdateGroupName(groupId, groupName);

        }

        public async Task DeleteGroup(int groupId)
        {
            await _groupGateway.DeleteGroup(groupId);
        }

        public async Task DeleteUserInGroup(int groupId, int userId)
        {
            await _groupGateway.DeleteUserInGroup(groupId, userId);
        }
        
    }
}
