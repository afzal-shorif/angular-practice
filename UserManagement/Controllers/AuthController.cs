using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Dtos;
using UserManagement.Application.Services.Auth;
using UserManagement.Core.Entities;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthController(SignInManager<User> signInManager, 
                            IAuthService authService,
                            IConfiguration configuration,
                            UserManager<User> userManager) 
        {
            _signInManager = signInManager;
            _authService = authService;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var response = await _authService.VerifyUser(userLoginDto);

            if (!response.Status)
            {
                return Ok(response);
            }

            response.Data = new
            {
                AccessToken = await _authService.GenerateTokenAsync(userLoginDto.UserName, "access"),
                RefreshToken = await _authService.GenerateTokenAsync(userLoginDto.UserName, "refresh"),
            };

            response.Status = true;
            response.Message = "";

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GenerateAccessToken([FromBody] RefreshTokenRequest tokenRequest)
        {
            var response = new ApiResponse<object>();

            var principal = _authService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) 
            {
                response.Status = false;
                response.Message = "Unable to access. Please login again.";
                return Ok(response);
            }           

            response.Data = new
            {
                AccessToken = await _authService.GenerateTokenAsync(user.UserName, "access"),
                RefreshToken = await _authService.GenerateTokenAsync(user.UserName, "refresh"),
            };

            response.Status = true;
            response.Message = "";

            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] object empty)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "Unable to logout";

            if (empty != null)
            {
                await _signInManager.SignOutAsync();
                response.Status = true;
                response.Message = "";

                return Ok(response);
            }

            return Unauthorized(response);
        }
    }
}
