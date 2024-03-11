using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Notification
{
    public interface INotificationService
    {
        public Task<GenericResponse<string>> SendNotification(NotificationViewModel data);
        public Task<GenericResponse<List<NotificationViewModel>>> GetAllNotifications(string receiverId,bool IsSender);

        public Task<GenericResponse<string>> UpdateNotification(NotificationViewModel data);

        public Task<GenericResponse<int>> GetNewNotificationsCount(string userId);
    }
}
