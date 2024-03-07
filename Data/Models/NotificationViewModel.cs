using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }

        public DateTime Time { get; set; }
        public string Status { get; set; }

        public string? SenderName { get; set; }
        public string? SenderId { get; set; }

        public string?  ReceiverId { get; set; }

        public string? GroupName {  get; set; }
        public int? GroupId { get; set; }
        public string? NotificationType { get; set; }
    }
}
