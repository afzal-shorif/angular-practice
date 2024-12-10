using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IList<User>> GetUsersAsync();

        Task<User> UpdateUserInfoAsync(User user);
        Task<UserPhoto> GetUserPhotoAsync(User user);
        Task<UserPhoto> AddUserPhotoAsync(UserPhoto userPhoto);
        Task<UserPhoto> UpdateUserPhotoAsync(UserPhoto userPhoto);
    }
}
