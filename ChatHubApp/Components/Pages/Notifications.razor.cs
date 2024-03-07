using ChatHubApp.Services.FriendShip;
using ChatHubApp.Services.Notification;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class Notifications
    {

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        INotificationService _notificationService { get; set; }

        [Inject]
        IFriendService _friendService { get; set; }


        bool isBusy = false;
        public string userId = string.Empty;
        public List<NotificationViewModel> allNotifications = new List<NotificationViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await InitializeList();
        }
        private async Task InitializeList()
        {
            isBusy = true;
            userId =  Preferences.Get("UserId", null);
            var response = await _notificationService.GetAllNotifications(userId, false);
            allNotifications = response.Data.OrderByDescending( n => n.Time).ToList();
            isBusy = false;

        }
        private async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("goBack");
        }

        private async Task AcceptRequest(NotificationViewModel notificationViewModel)
        {

            UpdateRequest(notificationViewModel, RequestType.Accepted);
        }
        private async Task CancelRequest(NotificationViewModel notificationViewModel)
        {
            UpdateRequest(notificationViewModel, RequestType.Rejected);
        }
        private async void UpdateRequest(NotificationViewModel notificationViewModel, RequestType status)
        {
            
            if(status == RequestType.Accepted)
            {
                if (notificationViewModel.NotificationType == NotificationType.FriendRequest.ToString())
                {
                    FriendRequestViewModel newfriendRequest = new FriendRequestViewModel()
                    {
                        UserId = userId,
                        FriendId = notificationViewModel.SenderId,
                        RequestStatus = "Accepted",
                        UnReadMessagesCount = 0
                    };
                    _friendService.AddFriend(newfriendRequest);
                }
                else if(notificationViewModel.NotificationType == NotificationType.GroupRequest.ToString())
                {
                    //this is done in backend to and user group matching
                }
                

            }
            notificationViewModel.Status = status.ToString();
            _notificationService.UpdateNotification(notificationViewModel);
            await InitializeList();
            StateHasChanged();
        }
    }
}
