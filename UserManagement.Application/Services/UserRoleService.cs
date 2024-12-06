using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;

namespace UserManagement.Application.Services
{
    public class UserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository) 
        {
            _userRoleRepository = userRoleRepository;
        }


        //public async ValueTask<bool> AddUserRole(User user, Role role)
        //{
        //    var userRole = new UserRole();
        //    userRole.User = user;
        //    userRole.Role = role;

        //    var result = await _userRoleRepository.AddUserRoleAsync(userRole);
            
        //    return result;
        //}
    }
}
