using UserManagement.Core.Entities;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) 
        { 
            _roleRepository = roleRepository;
        }

        //public async Task<IList<Role>> GetRolesAsync()
        //{
        //    return await _roleRepository.GetRolesAsync();
        //}

        //public async Task<Role> GetRoleAsync(string roleId)
        //{
        //    var result = await _roleRepository.GetRoleAsync(roleId);
        //    return result;
        //}
    }
}
