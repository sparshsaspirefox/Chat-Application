using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubApi.Models.GroupsModels
{
    public class GroupMessage
    {

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public string ContentType { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public string? SenderId { get; set; }
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]

        public virtual Group Group { get; set; }
    }
}
