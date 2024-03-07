using ChatHubApi.Context;
using ChatHubApi.Models.GroupsModels;
using Data.Models;

namespace ChatHubApi.Services.GroupRepo
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext _context) : base(_context)
        {
            this._context = _context;
        }

        public List<GroupViewModel> GetAllGroupsById(string UserId)
        {
            IQueryable<GroupViewModel> groups = _context.UserGroupMatchings.Where(m => m.UserId == UserId).Select(
                m => new GroupViewModel
                {
                    Id = m.GroupId,
                    Name = m.Group.GroupName,
                    Description = m.Group.GroupDescription,
                    AdminId = m.Group.AdminId,
                    UnreadMessageCount = m.UnReadMessages
                });
            return groups.ToList();
        }

        public List<GroupMessageViewModel> GetAllMessagesByGroupId(int GroupId)
        {
            IQueryable<GroupMessageViewModel> allMessages = _context.GroupMessages.Where(m => m.GroupId == GroupId).Select(
                m => new GroupMessageViewModel
                {
                    Id = m.Id,
                    GroupId = m.GroupId,
                    Content = m.Content,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.Name,
                    ContentType = m.ContentType,
                    Time = m.Time,
                });
            return allMessages.ToList();
        }

        public List<string> GetGroupMembersId(int GroupId)
        {
            IQueryable<string> allUsersId = _context.UserGroupMatchings.Where(m => m.GroupId == GroupId).Select(
                m => m.UserId);
            return allUsersId.ToList();
        }

        public List<UserViewModel> GetGroupMembersDetails(int GroupId)
        {
            IQueryable<UserViewModel> allUsersDetails = _context.UserGroupMatchings.Where(m => m.GroupId == GroupId).Select(
                 m => new UserViewModel
                 {
                     id = m.UserId,
                     Name = m.User.Name,
                     PhoneNumber = m.User.PhoneNumber,
                     LastSeen = m.User.LastSeen,
                     IsAdmin = m.IsAdmin
                 });
            return allUsersDetails.ToList();
        }

        public UserGroupMatching GetUserGroupMatching(int GroupId, string UserId)
        {
           return _context.UserGroupMatchings.Where(m => m.UserId == UserId && m.GroupId == GroupId).FirstOrDefault();
           
        }

        public void UpdateUnreadMessageCount(int groupId, string senderId, bool IsClearCount)
        {
            if (IsClearCount)
            {
                UserGroupMatching userGroupMatching = _context.UserGroupMatchings.Where(m => m.GroupId == groupId && m.UserId == senderId).FirstOrDefault();
                if (userGroupMatching != null)
                {
                    userGroupMatching.UnReadMessages = 0;
                    _context.UserGroupMatchings.Update(userGroupMatching);
                    _context.SaveChanges();
                }
            }
            else
            {
                //find out all the entry to update message count except the sender
                IQueryable<UserGroupMatching> userGroupMatchings = _context.UserGroupMatchings.Where(m => m.GroupId == groupId && m.UserId != senderId);
                foreach (UserGroupMatching item in userGroupMatchings)
                {
                    item.UnReadMessages++;
                    _context.UserGroupMatchings.Update(item);
                }
                _context.SaveChanges();
            }
            
        }
    }
}
