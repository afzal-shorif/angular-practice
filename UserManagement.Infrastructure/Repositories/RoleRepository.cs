using UserManagement.Infrastructure.Data;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace UserManagement.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<IList<Role>> GetRolesAsync()
        //{
        //    try
        //    {
        //        return await _dbContext.Roles.ToListAsync() ?? new List<Role>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return new List<Role>();
        //    }
            
        //}

        //public async Task<Role?> GetRoleAsync(string roleId)
        //{
        //    try
        //    {
        //        var result = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return null;
        //    }

        //}
    }
}
