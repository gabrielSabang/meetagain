using MeetAgain.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetAgain.Server.Services
{
    public class FriendService
    {
        private readonly FirestoreService _fs;
        private readonly AuthService _auth;

        public FriendService(FirestoreService fs, AuthService auth)
        {
            _fs = fs;
            _auth = auth;
        }

        public async Task AddFriendAsync(string friendUserId, string friendName, string friendEmail)
        {
            var myUserId = _auth.UserId;
            if (myUserId == null) return;

            var friend = new Friend
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = friendName,
                Email = friendEmail,
                AddedAt = DateTime.UtcNow.ToString("o")
            };

            await _fs.AddFriendAsync(friend);
        }

        public async Task<List<Friend>> GetFriendsAsync()
        {
            var myUserId = _auth.UserId;
            if (myUserId == null) return new List<Friend>();

            return await _fs.GetFriendsAsync(myUserId);
        }

        public async Task RemoveFriendAsync(string friendId)
        {
            if (string.IsNullOrWhiteSpace(friendId)) return;
            await _fs.RemoveFriendAsync(friendId);
        }
    }
}
