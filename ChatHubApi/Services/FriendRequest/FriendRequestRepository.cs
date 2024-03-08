using ChatHubApi.Context;
using ChatHubApi.Models;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatHubApi.Services.FriendRequest
{
    public class FriendRequestRepository:GenericRepository<FriendShip>, IFriendRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public FriendRequestRepository(ApplicationDbContext context):base(context) {
           _context = context;
        }


        public async Task<FriendShip> UpdateMessageCount(string senderId, string friendId,bool IsIncrease)
        {
           
                FriendShip friendShip = await _context.FriendShips.FirstOrDefaultAsync(p => p.UserId == senderId && p.FriendId == friendId);
               if (friendShip != null)
                {
                    if (IsIncrease)
                    {
                        friendShip.UnReadMessagesCount++;
                    }
                else
                {
                    friendShip.UnReadMessagesCount = 0;
                }
                }
            return friendShip;
        }

        public async Task<List<FriendRequestViewModel>> GetAllFriends(string userId)
        {
            IQueryable<FriendRequestViewModel> friends = _context.FriendShips.Where(r => r.UserId == userId).Select(friendship => new FriendRequestViewModel()
            {
                FriendShipId = friendship.FriendShipId,
                UserId = friendship.UserId,
                FriendId = friendship.FriendId,
                UnReadMessagesCount = friendship.UnReadMessagesCount,
                FriendName = friendship.Friend.Name,
                FriendPhoneNumber = friendship.Friend.PhoneNumber,
                LastSeen = friendship.Friend.LastSeen,
                About = friendship.Friend.About,
                ImageUrl = friendship.Friend.ImageUrl,
            });

           return friends.ToList();
            
        }

        public async Task<int> GetUnReadMessageCount(string senderId, string friendId)
        {
            FriendShip friendShip = await _context.FriendShips.FirstOrDefaultAsync(p => p.UserId == senderId && p.FriendId == friendId);
            if(friendShip != null)
            {
                return friendShip.UnReadMessagesCount;

            }
            else
            {
                return 0;
            }
        }
    }
}
