using ChatHubApi.Context;
using ChatHubApi.Models;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatHubApi.Services.NotificationRepo
{
    public class NotificationRepository: GenericRepository<Notification>,INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllWithSender(string receiverId, bool IsSender)
        {
            if(IsSender)
            {
                return _context.Notifications.Include(m => m.Sender).Where(m => m.SenderId == receiverId).ToList();
            }
            return _context.Notifications.Include(m => m.Sender).Where(m => m.ReceiverId == receiverId).ToList();
        }

        
    }
}
