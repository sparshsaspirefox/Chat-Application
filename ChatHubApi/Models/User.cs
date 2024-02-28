using Microsoft.AspNetCore.Identity;

namespace ChatHubApi.Models
{
    public class User : IdentityUser
    {
       
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime LastSeen { get; set; } = DateTime.Now;
    }
}
