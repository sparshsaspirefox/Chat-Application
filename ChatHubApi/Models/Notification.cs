using ChatHubApi.Models.GroupsModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubApi.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public string? SenderId { get; set; }


        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }

        public string? ReceiverId { get; set; }

        public string NotificationType {  get; set; }

        public int? GroupId {  get; set; }
        [ForeignKey("GroupId")]
        public virtual Group? Group { get; set; }

        public bool IsSeen {  get; set; }
    }
}
