using Microsoft.AspNetCore.Identity;

namespace ChatHubApi.Models
{
    public class User : IdentityUser
    {
       
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string? About {  get; set; }
        public string? ImageUrl {  get; set; }
        public DateTime LastSeen { get; set; } = DateTime.Now;
    }
}
