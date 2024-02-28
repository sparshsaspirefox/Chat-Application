using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class FriendRequestViewModel
    {
        public int FriendShipId { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public int UnReadMessagesCount { get; set; } = 0;
        public string RequestStatus { get; set; }

        public string? FriendPhoneNumber {  get; set; }
        public string? FriendName { get; set; }

        public DateTime? LastSeen {  get; set; }
        public bool? IsOnline { get; set; }
    }
}
