using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdminId {  get; set; }

        public int? UnreadMessageCount { get; set; }
        public List<string> GroupUsersIds {  get; set; }
    }
}
