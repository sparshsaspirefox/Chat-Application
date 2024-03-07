using ChatHubApi.Models;
using ChatHubApi.Models.GroupsModels;
using ChatHubApi.Services;
using ChatHubApi.Services.NotificationRepo;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ChatHubApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IGenericRepository<UserGroupMatching> _userGroupRepository;
        public NotificationController(INotificationRepository notificationRepository, IGenericRepository<UserGroupMatching> userGroupRepository)
        {
            _notificationRepository = notificationRepository;
            _userGroupRepository = userGroupRepository;
        }

        [HttpPost]
        public IActionResult NewNotification(NotificationViewModel notificationViewModel)
        {
            try
            {
                Notification newNotification = new Notification()
                {
                    Status = notificationViewModel.Status,
                    SenderId = notificationViewModel.SenderId,
                    ReceiverId = notificationViewModel.ReceiverId,
                    Time = notificationViewModel.Time,
                    NotificationType = NotificationType.FriendRequest.ToString()
                };
                _notificationRepository.Insert(newNotification);
                _notificationRepository.Save();
                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications([FromQuery] string receiverId,[FromQuery] bool IsSender)
        {
            try
            {
                List<NotificationViewModel> allNotifications = await _notificationRepository.GetAllWithSender(receiverId,IsSender);
                List<NotificationViewModel> newNotifications = new List<NotificationViewModel>();
                //foreach (Notification notification in allNotifications)
                //{
                //    newNotifications.Add(new NotificationViewModel()
                //    {
                //        NotificationId = notification.NotificationId,
                //        SenderId = notification.SenderId,
                //        ReceiverId = notification.ReceiverId,
                //        Time = notification.Time,
                //        Status = notification.Status,
                //        SenderName = notification.Sender.Name,
                //        GroupId = notification.GroupId,
                //        NotificationType = notification.NotificationType
                //    }); 
                //}
                return Ok(new GenericResponse<List<NotificationViewModel>> { Data = allNotifications });
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse<List<NotificationViewModel>> { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNotification(NotificationViewModel notification)
        {
            try
            {
                
                Notification updateNotification = new Notification()
                {
                    NotificationId = notification.NotificationId,
                    SenderId = notification.SenderId,
                    ReceiverId = notification.ReceiverId,
                    Status = notification.Status,
                    Time = DateTime.Now,
                    NotificationType = notification.NotificationType,
                    GroupId = notification.GroupId
                };

                //if request is rejected then delete from db and return
                if (notification.Status == RequestType.Rejected.ToString())
                {
                    _notificationRepository.Delete(updateNotification.NotificationId);
                    _notificationRepository.Save();
                    return Ok(new GenericResponse<string> { Success = true });
                }
                _notificationRepository.Update(updateNotification);
                _notificationRepository.Save();

                //Swap the ids so another also get notification
                Notification newNotification = new Notification()
                {
                    SenderId = notification.ReceiverId,
                    ReceiverId = notification.SenderId,
                    Status = notification.Status,
                    Time = DateTime.Now,
                    NotificationType = notification.NotificationType,
                    GroupId = notification.GroupId
                };
               _notificationRepository.Insert(newNotification);
                _notificationRepository.Save();

                // add group matching if request is acccepted
                if(notification.NotificationType == NotificationType.GroupRequest.ToString())
                {
                    await AddGroupMatching(notification.ReceiverId,notification.GroupId);
                }
                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }

        private async Task AddGroupMatching(string? receiverId, int? groupId)
        {
            try
            {
                _userGroupRepository.Insert(new UserGroupMatching
                {
                    GroupId = (int)groupId,
                    UserId = receiverId,
                    IsAdmin = false
                });
                _userGroupRepository.Save();
            }
            catch { }
        }
    }
}
