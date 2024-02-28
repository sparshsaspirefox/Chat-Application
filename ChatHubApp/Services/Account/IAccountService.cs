using ChatHubApp.ViewModels;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Account
{
    public interface IAccountService
    {
        public Task<GenericResponse<string>> RegisterUser(UserViewModel data);

        public Task<GenericResponse<string>> Login(UserLoginModel data);

        public Task<GenericResponse<string>> Logout();
        public Task<GenericResponse<List<UserViewModel>>> GetAllUsers();

        public Task<GenericResponse<UserViewModel>> GetUserById(string userId);
        public Task<GenericResponse<string>> UpdateLastSeen(string userId);
    }
}
