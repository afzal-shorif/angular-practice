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

        public AuthController(SignInManager<User> signInManager, 
                            IAuthService authService) 
        {
            _signInManager = signInManager;
            _authService = authService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var response = await _authService.VerifyUser(userLoginDto);

            if (!response.Status)
            {
                return Ok(response);

            }

            response.Data = await _authService.GetAccessToken(userLoginDto.UserName);

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
