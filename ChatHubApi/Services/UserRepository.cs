using ChatHubApi.Context;
using ChatHubApi.Models;

namespace ChatHubApi.Services
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
