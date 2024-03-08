using ChatHubApp.Helpers;
using ChatHubApp.Services.Account;
using ChatHubApp.Services.FriendShip;
using ChatHubApp.Services.Notification;
using ChatHubApp.ViewModels;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class AllUsers
    {
        [Inject]
        NavigationManager navigationManager {  get; set; }

        [Inject]
        IAccountService _accountService { get; set; }

        [Inject]
        INotificationService _notificationService { get; set; }

        bool isBusy = false;
        
        public List<UserViewModel> users = new List<UserViewModel>();
      
        public List<NotificationViewModel> notifications = new List<NotificationViewModel>();

        string currentUserId;
        protected override async Task OnInitializedAsync()
        {
            isBusy = true;
            await InitializeList();
            isBusy = false;
        }

        private async Task InitializeList()
        {
            GenericResponse<List<UserViewModel>> usersResponse = await _accountService.GetAllUsers();
           
            currentUserId =  Preferences.Get("UserId", null);
            users = usersResponse.Data.Where(p => p.id != currentUserId).ToList();

            GenericResponse<List<NotificationViewModel>> notificationResponse = await _notificationService.GetAllNotifications(currentUserId,true);
            notifications = notificationResponse.Data;

            
            foreach (var friend in notifications)
            {
                foreach (var user in users)
                {
                    if (user.id.Equals(friend.ReceiverId))
                    {
                        user.FriendStatus = friend.Status;
                    }
                }
            }

        }

        private async void GoToIndividualChat(UserViewModel user)
        {
            navigationManager.NavigateTo($"individualChat/{user.id}");
        }

        private void Logout()
        {
            navigationManager.NavigateTo("/");
        }
        private void GoToNotification()
        {
            navigationManager.NavigateTo("/notifications");
        }
        private async Task SendRequest(UserViewModel userViewModel)
        {
            NotificationViewModel newRequestNotification = new NotificationViewModel()
            {
                SenderId = currentUserId,
                ReceiverId = userViewModel.id,
                Status = RequestType.Pending.ToString(),
                Time = DateTime.Now,
                NotificationType = NotificationType.FriendRequest.ToString()
            };
            _notificationService.SendNotification(newRequestNotification);

          
            await InitializeList();
        }
        string GetUrl(string imageUrl)
        {
            if (imageUrl != null)
            {
                return AppConstants.staticsFiles.ToString() + imageUrl;
            }
            return ".\\imagePlace.jpg";
        }
    }
}
