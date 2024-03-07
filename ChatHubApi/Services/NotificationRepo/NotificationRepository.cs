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

        public async Task<List<NotificationViewModel>> GetAllWithSender(string receiverId, bool IsSender)
        {
            if(IsSender)
            {
                return _context.Notifications.Where(m => m.SenderId == receiverId)
                .Select(m => new NotificationViewModel
                {
                    NotificationId = m.NotificationId,
                    Time = m.Time,
                    Status = m.Status,  
                    ReceiverId = m.ReceiverId,
                    NotificationType= m.NotificationType,
                    SenderName = m.Sender.Name,
                    SenderId = m.Sender.Id,
                    GroupName = m.Group.GroupName,
                    GroupId = m.Group.Id

                }).ToList();
            }
            return _context.Notifications.Where(m => m.ReceiverId == receiverId)
                .Select(m => new NotificationViewModel
                {
                    NotificationId = m.NotificationId,
                    Time = m.Time,
                    Status = m.Status,
                    ReceiverId = m.ReceiverId,
                    NotificationType = m.NotificationType,
                    SenderName = m.Sender.Name,
                    SenderId = m.Sender.Id,
                    GroupName = m.Group.GroupName,
                    GroupId = m.Group.Id
                }).ToList();
            
        }

        
    }
}
