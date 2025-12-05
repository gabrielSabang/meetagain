using MeetAgain.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetAgain.Server.Services
{
    public class GroupService
    {
        private readonly FirestoreService _fs;
        private readonly AuthService _auth;

        public GroupService(FirestoreService fs, AuthService auth)
        {
            _fs = fs;
            _auth = auth;
        }

        public async Task<string> CreateGroupAsync(string groupName)
        {
            var userId = _auth.UserId;
            if (userId == null) return string.Empty;

            var group = new Group
            {
                Id = Guid.NewGuid().ToString("N"),
                OwnerId = userId,
                Name = groupName,
                CreatedAt = DateTime.UtcNow.ToString("o")
            };

            await _fs.CreateGroupAsync(group);
            return group.Id;
        }

        public async Task<List<Group>> GetMyGroupsAsync()
        {
            var userId = _auth.UserId;
            if (userId == null) return new List<Group>();

            return await _fs.GetGroupsByOwnerAsync(userId);
        }

        public async Task AddMemberAsync(string groupId, string userToAdd)
        {
            var member = new GroupMember
            {
                UserId = userToAdd,
                AddedAt = DateTime.UtcNow.ToString("o")
            };

            await _fs.AddMemberAsync(groupId, member);
        }

        public Task<List<GroupMember>> GetMembersAsync(string groupId)
        {
            return _fs.GetGroupMembersAsync(groupId);
        }
    }
}
