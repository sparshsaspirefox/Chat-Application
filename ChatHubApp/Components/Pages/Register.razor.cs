using ChatHubApp.Services.Account;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ChatHubApp.Components.Pages
{
    public partial class Register
    {
        UserViewModel userViewModel = new UserViewModel();

        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        IAccountService _register { get; set; }

        bool isBusy = false;
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        private async Task RegisterUser()
        {
            isBusy = true;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = "Please wait ";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
            var response = await _register.RegisterUser(userViewModel);

             if(response.Success == true) {
                CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();

                string text2 = "Registered Successfull";
                ToastDuration duration2 = ToastDuration.Short;
                double fontSize2 = 14;

                var toast2 = Toast.Make(text2, duration2, fontSize2);

                await toast2.Show(cancellationTokenSource2.Token);

                navigationManager.NavigateTo("/");
            }
             isBusy= false;
        }

        private async Task GoToLogin()
        {
            await JSRuntime.InvokeVoidAsync("goBack");
        }
    }
}
