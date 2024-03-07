using ChatHubApp.Services.Account;
using ChatHubApp.Services.Group;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Pages.GroupPages
{

  
    public partial class NewGroup
    {
        [Inject]
        IAccountService _accountService { get; set; }
        string currentUserId;

        [Inject]
        IGroupService groupService { get; set; }

        [Inject]
        DialogService dialogService { get; set; }
        public List<UserViewModel> users { get; set; }

        public GroupViewModel newGroup = new GroupViewModel();

        
        
        protected override async Task OnInitializedAsync()
        {
            await IntializeData();
        }

        private async Task IntializeData()
        {
            GenericResponse<List<UserViewModel>> usersResponse = await _accountService.GetAllUsers();

            currentUserId = Preferences.Get("UserId", null);
            users = usersResponse.Data.Where(p => p.id != currentUserId).ToList();
        }

        private async Task LoadData(LoadDataArgs args)
        {
            var query = await _accountService.GetAllUsers();
            List<UserViewModel>filterData = new List<UserViewModel>();
            if (!string.IsNullOrEmpty(args.Filter))
            {
                filterData = query.Data.Where(p => ( p.id != currentUserId && ( p.Name.Contains(args.Filter, StringComparison.CurrentCultureIgnoreCase)))).ToList();
                users = filterData;
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task OnSubmit()
        {
            newGroup.AdminId = currentUserId;
            var response = await groupService.NewGroup(newGroup);
            if(response.Success)
            {
                dialogService.Close();
            }
        }
    }
}
