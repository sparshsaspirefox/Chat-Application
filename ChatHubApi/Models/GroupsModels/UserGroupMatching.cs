using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubApi.Models.GroupsModels
{
    public class UserGroupMatching
    {
        public int Id { get; set; } 
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]

        public virtual Group Group { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public bool IsAdmin { get; set; }

        public int UnReadMessages { get; set; } = 0;
        
    }
}
