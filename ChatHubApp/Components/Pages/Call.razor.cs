using ChatHubApp.Services.Account;
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
    public partial class Call
    {
        string currentUserId;

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
            isBusy = false;
        }

      

        private async Task InitilizeList()
        {
            GenericResponse<List<UserViewModel>> usersResponse = await _accountService.GetAllUsers();

            currentUserId = Preferences.Get("UserId", null);
            users = usersResponse.Data.Where(p => p.id != currentUserId).ToList();

            GenericResponse<List<FriendRequestViewModel>> friendsResponse = await _friendService.GetAllFriends(currentUserId);
            friends = friendsResponse.Data;


          
        }

      

       
    }
}
