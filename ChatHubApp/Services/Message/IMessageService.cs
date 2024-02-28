using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.Message
{
    public interface IMessageService
    {

        Task<GenericResponse<string>> NewMessage(MessageViewModel message);
        Task<GenericResponse<List<MessageViewModel>>> GetMessages(string senderId, string receiverId);

        Task<GenericResponse<List<MessageViewModel>>> GetMessagesByFilter(string senderId, string receiverId,int pageNo);
    }
}
