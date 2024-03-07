using ChatHubApi.Models;
using ChatHubApi.Models.GroupsModels;
using ChatHubApi.Services;
using ChatHubApi.Services.GroupRepo;
using ChatHubApi.Services.NotificationRepo;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatHubApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly INotificationRepository _notificationRepository;

        private readonly IGenericRepository<UserGroupMatching> _userGroupRepository;
        private readonly IGenericRepository<GroupMessage> _groupMessageRepository;

        public GroupController(IGroupRepository groupRepository,INotificationRepository notificationRepository,IGenericRepository<UserGroupMatching> userGroupRepository,
            IGenericRepository<GroupMessage> groupMessageRepository)
        {
            _groupRepository = groupRepository;
            _notificationRepository = notificationRepository;
            _userGroupRepository = userGroupRepository;
            _groupMessageRepository = groupMessageRepository;
        }
        [HttpPost]
        public async Task<IActionResult> NewGroup(GroupViewModel groupModel)
        {
            Group newGroup = new Group()
            {
                GroupName = groupModel.Name,
                GroupDescription = groupModel.Description,
                AdminId = groupModel.AdminId,
            };
            try
            {
                _groupRepository.Insert(newGroup);
                _groupRepository.Save();

                _userGroupRepository.Insert(new UserGroupMatching
                {
                    GroupId = newGroup.Id,
                    UserId = newGroup.AdminId,
                    IsAdmin = true
                });
                _userGroupRepository.Save();
                foreach (var groupUser in groupModel.GroupUsersIds)
                {
                    _notificationRepository.Insert(
                        new Notification()
                        {
                            Time = DateTime.Now,
                            Status = RequestType.Pending.ToString(),
                            SenderId = groupModel.AdminId,
                            GroupId = newGroup.Id,
                            ReceiverId = groupUser,
                            NotificationType = NotificationType.GroupRequest.ToString(),
                        });
                }
                _notificationRepository.Save();

                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false,Error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetGroupsByUserId([FromQuery]string userId)
        {
            try
            {
                List<GroupViewModel> groups =  _groupRepository.GetAllGroupsById(userId);

                return Ok(new GenericResponse<List<GroupViewModel>> { Success = true, Data = groups });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<List<GroupViewModel>> { Success = false, Error = ex.Message });

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupById([FromQuery]int GroupId)
        {
            try
            {
                Group group =  _groupRepository.GetById(GroupId);
                GroupViewModel groupViewModel = new GroupViewModel()
                {
                    Description = group.GroupDescription,
                    AdminId = group.AdminId,
                    Name = group.GroupName,
                    Id = group.Id,
                    GroupUsersIds = _groupRepository.GetGroupMembersId(GroupId)
                    
                };
                return Ok(new GenericResponse<GroupViewModel> { Success = true,Data = groupViewModel });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<GroupViewModel> { Success = false, Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> NewGroupMessage(GroupMessageViewModel groupMessageViewModel)
        {
            try
            {
                GroupMessage groupMessage = new GroupMessage()
                {
                    SenderId = groupMessageViewModel.SenderId,
                    Content = groupMessageViewModel.Content,
                    ContentType = groupMessageViewModel.ContentType,
                    GroupId = groupMessageViewModel.GroupId,
                    Time = groupMessageViewModel.Time
                };
                _groupMessageRepository.Insert(groupMessage);
                _groupMessageRepository.Save();
                return Ok(new GenericResponse<string> { Success = true });
            }
            catch(Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false,Error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllGroupMessagesById([FromQuery] int GroupId)
        {
            try
            {
                List<GroupMessageViewModel> allMessages = _groupRepository.GetAllMessagesByGroupId(GroupId);
                return Ok(new GenericResponse<List<GroupMessageViewModel>> { Success = true , Data = allMessages});
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<List<GroupMessageViewModel>> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupMembersDetails([FromQuery] int GroupId)
        {
            try
            {
                List<UserViewModel> allMembers = _groupRepository.GetGroupMembersDetails(GroupId);
                return Ok(new GenericResponse<List<UserViewModel>> { Success = true, Data = allMembers });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<List<UserViewModel>> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromGroup([FromQuery] int GroupId, [FromQuery]string UserId)
        {
            try
            {
                
                UserGroupMatching userGroupMatching = _groupRepository.GetUserGroupMatching(GroupId,UserId);
                _userGroupRepository.Delete(userGroupMatching.Id);
                _userGroupRepository.Save();
                return Ok(new GenericResponse<List<UserViewModel>> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<List<UserViewModel>> { Success = false, Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUnreadMessageCount(int GroupId, string SenderId, bool IsClearCount)
        {
            try
            {
                _groupRepository.UpdateUnreadMessageCount(GroupId, SenderId, IsClearCount);
                return Ok(new GenericResponse<string> { Success = true });
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<string> { Success = false, Error = ex.Message });
            }
        }
       
    }
}
