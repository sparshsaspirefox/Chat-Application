using ChatHubApp.Helpers;
using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.Group;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages.GroupPages
{
    public partial class GroupChat
    {
        [Parameter]
        public int GroupId { get; set; }


        public string loggedUserId = string.Empty;
        public string loggedUserName = string.Empty;
        public GroupViewModel currentGroup = new GroupViewModel();

        GroupMessageViewModel newMessage = new GroupMessageViewModel();

        [Inject]
        IGroupService groupService { get; set; }

        List<GroupMessageViewModel> allMessages = new List<GroupMessageViewModel>();
        private HubConnection _hubConnection;


        [Inject]
        IChatHubService ChatHubService { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await CreateHubConnection();
            await IntializeData();
        }
        private async Task CreateHubConnection()
        {

            _hubConnection = await ChatHubService.CreateHubConnection();

            _hubConnection.On<GroupMessageViewModel>("ReceiveGroupMessage", (message) =>
            {
                if (message.GroupId == this.GroupId)
                {
                    allMessages.Add(message);
                    ScrollToBottom();
                    this.InvokeAsync(() => this.StateHasChanged());
                }
                //else
                //{
                //    string description = string.Empty;
                //    if (message.ContentType == MessageType.Written.ToString())
                //    {
                //        description = message.Content;
                //    }
                //    else if (message.ContentType == MessageType.Image.ToString())
                //    {
                //        description = "New Image";
                //    }
                //    else
                //    {
                //        description = "New document";
                //    }

                //    var request = new NotificationRequest
                //    {
                //        NotificationId = 1000,
                //        Title = message.SenderId,
                //        Subtitle = "New Message",
                //        Description = description,
                //        BadgeNumber = 42,
                //        Schedule = new NotificationRequestSchedule
                //        {
                //            NotifyTime = DateTime.Now
                //        }
                //    };
                //    LocalNotificationCenter.Current.Show(request);
                //}
            });
        }

        private async Task IntializeData()
        {
            loggedUserId = Preferences.Get("UserId", null);
            loggedUserName = Preferences.Get("UserName", null);
            await groupService.UpdateUnreadMessageCount(GroupId, loggedUserId, true);
            var response = await groupService.GetGroupById(GroupId);
            if (response.Success) currentGroup = response.Data;
             
            var msgResponse = await groupService.GetAllGroupMessagesById(GroupId);
            if(msgResponse.Success) allMessages = msgResponse.Data;
            StateHasChanged();

            await ScrollToBottom();
        }
        private async Task ScrollToBottom()
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom");
        }

        private async Task GoToGroupDetails()
        {
            navigationManager.NavigateTo($"/groupDetials/{GroupId}");
        }
        private async Task GoBack()
        {
            AppConstants.ActiveTab = "groups";
            await groupService.UpdateUnreadMessageCount(GroupId, loggedUserId, true);
            await JSRuntime.InvokeVoidAsync("goBack");
        }
        private string GetUrl(string url) => AppConstants.staticsFiles.ToString() + url;

        private async Task OpenPdf(string documentUrl)
        {
            string fullDocumentUrl = GetUrl(documentUrl);
            byte[] pdfDocumentInBytes = null;
            using (var webClient = new WebClient())
            {
                pdfDocumentInBytes = webClient.DownloadData(fullDocumentUrl);
            }
            DocumentViewer.OpenDocumentInViewer(pdfDocumentInBytes);
        }

        private async Task SendNewMessage()
        {
            await SendMessage();
        }

        private async Task UploadPdfUrl(string pdfUrl)
        {
            if (string.IsNullOrEmpty(pdfUrl))
            {
                return;
            }

            await SendMessage(pdfUrl, MessageType.Pdf);

        }
        private async Task UploadImageUrl(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl))
            {
                return;
            }
            await SendMessage(imgUrl, MessageType.Image);

        }
        private async Task SendMessage(string documentUrl = null, MessageType messageType = MessageType.Written)
        {
            if (!String.IsNullOrEmpty(documentUrl))
            {
                newMessage.Content = documentUrl;
            }

            if (string.IsNullOrEmpty(newMessage.Content))
            {
                return;
            }
            try
            {
                newMessage.GroupId = this.GroupId;
                newMessage.SenderId = loggedUserId;
                newMessage.Time = DateTime.Now;
                newMessage.ContentType = messageType.ToString();
                newMessage.SenderName = loggedUserName;
                allMessages.Add(newMessage);
                await groupService.NewGroupMessage(newMessage);
                await groupService.UpdateUnreadMessageCount(GroupId, loggedUserId, false);
                await ScrollToBottom();
                if (_hubConnection is not null)
                {
                    if (_hubConnection.State == HubConnectionState.Connected)
                    {
                        await _hubConnection.SendAsync("SendGroupMessage", newMessage, currentGroup.GroupUsersIds);
                        await _hubConnection.SendAsync("UpdateGroupMessageCount",newMessage, currentGroup.GroupUsersIds);
                    }
                }
                newMessage = new GroupMessageViewModel();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


        }

    }
}
