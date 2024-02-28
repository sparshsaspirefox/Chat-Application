using ChatHubApi.Models;
using Data.Models;

namespace ChatHubApi.Services
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        List<MessageViewModel> GetMessagesByFilter(string senderId,string receiverId,int pageNo);
    }
}
