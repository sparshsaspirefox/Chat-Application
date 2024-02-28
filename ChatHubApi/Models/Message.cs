using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubApi.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public string? SenderId {  get; set; }
        

        [ForeignKey("ReceiverId")]
        
        public virtual User Receiver { get; set; }

        public string? ReceiverId { get; set; }

    }
}
