using ChatHubApp.Helpers;
using ChatHubApp.HttpApiManager;
using ChatHubApp.Services.ChatHub;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatHubApp.Services.FriendShip
{
    public class FriendService : IFriendService
    {
        private readonly IApiManager _apiManager;

        public static List<string> ActiveUsers = new List<string>();

        private readonly IChatHubService _chatHubService;
        private HubConnection _hubConnection;

        public List<string> GetActiveUsers()
        {
            return ActiveUsers;
        }
        public FriendService(IApiManager apiManager,IChatHubService chatHubService)
        {
            _chatHubService = chatHubService;
            _apiManager = apiManager;
            Task.Run(async () => await CreateHubConnection());
        }
        private async Task CreateHubConnection()
        {
            _hubConnection = await _chatHubService.CreateHubConnection();

            _hubConnection.On<List<string>>("ActiveUsers", (activeUsers) =>
            {
                ActiveUsers = activeUsers;
            });
        }

        public async Task<GenericResponse<List<FriendRequestViewModel>>> GetAllFriends(string userId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<List<FriendRequestViewModel>>>(AppConstants.baseAddress + "/FriendRequest/GetFriends?userId=" + userId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<FriendRequestViewModel>> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> AddFriend(FriendRequestViewModel data)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/FriendRequest/NewFriendRequest", data);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> UpdateMessageCount(string SenderId,string ReceiverId,bool IsIncrease)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<string>>(AppConstants.baseAddress + "/FriendRequest/UpdateMessageCount?SenderId=" + SenderId + "&ReceiverId=" + ReceiverId + "&IsIncrease=" + IsIncrease);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> GetUnReadMessageCount(string senderId, string friendId)
        {

            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<string>>(AppConstants.baseAddress + "/FriendRequest/GetUnReadMessageCount?SenderId=" + senderId + "&ReceiverId=" + friendId );
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

       
    }
}
