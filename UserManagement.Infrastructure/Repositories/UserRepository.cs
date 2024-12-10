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

        public async Task<User> UpdateUserInfoAsync(User user)
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

        public async Task<UserPhoto> GetUserPhotoAsync(User user)
        {
            try
            {
                var result = await _context.UserPhotos.FirstOrDefaultAsync(u => u.UserId == user.Id);

                return result;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public async Task<UserPhoto> AddUserPhotoAsync(UserPhoto userPhoto)
        {
            try
            {
                var result = await _context.UserPhotos.AddAsync(userPhoto);
                int changeId = _context.SaveChanges();

                if (changeId > 0)
                {
                    return result.Entity;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }        
        }

        public async Task<UserPhoto> UpdateUserPhotoAsync(UserPhoto userPhoto)
        {
            try
            {            
                _context.UserPhotos.Attach(userPhoto);
                int changeId = await _context.SaveChangesAsync();

                if (changeId < 0)
                {
                    return null;
                }

                return userPhoto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
    }
}
