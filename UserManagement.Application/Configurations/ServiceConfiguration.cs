using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Services.Auth;
using UserManagement.Application.Services.User;

namespace UserManagement.Application.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            //Infrastructure.ServiceConfiguration.ConfigureServices(services);
        }
    }
}
