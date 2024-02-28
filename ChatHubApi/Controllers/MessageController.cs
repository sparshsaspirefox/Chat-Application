using ChatHubApi.Models;
using ChatHubApi.Services;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatHubApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
            
        }

        [HttpPost]
        public IActionResult NewMessage(MessageViewModel messageViewModel)
        {
            Message newMessage = new Message()
            {
                Content = messageViewModel.Content,
                SenderId = messageViewModel.SenderId,
                ReceiverId = messageViewModel.ReceiverId,
                Time = messageViewModel.Time
            };
            try
            {
                _messageRepository.Insert(newMessage);
                _messageRepository.Save();
                return Ok(new GenericResponse<string> { Success = true }); ;
            }
            catch(Exception ex) 
            {
                return BadRequest(new GenericResponse<string> { Success = false,Message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllMessages([FromQuery] string senderId, [FromQuery] string receiverId)
        {
            try
            {
                List<Message> allMessages = _messageRepository.GetAll().Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                                m.SenderId == receiverId && m.ReceiverId == senderId).ToList();
                List<MessageViewModel> messages = new List<MessageViewModel>();
                foreach (Message message in allMessages)
                {
                    messages.Add(new MessageViewModel()
                    {
                        MessageId = message.MessageId,
                        SenderId = message.SenderId,
                        ReceiverId = message.ReceiverId,
                        Time = message.Time,
                        Content = message.Content,

                    });
                }
                return Ok(new GenericResponse<List<MessageViewModel>> { Data = messages});
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse<List<MessageViewModel>> { Success = false, Message = ex.Message });
            }
           
        }

        [HttpGet]
        public IActionResult GetMessagesByFilter([FromQuery] string senderId, [FromQuery] string receiverId, [FromQuery]int pageNo)
        {
            try
            {
                List<MessageViewModel> allMessages = _messageRepository.GetMessagesByFilter(senderId,receiverId,pageNo).ToList();
                return Ok(new GenericResponse<List<MessageViewModel>> { Data = allMessages });
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse<List<MessageViewModel>> { Success = false, Message = ex.Message });
            }

        }
    }
}
