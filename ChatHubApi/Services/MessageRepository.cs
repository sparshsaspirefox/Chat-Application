using ChatHubApi.Context;
using ChatHubApi.Models;
using Data.Models;

namespace ChatHubApi.Services
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<MessageViewModel> GetMessagesByFilter(string senderId, string receiverId, int pageNo)
        {
            int NoOfMessagesPerPage = 20;
            List<Message> filteredMessages = _context.Messages.OrderByDescending(m => m.Time).Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                                m.SenderId == receiverId && m.ReceiverId == senderId).Skip((pageNo - 1) * NoOfMessagesPerPage).Take(NoOfMessagesPerPage).ToList();

            List<MessageViewModel> messages = new List<MessageViewModel>();
            foreach (Message message in filteredMessages)
            {
                messages.Add(new MessageViewModel()
                {
                    MessageId = message.MessageId,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    Time = message.Time,
                    Content = message.Content,
                    ContentType = message.ContentType
                });
            }
            return messages;
        }
    }
}
