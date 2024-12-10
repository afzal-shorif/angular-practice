using System.Security.Claims;
using UserManagement.Application.Dtos;

namespace UserManagement.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> VerifyUser(UserLoginDto userLoginDto);
        ValueTask<string> GenerateTokenAsync(string userName, string tokenType);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
