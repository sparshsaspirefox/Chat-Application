using ChatHubApi.Models;
using Data.Models;

namespace ChatHubApi.Services.FriendRequest
{
    public interface IFriendRequestRepository:IGenericRepository<FriendShip>
    {
        public Task<FriendShip> UpdateMessageCount(string senderId, string friendId, bool IsIncrease);
        public Task<int> GetUnReadMessageCount(string senderId, string friendId);

        public Task<List<FriendRequestViewModel>> GetAllFriends(string userId);
    }
}
