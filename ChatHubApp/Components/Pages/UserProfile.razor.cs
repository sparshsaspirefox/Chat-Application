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
        bool isEditable = true;
        bool isEditMode = false;

        private async void EditMode()
        {
            isEditMode = true;
        }
        private async void UpdateProfile()
        {
            isEditMode = false;
        }
        private async void CancelEditing()
        {
            isEditMode = false;
        }



        public string loggedUserId = string.Empty;
        private async Task InitializeData()
        {
            loggedUserId = Preferences.Get("UserId", null);
            GenericResponse<UserViewModel> userResponse;
           
            if (UserId == string.Empty)
            {
                isEditable = true;
                userResponse = await accountService.GetUserById(loggedUserId);
            }
            else
            {
                userResponse = await accountService.GetUserById(UserId);
            }
            user = userResponse.Data;
        }

        private int maxAllowedFiles = 1;
        private long maxFileSize = long.MaxValue;
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            using var content = new MultipartFormDataContent();
            foreach(var file in e.GetMultipleFiles())
            {
                var fileContent =  new StreamContent(file.OpenReadStream(maxFileSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                content.Add( content:fileContent,name:"\"files\"" ,fileName: file.Name);
            }

        }
        private async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("goBack");
        }
    }
}
