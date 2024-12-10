using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserManagement.Application.Dtos;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using Entity = UserManagement.Core.Entities;

namespace UserManagement.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<Entity.User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<Entity.User> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<object>> RegisterUser(UserRegistrationDto userRegDto, string userRole)
        {
            var response = new ApiResponse<object>();

            if (await _userManager.FindByEmailAsync(userRegDto.Email) != null)
            { 
                response.Message = "User is already exist.";
                return response;
            }

            // map the user dto object with user object
            var user = new Entity.User();
            user.Email = userRegDto.Email;
            user.FirstName = userRegDto.FirstName;
            user.LastName = userRegDto.LastName;
            user.UserName = userRegDto.UserName;
            user.isActive = false;

            var result = await _userManager.CreateAsync(user, userRegDto.Password);

            response.Message = "An error occur while register new user. Please try later.";

            if (result.Succeeded)
            {
                var roleResponse = await _userManager.AddToRoleAsync(user, userRole);

                response.Status = true;
                response.Message = "User Created Successfully";
                response.Data = result;
            }
            else
            {
                response.Errors = result.Errors;
            }
            
            return response;
        }

        public async Task<ApiResponse<object>> GetUsers(ClaimsPrincipal claimsPrincipal)
        {
            var response = new ApiResponse<object>();
            response.Message = "Unable to access user list";

            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Admin"))
            {
                response.Status = true;
                response.Message = "";
                response.Data = await _userManager.Users.ToListAsync();
            }

            return response;
        }

        public async Task<ApiResponse<object>> GetCurrentUserInfo(ClaimsPrincipal claimsPrincipal)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "An error occur while fetch the user info";

            var currentUser = await _userManager.GetUserAsync(claimsPrincipal);

            if (currentUser != null)
            {
                var roles = await _userManager.GetRolesAsync(currentUser);
                currentUser.Photo = await _userRepository.GetUserPhotoAsync(currentUser);

                response.Status = true;
                response.Message = "";
                response.Data = new
                {
                    User = currentUser,
                    Role = roles
                };
            }

            return response;
        }

        public async Task<ApiResponse<object>> UpdateUserInfoAsync(Entity.User user, UpdateUserInfoDto userInfoDto)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "Unable to upload profile picture";

            // if username and email is unique
            user.FirstName = userInfoDto.FirstName;
            user.LastName = userInfoDto.LastName;
            user.UserName = userInfoDto.UserName;
            user.Email = userInfoDto.Email;

            var userResult = await _userManager.UpdateAsync(user);

            if (!userResult.Succeeded)
            {
                response.Status = false;
                response.Errors = userResult.Errors;
                return response;
            }


            UserPhoto photoResult = null;

            var previousPhoto = await _userRepository.GetUserPhotoAsync(user);

            if(previousPhoto != null)
            {
                previousPhoto.Base64String = userInfoDto.Photo;
                photoResult = await _userRepository.UpdateUserPhotoAsync(previousPhoto);

            }
            else
            {
                var userPhoto = new UserPhoto();
                userPhoto.UserId = user.Id;
                userPhoto.Base64String = userInfoDto.Photo;

                photoResult = await _userRepository.AddUserPhotoAsync(userPhoto);
            }   
       
            if (photoResult != null) 
            {
                response.Status = true;
                response.Message = "";
                response.Data = new
                {
                    User = user,
                };
            }

            return response;
        }
    }
}
