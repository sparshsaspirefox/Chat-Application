using ChatHubApp.Helpers;
using ChatHubApp.HttpApiManager;
using ChatHubApp.ViewModels;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IApiManager _apiManager;

        public AccountService(IApiManager apiManager)
        {
            _apiManager = apiManager;

        }
        public async Task<GenericResponse<string>> RegisterUser(UserViewModel model)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Account/CreateUser", model);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<List<UserViewModel>>> GetAllUsers()
        {
            try
            {

                var response = await _apiManager.GetAsync<GenericResponse<List<UserViewModel>>>(AppConstants.baseAddress + "/Account/GetAllUsers");
                
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<UserViewModel>> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> Login(UserLoginModel data)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Account/Login", data);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> Logout()
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Account/Logout");
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<UserViewModel>> GetUserById(string userId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<UserViewModel>>(AppConstants.baseAddress + "/Account/GetUserById?UserId=" + userId );
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<UserViewModel> { Success = false, Error = ex.Message };
            }
        }

        public async Task<GenericResponse<string>> UpdateLastSeen(string userId)
        {
            try
            {
                var response = await _apiManager.GetAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Account/UpdateLastSeen?UserId=" + userId);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        //have to modify
        public async Task<GenericResponse<string>> UploadProfileImage(MultipartFormDataContent content)
        {
            try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/File/UpdateLastSeen?UserId=" ,content);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }

        async Task<GenericResponse<string>> IAccountService.UpdateProfile(UserViewModel userViewModel)
        {
           try
            {
                var response = await _apiManager.PostAsync<GenericResponse<string>>(AppConstants.baseAddress + "/Account/UpdateProfile", userViewModel);
                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }
        }
    }
}
