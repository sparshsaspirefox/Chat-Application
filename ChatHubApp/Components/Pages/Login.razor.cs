using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Data.Models;
using Microsoft.AspNetCore.Components;
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

        protected override async Task OnInitializedAsync()
        {
            //isBusy = false;
        }
        
        private async Task LoginUser()   
        {
            isBusy = true;

            errorMessage = string.Empty;
           
            var response = await _accountService.Login(userViewModel);
            if (response.Success)
            {
                Preferences.Set("Token", response.Message);
                Preferences.Set("UserId", response.Error);
                Preferences.Set("UserName", response.Data);
                await chatHubService.CreateHubConnection();
                navigationManager.NavigateTo("chats");
            }
            else
            {
                errorMessage = response.Error;
            }
            isBusy = false;
           
        }
        private void GoToRegister()
        {
            navigationManager.NavigateTo("register");
        }
    }
}
