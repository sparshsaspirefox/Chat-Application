﻿using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.FriendShip;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class Chats
    {
        [Inject]
        NavigationManager navigationManager { get; set; }
        string currentUserId;

        private HubConnection _hubConnection;

        [Inject]
        IChatHubService ChatHubService { get; set; }

        [Inject]
        IFriendService _friendService { get; set; }


        bool isBusy = false;
        [Inject]
        IAccountService _accountService { get; set; }

        public List<UserViewModel> users = new List<UserViewModel>();

        public List<FriendRequestViewModel> friends = new List<FriendRequestViewModel>();

        public List<UserViewModel> friendsDetails = new List<UserViewModel>();
        protected override async Task OnInitializedAsync()
        {
            isBusy = true;
            await InitilizeList();
            await CreateHubConnection();
            isBusy = false;
        }

        private async Task CreateHubConnection()
        {
            _hubConnection = await ChatHubService.CreateHubConnection();


            _hubConnection.On<string>("UpdateMessageCount", (sendTo) =>
            {
                foreach(var user in friends)
                {
                    if(user.FriendId == sendTo)
                    {
                        user.UnReadMessagesCount++;
                    }
                }
                this.InvokeAsync(() => this.StateHasChanged());

            });
        }

        private async Task InitilizeList()
        {
            GenericResponse<List<UserViewModel>> usersResponse = await _accountService.GetAllUsers();

            _hubConnection = await ChatHubService.CreateHubConnection();

            currentUserId = Preferences.Get("UserId", null);
            users = usersResponse.Data.Where(p => p.id != currentUserId).ToList();

            await _hubConnection.SendAsync("GetAllActiveUsers");
            GenericResponse<List<FriendRequestViewModel>> friendsResponse = await _friendService.GetAllFriends(currentUserId);
            friends = friendsResponse.Data;

            await UpdateActiveUsers();

            _hubConnection.On("UpdateActiveUsers", () =>
            {
                UpdateActiveUsers();
            });
        }

        private async Task UpdateActiveUsers()
        {   foreach(var friend in friends)
            {
                friend.IsOnline = false;
            }
            List<string> AllActiveUsers = _friendService.GetActiveUsers();
            foreach (var activeUserId in AllActiveUsers)
            {
                foreach (var friend in friends)
                {
                    if (friend.FriendId == activeUserId)
                    {
                        friend.IsOnline = true;
                    }
                   
                }
            }
            this.InvokeAsync(() => this.StateHasChanged());
        }

        private async Task Logout()
        {
            await _accountService.UpdateLastSeen(currentUserId);
            await _hubConnection.SendAsync("DisconnectOnLogout");

            Preferences.Clear();
            await _hubConnection.DisposeAsync();
            navigationManager.NavigateTo("/");
        }
        private void GoToNotification()
        {
            navigationManager.NavigateTo("/notifications");
        }
        private void GoToInDividualChat(string id,int unreadMessages)
        {
            navigationManager.NavigateTo($"individualChat/{id}/{unreadMessages}");
        }
    }
}