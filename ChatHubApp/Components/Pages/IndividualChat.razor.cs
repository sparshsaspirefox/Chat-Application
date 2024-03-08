﻿using ChatHubApp.Helpers;
using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.FriendShip;
using ChatHubApp.Services.Message;
using CommunityToolkit.Maui.Core.Platform;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class IndividualChat
    {
        [Parameter]
        public string UserId { get; set; }

        [Parameter]
        public int UnreadMessages {  get; set; }

        private HubConnection _hubConnection;

        [Inject]
        IChatHubService ChatHubService { get; set; }

        UserViewModel currentUser = new UserViewModel();

        string loggedUserId = string.Empty;

        [Inject]
        IAccountService _accountService {  get; set; }

        [Inject]
        IFriendService _friendService { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        IMessageService _messageService { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }



        bool isBusy = false;
        bool isLoadingMore = false;

        string loggedUserName = string.Empty;

        MessageViewModel newMessage = new MessageViewModel();

        List<MessageViewModel> allMessages = new List<MessageViewModel>();

        public int UnSeenMessagesCount;
        protected override async Task OnInitializedAsync()
        {
            await CreateHubConnection();
            await IntializeList();
           // await JSRuntime.InvokeVoidAsync("onScrollToTop");

        }
        private ElementReference inputElementRef;

        private bool isTyping = false;


        private ElementReference elementRef;
        private bool _loading = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await inputElementRef.FocusAsync();

                await JSRuntime.InvokeVoidAsync("scrollHelper.initScrollListener", DotNetObjectReference.Create(this));
            }
        }


        

        int NotificationCount = 1;
        private async Task CreateHubConnection()
        {
            
            _hubConnection = await ChatHubService.CreateHubConnection();

            _hubConnection.On<MessageViewModel>("ReceiveMessage",  ( message) =>
            {
                if (message.SenderId == UserId)
                {
                    allMessages.Add(message);
                    ScrollToBottom();
                    this.InvokeAsync(() => this.StateHasChanged());
                }
            });
            _hubConnection.On<string>("UpdateSeenMessages", (sendTo) =>
            {
                if(UserId == sendTo)
                {
                    UnSeenMessagesCount = 0;
                }
                this.InvokeAsync(() => this.StateHasChanged());

            });

            _hubConnection.On<string>("ActiveChat", (loggedId) =>
            {
                if (loggedId == loggedUserId)
                {
                    UnSeenMessagesCount = 0;
                }
                this.InvokeAsync(() => this.StateHasChanged());
            });

            _hubConnection.On<string>("ShowTyping", (userId) =>
            {
                if (userId == UserId)
                {
                    isTyping = true;
                }
                this.InvokeAsync(() => this.StateHasChanged());
            });
            _hubConnection.On<string>("HideTyping", (userId) =>
            {
                if (userId == UserId)
                {
                    isTyping = false;
                }
                this.InvokeAsync(() => this.StateHasChanged());
            });

        }
        int pageNo;
        private async Task IntializeList()
        {

          //  isBusy = true;
            pageNo = 1;
            loggedUserName = Preferences.Get("UserName", null);

            //var response = await _messageService.GetMessages(Preferences.Get("UserId", null), UserId);
            loggedUserId = Preferences.Get("UserId", null);

            var response = await _messageService.GetMessagesByFilter(loggedUserId, UserId, pageNo);
            allMessages = response.Data.OrderBy(p=>p.Time).ToList();
            if (response.Data.Count < 20)
            {
                noMoreData = true;
            }

            await _hubConnection.SendAsync("UpdateSeenMessages", loggedUserId, UserId);
            await _hubConnection.SendAsync("AddActiveChats", loggedUserId, UserId);

            var userResponse = await _accountService.GetUserById(UserId);
            currentUser = userResponse.Data;

            var unSeenMsgResponse = await _friendService.GetUnReadMessageCount(loggedUserId, UserId);
            if(unSeenMsgResponse.Success = true)
            {
                UnSeenMessagesCount = int.Parse(unSeenMsgResponse.Data);
            }
            //clear unreadmesage count
            await _friendService.UpdateMessageCount(UserId, loggedUserId, false);
            StateHasChanged();

          
           // isBusy = false;

            await ScrollToBottom();
        }
        string bottomHeight = "30px";
    


        private async Task KeyboardEventHandler()
        {
            await _hubConnection.SendAsync("ShowTyping", loggedUserId, UserId);
        }
        private async Task OnUnfocusOnInput()
        {
            await _hubConnection.SendAsync("HideTyping", loggedUserId, UserId);
        }
        private async Task GoToProfile()
        {
            navigationManager.NavigateTo($"profilePage/{UserId}");
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


        private async Task SendMessage(string documentUrl = null,MessageType messageType = MessageType.Written)
        {
            if (!String.IsNullOrEmpty(documentUrl))
            {
                newMessage.Content = documentUrl;
            }

            if (string.IsNullOrEmpty(newMessage.Content.Trim()))
            {
                return;
            }
            try
            {
                newMessage.ReceiverId = this.UserId;
                newMessage.SenderId = loggedUserId;
                newMessage.Time = DateTime.Now;
                newMessage.SenderName = loggedUserName;
                newMessage.ContentType = messageType.ToString();
                UnreadMessages = 0;
                UnSeenMessagesCount++;
                allMessages.Add(newMessage);
                await _messageService.NewMessage(newMessage);
                await _friendService.UpdateMessageCount(newMessage.SenderId, newMessage.ReceiverId, true);
               
                
                await ScrollToBottom();
                if (_hubConnection is not null)
                {
                    if(_hubConnection.State == HubConnectionState.Connected)
                    {
                        await _hubConnection.SendAsync("SendMessage", newMessage);
                        await _hubConnection.SendAsync("UpdateMessageCount", newMessage.SenderId, newMessage.ReceiverId);
                        await _hubConnection.SendAsync("IsChatActive", newMessage.SenderId, newMessage.ReceiverId);
                    }
                }
                newMessage = new MessageViewModel();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            

        }

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
        private bool noMoreData = false;



        [JSInvokable]
        public async Task LoadMoreMessages()
        {
            if (isLoadingMore || noMoreData) return;
            isLoadingMore = true;
            StateHasChanged();
            pageNo++;
            var response = await _messageService.GetMessagesByFilter(loggedUserId, UserId, pageNo);
            //  List<MessageViewModel> moreMessages = response.Data;
            if (response.Data.Count == 0)
            {
                noMoreData = true;
            }
            allMessages.AddRange(response.Data);
            allMessages = allMessages.OrderBy(p => p.Time).ToList();
            
            isLoadingMore = false;
            StateHasChanged();
        }
      
        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;

        private async Task GoBack()
        {
            AppConstants.ActiveTab = "chats";
            await _hubConnection.SendAsync("RemoveActiveChats", loggedUserId, UserId);
            await _friendService.UpdateMessageCount(UserId, loggedUserId, false);
            await JSRuntime.InvokeVoidAsync("goBack");
        }

        private async Task ScrollToBottom ()
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom");
        }

        // coverting relative url to absolute
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
