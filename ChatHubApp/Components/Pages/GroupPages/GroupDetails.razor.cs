using ChatHubApp.Services.Group;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages.GroupPages
{
    public partial class GroupDetails
    {

        [Parameter]
        public int GroupId { get; set; }
        [Inject]
        IGroupService groupService { get; set; }
        public GroupViewModel currentGroup = new GroupViewModel();

        [Inject]
        IJSRuntime JSRuntime { get; set; }
        string loggedUserId = string.Empty;
        [Inject]
        NavigationManager navigationManager { get; set; }

        public List<UserViewModel> allGroupMembers = new List<UserViewModel>();
        protected override async Task OnInitializedAsync()
        {
            await IntializeData();
        }

        private async Task IntializeData()
        {
            loggedUserId = Preferences.Get("UserId", null);
            var response = await groupService.GetGroupById(GroupId);
            if (response.Success) currentGroup = response.Data;

            var membersRes = await groupService.GetGroupMembersDetails(GroupId);
            if (membersRes.Success) allGroupMembers = membersRes.Data;
        }

        private async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("goBack");
        }

        private async Task ExitGroup()
        {
            var res = await groupService.RemoveUserFromGroup(GroupId, loggedUserId);
            if (res.Success)
            {
                navigationManager.NavigateTo("/groups", replace: true);
            }
        }
    }
}
