using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public string? SenderId { get; set; }

        public string? ReceiverId { get; set; }
    }
}