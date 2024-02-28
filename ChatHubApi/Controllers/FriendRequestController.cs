using ChatHubApi.Models;
using ChatHubApi.Services.FriendRequest;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatHubApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        public FriendRequestController(IFriendRequestRepository friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        [HttpPost]
        public IActionResult NewFriendRequest(FriendRequestViewModel friendRequestViewModel)
        {
            try
            {
                FriendShip newRequest = new FriendShip()
                {
                    UserId = friendRequestViewModel.UserId,
                    FriendId = friendRequestViewModel.FriendId,
                    UnReadMessagesCount = friendRequestViewModel.UnReadMessagesCount,
                };
                
                _friendRequestRepository.Insert(newRequest);
                _friendRequestRepository.Save();

                FriendShip newRequest1 = new FriendShip()
                {
                    UserId = friendRequestViewModel.FriendId,
                    FriendId = friendRequestViewModel.UserId,
                    UnReadMessagesCount = friendRequestViewModel.UnReadMessagesCount,
                };

                _friendRequestRepository.Insert(newRequest1);
                _friendRequestRepository.Save();


                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMessageCount([FromQuery]string SenderId, [FromQuery] string ReceiverId, [FromQuery] bool IsIncrease)
        {
            try
            {
                //swaps the ids so it is shown by receiver
                FriendShip friendShip = await _friendRequestRepository.UpdateMessageCount(ReceiverId, SenderId, IsIncrease);
                _friendRequestRepository.Update(friendShip);
                _friendRequestRepository.Save();

                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends([FromQuery] string userId)
        {
            try
            {
                string usersId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<FriendShip> allFriends = await _friendRequestRepository.GetAllFriends(userId);
                List<FriendRequestViewModel> friends = new List<FriendRequestViewModel>();
                foreach (FriendShip friendship in allFriends)
                {
                    friends.Add(new FriendRequestViewModel()
                    {
                        FriendShipId = friendship.FriendShipId,
                        UserId = friendship.UserId,
                        FriendId = friendship.FriendId,
                        UnReadMessagesCount = friendship.UnReadMessagesCount,
                        FriendName = friendship.Friend.Name,
                        FriendPhoneNumber = friendship.Friend.PhoneNumber,
                        LastSeen = friendship.Friend.LastSeen
                    }) ;

                }
                return Ok(new GenericResponse<List<FriendRequestViewModel>>() { Data = friends });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<List<FriendRequestViewModel>>() { Success = false,Error=ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnReadMessageCount([FromQuery] string SenderId, [FromQuery] string ReceiverId)
        {
            try
            {
                //swaps the ids so it is shown by receiver
                int messagesCount = await _friendRequestRepository.GetUnReadMessageCount(ReceiverId, SenderId);
               

                return Ok(new GenericResponse<string> { Success = true ,Data = messagesCount.ToString() });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }


    }
}
