using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Application.Dtos;
using UserManagement.Core.Entities;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration configuration) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "Unable to login";

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.UserName, userLoginDto.Password, false, false);

            if (!result.Succeeded)
            {
                response.Message = "Username or Password did not match";
                return Ok(response);
            }

            var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _signInManager.SignOutAsync();

            if (!user.isActive)
            {
                response.Message = "User is not active yet. Please try later.";
                return Ok(response);
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName ),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserName", user.UserName),
                new Claim("isActive", user.isActive.ToString()),
            };

            if (userRoles?.Any() == true)
            {
                foreach (string role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            response.Status = true;
            response.Message = "";

            response.Data = new
            {
                Token = accessToken
            };

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
