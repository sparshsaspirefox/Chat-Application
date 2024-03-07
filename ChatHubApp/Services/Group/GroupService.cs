using ChatHubApp.Helpers;
using ChatHubApp.HttpApiManager;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Group
{
    public class GroupService : IGroupService
    {
        private readonly IApiManager _apiManager;

        public GroupService(IApiManager apiManager)
        {
            _apiManager = apiManager;

        }
        public async Task<GenericResponse<string>> NewGroup(GroupViewModel groupModel)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Group/NewGroup", groupModel);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<List<GroupViewModel>>> GetGroupsByUserId(string userId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<List<GroupViewModel>>>(AppConstants.baseAddress + "/Group/GetGroupsByUserId?userId=" + userId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<GroupViewModel>> { Success = false, Error = ex.Message };
            }

        }

        public async Task<GenericResponse<GroupViewModel>> GetGroupById(int groupId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<GroupViewModel>>(AppConstants.baseAddress + "/Group/GetGroupById?GroupId=" + groupId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<GroupViewModel> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> NewGroupMessage(GroupMessageViewModel groupMessageModel)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Group/NewGroupMessage", groupMessageModel);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<List<GroupMessageViewModel>>> GetAllGroupMessagesById(int groupId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<List<GroupMessageViewModel>>>(AppConstants.baseAddress + "/Group/GetAllGroupMessagesById?GroupId=" + groupId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<GroupMessageViewModel>> { Success = false, Error = ex.Message };
            }
           
        }

        public async Task<GenericResponse<List<UserViewModel>>> GetGroupMembersDetails(int groupId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<List<UserViewModel>>>(AppConstants.baseAddress + "/Group/GetGroupMembersDetails?GroupId=" + groupId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<UserViewModel>> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> RemoveUserFromGroup(int groupId, string userId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Group/RemoveUserFromGroup?GroupId=" + groupId + "&UserId=" + userId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> UpdateUnreadMessageCount(int groupId, string senderId, bool IsClearCount)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Group/UpdateUnreadMessageCount?GroupId=" + groupId + "&SenderId=" + senderId + "&IsClearCount=" + IsClearCount);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }
    }
}