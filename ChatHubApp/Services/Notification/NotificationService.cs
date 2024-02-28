using ChatHubApp.Helpers;
using ChatHubApp.HttpApiManager;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IApiManager _apiManager;

        public NotificationService(IApiManager apiManager)
        {
            _apiManager = apiManager;

        }
        public async Task<GenericResponse<string>> SendNotification(NotificationViewModel data)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Notification/NewNotification", data);
                return response;
            }catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<List<NotificationViewModel>>> GetAllNotifications(string receiverId, bool IsSender)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<List<NotificationViewModel>>>(AppConstants.baseAddress  + "/Notification/GetAllNotifications?receiverId=" + receiverId + "&IsSender=" + IsSender);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<NotificationViewModel>> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> UpdateNotification(NotificationViewModel data)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Notification/UpdateNotification", data);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }
    }
}
