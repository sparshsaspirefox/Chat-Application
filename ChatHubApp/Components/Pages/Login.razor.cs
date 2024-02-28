using ChatHubApp.Services.Account;
using ChatHubApp.Services.ChatHub;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Data.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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
        private async Task LoginUser()
        {
            isBusy = true;

            errorMessage = string.Empty;
            var response = await _accountService.Login(userViewModel);
            if (response.Success == true)
            {
                Preferences.Set("Token", response.Message);
                Preferences.Set("UserId", response.Error);
            
                string abc = Preferences.Get("Token", null);
                string abc1 = Preferences.Get("UserId", null);
                chatHubService.CreateHubConnection();
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
