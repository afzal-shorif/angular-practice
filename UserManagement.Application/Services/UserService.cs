using System.Security.Claims;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        //public async Task GenerateTokens(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim("UserId", user.Id),
        //        new Claim("FirstName", user.FirstName),
        //        new Claim("LastName", user.FirstName),
        //        new Claim(ClaimTypes.Email, user.Email),             
        //        new Claim("UserName", user.UserName),
        //        new Claim("isActive", user.isActive.ToString()),
        //    };

        //    var jwtResultToCallSubscriptionApi = jwtAuthManager.GenerateTokens(user, claims, DateTime.Now);

        //    var userAllowedActions = await subscriptionServices.RetrieveCurrentUserActiveSubscription(jwtResultToCallSubscriptionApi.AccessToken);

        //    if (!userAllowedActions.IsEmpty())
        //    {
        //        claims.Add(new Claim(Common.IdentityConstants.ScopeClaim, string.Join(" ", userAllowedActions)));
        //    }

        //    return claims;
        //}
    }
}
