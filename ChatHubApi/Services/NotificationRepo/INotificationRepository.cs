using ChatHubApi.Models;
using Data.Models;

namespace ChatHubApi.Services.NotificationRepo
{
    public interface INotificationRepository:IGenericRepository<Notification>
    {
        public Task<List<NotificationViewModel>> GetAllWithSender(string receiverId, bool IsSender);

        public Task<int> GetNewNotificationsCount(string userId);

      
    }
}
