using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Group
{
    public interface IGroupService
    {
        Task<GenericResponse<string>> NewGroup(GroupViewModel groupModel);
        Task<GenericResponse<List<GroupViewModel>>> GetGroupsByUserId(string userId);
        Task<GenericResponse<GroupViewModel>> GetGroupById(int groupId);

        Task<GenericResponse<string>> NewGroupMessage(GroupMessageViewModel groupMessageModel);
        Task<GenericResponse<List<GroupMessageViewModel>>> GetAllGroupMessagesById(int groupId);
        Task<GenericResponse<List<UserViewModel>>> GetGroupMembersDetails(int groupId);

        Task<GenericResponse<string>> RemoveUserFromGroup(int groupId,string userId);

        Task<GenericResponse<string>> UpdateUnreadMessageCount(int groupId, string senderId, bool IsClearCount);
    }
}
