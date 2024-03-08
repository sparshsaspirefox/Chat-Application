using ChatHubApp.Helpers;
using ChatHubApp.Services.Account;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages
{
    public partial class UserProfile
    {
        [Parameter]
        public string UserId {  get; set; }

        [Inject]
        IAccountService accountService {  get; set; }

        UserViewModel user = new UserViewModel();
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await InitializeData();
        }
        bool isEditable = false;
        bool isEditMode = false;

        public string loggedUserId = string.Empty;
        private async Task InitializeData()
        {
            loggedUserId = Preferences.Get("UserId", null);
            GenericResponse<UserViewModel> userResponse;

            if (UserId.Equals("SelfProfile"))
            {
                isEditable = true;
                userResponse = await accountService.GetUserById(loggedUserId);
            }
            else
            {
                userResponse = await accountService.GetUserById(UserId);
            }
            user = userResponse.Data;

            //for form validation
            user.Password = "Admin@123";
            user.ConfirmPassword = "Admin@123";
        }
        private async void EditMode()
        {
            isEditMode = true;
        }

        bool IsUpdating = false;
        private async void UpdateProfile()
        { 
            IsUpdating = true;
            await accountService.UpdateProfile(user);
            isEditMode = false;
            IsUpdating = false;
            StateHasChanged();
        }
        private async void CancelEditing()
        {
            isEditMode = false;
        }

      

      
        private async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("goBack");
        }

        private async Task UploadImageUrl(string ImageUrl)
        {

            if (string.IsNullOrEmpty(ImageUrl))
            {
                return;
            }
            user.ImageUrl = ImageUrl;
            StateHasChanged();
        }

        string GetUrl(string imageUrl)
        {
            if(imageUrl != null) {
                return AppConstants.staticsFiles.ToString() + imageUrl;
            }
            return ".\\imagePlace.jpg";
        } 
    }
}
