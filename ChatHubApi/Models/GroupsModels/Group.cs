using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubApi.Models.GroupsModels
{
    public class Group
    {
        public int Id { get; set; }
        public string AdminId { get; set; }

        [ForeignKey("AdminId")]
        public virtual User User { get; set; }

        public string GroupName {  get; set; }  
        public string GroupDescription { get; set; }
    }
}
