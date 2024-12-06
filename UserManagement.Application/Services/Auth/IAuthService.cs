using UserManagement.Application.Dtos;

namespace UserManagement.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> VerifyUser(UserLoginDto userLoginDto);
        Task<object> GetAccessToken(string userName);
    }
}
