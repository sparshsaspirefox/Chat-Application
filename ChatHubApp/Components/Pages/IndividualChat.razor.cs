using ChatHubApp.Helpers;
using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.FriendShip;
using ChatHubApp.Services.Message;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
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

        MessageViewModel newMessage = new MessageViewModel();

        List<MessageViewModel> allMessages = new List<MessageViewModel>();

        public int UnSeenMessagesCount;
        protected override async Task OnInitializedAsync()
        {
            await CreateHubConnection();
            await IntializeList();
           // await JSRuntime.InvokeVoidAsync("onScrollToTop");

        }
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
           
        }
        int pageNo;
        private async Task IntializeList()
        {

          //  isBusy = true;
            pageNo = 1;
           
            //var response = await _messageService.GetMessages(Preferences.Get("UserId", null), UserId);
            loggedUserId = Preferences.Get("UserId", null);

            var response = await _messageService.GetMessagesByFilter(loggedUserId, UserId, pageNo);
            allMessages = response.Data.OrderBy(p=>p.Time).ToList();
            if (response.Data.Count < 20)
            {
                noMoreData = true;
            }

            await _hubConnection.SendAsync("UpdateSeenMessages", loggedUserId, UserId);

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

        

        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(newMessage.Content))
            {
                return;
            }
            try
            {
                newMessage.ReceiverId = this.UserId;
                newMessage.SenderId = loggedUserId;
                newMessage.Time = DateTime.Now;
                await _messageService.NewMessage(newMessage);
                await _friendService.UpdateMessageCount(newMessage.SenderId, newMessage.ReceiverId, true);
               
                allMessages.Add(newMessage);
                UnreadMessages = 0; 
                UnSeenMessagesCount++;
                await ScrollToBottom();
                if (_hubConnection is not null)
                {
                    
                    await _hubConnection.SendAsync("SendMessage", newMessage);
                    await _hubConnection.SendAsync("UpdateMessageCount", newMessage.SenderId,newMessage.ReceiverId);
                }
                newMessage = new MessageViewModel();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        private bool noMoreData = false;
        private async Task LoadMoreMessages()
        {
            isLoadingMore = true;
            pageNo++;
            var response = await _messageService.GetMessagesByFilter(loggedUserId, UserId, pageNo);
          //  List<MessageViewModel> moreMessages = response.Data;
            if(response.Data.Count == 0)
            {
                noMoreData = true;
            }
            allMessages.AddRange(response.Data);
            allMessages = allMessages.OrderBy(p => p.Time).ToList();
            StateHasChanged();
            isLoadingMore = false;
        }
        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;

        private async Task GoBack()
        {
            await _friendService.UpdateMessageCount(UserId, loggedUserId, false);
            await JSRuntime.InvokeVoidAsync("goBack");
        }

        private async Task ScrollToBottom ()
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom");
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
            }
        }

        // Method to be called when the user scrolls to the top
        [JSInvokable]
        public async Task OnScrollToTop()
        {
            // Your logic when user scrolls to the top
            // For example:
            // await LoadPreviousData();
            // StateHasChanged();
        }
    }
}
