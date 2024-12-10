using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.Configurations.ValidationAttributes;
using UserManagement.Application.Dtos;
using UserManagement.Core.Entities;
using UserEntity = UserManagement.Core.Entities;

namespace UserManagement.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserEntity.User> _signInManager;
        private readonly UserManager<UserEntity.User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(SignInManager<UserEntity.User> signInManager,
                            UserManager<UserEntity.User> userManager,
                            IConfiguration configuration) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<object>> VerifyUser(UserLoginDto userLoginDto)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "Unable to login";

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.UserName, userLoginDto.Password, false, false);

            if (!result.Succeeded)
            {
                response.Message = "Username or Password did not match";
                return response;
            }

            var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _signInManager.SignOutAsync();

            if (!user.isActive)
            {
                response.Message = "User is not active yet. Please try later.";
                return response;
            }

            response.Status = true;
            response.Message = "";

            return response;
        }

        public async ValueTask<string> GenerateTokenAsync(string userName, string tokenType)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(tokenType == "refresh")
            {
                var refreshToken = GenerateRefreshToken();
                var RefreshTokenValidityDays =  _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays");

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(RefreshTokenValidityDays);

                await _userManager.UpdateAsync(user);
                return refreshToken;
            }          

            var userRoles = await _userManager.GetRolesAsync(user);

            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];

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
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _configuration["JwtConfig:Issuer"],
                ValidAudience = _configuration["JwtConfig:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if(jwtSecurityToken == null || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                // 
            }

            return principal;
        }
    }
}
