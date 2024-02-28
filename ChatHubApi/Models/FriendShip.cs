using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubApi.Models
{
    public class FriendShip
    {
        public int FriendShipId {  get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public string FriendId { get; set; }

        [ForeignKey("FriendId")]
        public User Friend { get; set; }
        public int UnReadMessagesCount { get; set; } = 0;

        
    }
}
