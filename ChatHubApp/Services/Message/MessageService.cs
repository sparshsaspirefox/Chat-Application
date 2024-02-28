using ChatHubApp.Helpers;
using ChatHubApp.HttpApiManager;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IApiManager _apiManager;

        public MessageService(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        public async Task<GenericResponse<string>> NewMessage(MessageViewModel message)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Message/NewMessage", message);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<List<MessageViewModel>>> GetMessages(string senderId, string receiverId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse< List < MessageViewModel >>> (AppConstants.baseAddress + "/Message/GetAllMessages?senderId=" + senderId + "&receiverId=" + receiverId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<MessageViewModel>> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<List<MessageViewModel>>> GetMessagesByFilter(string senderId, string receiverId, int pageNo)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<List<MessageViewModel>>>(AppConstants.baseAddress + "/Message/GetMessagesByFilter?senderId=" + senderId + "&receiverId=" + receiverId + "&pageNo=" + pageNo);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<MessageViewModel>> { Success = false, Error = ex.Message };
            }
        }
    }
}