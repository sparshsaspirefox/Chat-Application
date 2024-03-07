using ChatHubApp.Services.ChatHub;
using ChatHubApp.Services.Group;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages.GroupPages
{
    public partial class Groups
    {
        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        DialogService dialogService { get; set; }


        private string loggedUserId = string.Empty;

        private bool isBusy = false;
        public List<GroupViewModel> groups { get; set; }

        private HubConnection _hubConnection;
        [Inject]
        IChatHubService ChatHubService { get; set; }

        [Inject]
        IGroupService groupService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitilizeData();
            await CreateHubConnection();
        }

        private async Task CreateHubConnection()
        {
            _hubConnection = await ChatHubService.CreateHubConnection();

            _hubConnection.On<int>("UpdateGroupMessageCount", (groupId) =>
            {
                foreach (var group in groups)
                {
                    if (group.Id == groupId)
                    {
                        group.UnreadMessageCount++;
                    }
                }
                this.InvokeAsync(() => this.StateHasChanged());

            });
        }
        private async Task InitilizeData()
        {
            isBusy = true;
            loggedUserId = Preferences.Get("UserId", null);
            if(loggedUserId == null) { return; }
            var res = await groupService.GetGroupsByUserId(this.loggedUserId);
            if (res.Success)
            {
                groups = res.Data;
            }
            isBusy = false; 
        }

        private async Task NewGroup()
        {
            await dialogService.OpenAsync<NewGroup>("New Group",
               new Dictionary<string, object>() { },
               new DialogOptions() { Width = "300px", Height = "350px" });
        }

        private void GoToGroupChat(int groupId)
        {
            navigationManager.NavigateTo($"groupChat/{groupId}");
        }
    }
}
