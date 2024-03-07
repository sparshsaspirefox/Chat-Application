using AutoMapper;
using ChatHubApi.Models;
using ChatHubApi.Services;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatHubApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
        }

        [HttpGet]
        public async Task<GenericResponse<List<User>>> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();
                return new GenericResponse<List<User>> { Data = users };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<User>> { Success = false, Error = ex.Message };
            }
        }

        //register
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel userViewModel)
        {
            User newUser = new User
            {
                Name = userViewModel.Name,
                PhoneNumber = userViewModel.PhoneNumber,
                PasswordHash = userViewModel.Password,
                UserName = userViewModel.PhoneNumber
            };
            User userExist = await _userManager.FindByNameAsync(userViewModel.PhoneNumber);
            if (userExist != null)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = "Mobile number already exists" }); ;
            }
            try
            {
                var result = await _userManager.CreateAsync(newUser, userViewModel.Password);
                if (!result.Succeeded)
                {
                    return Ok(new GenericResponse<string> { Success = false,Error="server problem" });
                }
                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel userModel)
        {
            var user = await _userManager.FindByNameAsync(userModel.PhoneNumber);
            if (user != null &&
                await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                return Ok(new GenericResponse<string> { Success = true, Message = GenerateJSONWebToken(user),Error=user.Id,Data = user.Name });
                
            }
            else
            {
                return Ok(new GenericResponse<string> { Success = false, Error = "Invalid UserName or Password" });
            }
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string UserId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId);
                UserViewModel currentUser = new UserViewModel()
                {
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    LastSeen = user.LastSeen
                };
                return Ok(new GenericResponse<UserViewModel> { Success = true, Data = currentUser });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new GenericResponse<string> { Success = true});
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLastSeen([FromQuery] string UserId)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(UserId);
                user.LastSeen = DateTime.Now;
                await _userManager.UpdateAsync(user);
                return Ok(new GenericResponse<UserViewModel> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }
    }
}