using ChatHubApi.Models;
using Data.Models;

namespace ChatHubApi.Services.NotificationRepo
{
    public interface INotificationRepository:IGenericRepository<Notification>
    {
        public Task<List<Notification>> GetAllWithSender(string receiverId, bool IsSender);
    }
}
