using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Configurations
{
    public class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=(local);Initial Catalog=UserManagement;User Id=sa;Password=123456;Integrated Security=false;MultipleActiveResultSets=true;TrustServerCertificate=True;"));
            //services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
