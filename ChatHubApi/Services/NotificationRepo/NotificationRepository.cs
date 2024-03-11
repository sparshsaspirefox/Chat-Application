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
                //get all notifictions for all users page
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
                    GroupId = m.Group.Id,
                    IsSeen = m.IsSeen

                }).ToList();
            }

            //update seen notifications

            IQueryable<Notification> unSeenNotifications = _context.Notifications.Where(n => n.ReceiverId == receiverId && n.IsSeen == false);
            foreach(var notification in unSeenNotifications)
            {
                notification.IsSeen = true;
            }
            _context.UpdateRange(unSeenNotifications);
            _context.SaveChanges();

            //get all notifications
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
                    GroupId = m.Group.Id,
                    IsSeen = m.IsSeen
                }).ToList();
            
        }

        async Task<int> INotificationRepository.GetNewNotificationsCount(string userId)
        {
            return await _context.Notifications.Where(n => n.ReceiverId ==  userId && n.IsSeen == false).CountAsync();
        }

       
    }
}
