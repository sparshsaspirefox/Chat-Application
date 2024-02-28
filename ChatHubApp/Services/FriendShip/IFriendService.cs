using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.FriendShip
{
    public interface IFriendService
    {
        public List<string> GetActiveUsers();
        public Task<GenericResponse<string>> AddFriend(FriendRequestViewModel data);
        public Task<GenericResponse<List<FriendRequestViewModel>>> GetAllFriends(string userId);

        public Task<GenericResponse<string>> GetUnReadMessageCount(string senderId, string friendId);
        public Task<GenericResponse<string>> UpdateMessageCount(string SenderId, string ReceiverId, bool IsIncrease);
    }
}
