using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async ValueTask<bool> AddUserRoleAsync(UserRole userRole)
        //{
        //    try
        //    {
        //        await _dbContext.AddAsync(userRole);
        //        int changeId = await _dbContext.SaveChangesAsync();

        //        return changeId > 0;
        //    }
        //    catch (Exception ex) { 
        //        Console.WriteLine(ex);
        //        return false;
        //    }
        //}
    }
}
