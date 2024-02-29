using ChatHubApi.Context;
using ChatHubApi.Models;
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

        public async Task<List<FriendShip>> GetAllFriends(string userId)
        {
            
           return await _context.FriendShips.Include(r => r.Friend).Where(r => r.UserId == userId).ToListAsync();
            
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
