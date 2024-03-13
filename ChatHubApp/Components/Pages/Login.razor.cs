using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class Login
    {
        UserLoginModel userViewModel = new UserLoginModel();

        [Inject]
        NavigationManager navigationManager { get; set; }

        bool isBusy = false;
        [Inject]
        IAccountService _accountService { get; set; }

        string errorMessage = string.Empty;

        [Inject]
        IChatHubService chatHubService { get; set; }

        private HubConnection _hubConnection;
        protected override async Task OnInitializedAsync()
        {
            //if (Preferences.ContainsKey("Token"))
            //{
            //    await chatHubService.CreateHubConnection();
            //    navigationManager.NavigateTo("chats", replace: true);
            //}
        }
        
        private async void LoginUser()   
        {
            isBusy = true;

            errorMessage = string.Empty;
           
            var response = await _accountService.Login(userViewModel);
            if (response.Success)
            {
                Preferences.Set("Token", response.Message);
                Preferences.Set("UserId", response.Error);
                Preferences.Set("UserName", response.Data);
                await CreateHubConnection();
              //  await chatHubService.CreateHubConnection();
                navigationManager.NavigateTo("chats",replace:true);
            }
            else
            {
                errorMessage = response.Error;
            }
            isBusy = false;
            StateHasChanged();
        }

        private async Task CreateHubConnection()
        {
            _hubConnection = await chatHubService.CreateHubConnection();
            _hubConnection.On<MessageViewModel>("SendNotification", (message) =>
            {
                string description = string.Empty;
                if (message.ContentType == MessageType.Written.ToString())
                {
                    description = message.Content;
                }
                else if (message.ContentType == MessageType.Image.ToString())
                {
                    description = "New Image";
                }
                else
                {
                    description = "New document";
                }
                Random rnd = new Random();
                var request = new NotificationRequest
                {
                    NotificationId = rnd.Next(999999),
                    Title = message.SenderName,
                    Subtitle = "New Message",
                    Description = description,
                    BadgeNumber = 2,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now
                    }
                };
                LocalNotificationCenter.Current.Show(request);
            });
        }

        private void GoToRegister()
        {
            navigationManager.NavigateTo("register");
        }
    }
}
