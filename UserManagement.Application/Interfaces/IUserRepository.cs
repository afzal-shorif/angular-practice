using UserManagement.Core.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IList<User>> GetUsersAsync();

        Task<User> UpdateUserInfo(User user);
    }
}
