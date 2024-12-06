using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync<User>();
        }

        public async Task<User> UpdateUserInfo(User user)
        {
            try
            {
                _context.Users.Attach(user);
                int changeId = await _context.SaveChangesAsync();

                if(changeId <= 0)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }     
        }
    }
}
