using ChatHubApi.Models.GroupsModels;
using Data.Models;

namespace ChatHubApi.Services.GroupRepo
{
    public interface IGroupRepository:IGenericRepository<Group>
    {
        List<GroupViewModel> GetAllGroupsById(string UserId);

        List<GroupMessageViewModel> GetAllMessagesByGroupId(int GroupId);

        List<string> GetGroupMembersId(int GroupId);

        List<UserViewModel> GetGroupMembersDetails(int GroupId);

        UserGroupMatching GetUserGroupMatching(int GroupId, string UserId);

        void UpdateUnreadMessageCount(int groupId, string senderId, bool IsClearCount);

    }
}
