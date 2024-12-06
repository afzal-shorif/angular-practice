using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Dtos;
using UserManagement.Core.Entities;
using UserManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly UserRoleService _userRoleService;
        // need mapper

        public UserController(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              UserService userService,
                              RoleService roleService, 
                              UserRoleService userRoleService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _roleService = roleService;
            _roleManager = roleManager;
            _userRoleService = userRoleService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegDto)
        {
            var response = new ApiResponse<object>();
            
            if(userRegDto == null)
            {
                response.Status = false;
                response.Message = "Invalid User Info";
       
                return BadRequest(response);
            }

            var role = await _roleManager.FindByIdAsync(userRegDto.RoleId);          

            if (role == null)
            {
                response.Status = false;
                response.Message = "Invalid Role Info";

                return BadRequest(response);
            }

            if(await _userManager.FindByEmailAsync(userRegDto.Email) == null)
            {
                // map the user dto object with user object
                var user = new User();
                user.Email = userRegDto.Email;
                user.FirstName = userRegDto.FirstName;
                user.LastName = userRegDto.LastName;
                user.UserName = userRegDto.UserName;
                user.isActive = false;

                var result = await _userManager.CreateAsync(user, userRegDto.Password);

                response.Message = "An error occur while register new user. Please try later.";

                if (result.Succeeded)
                {
                    var roleResponse = await _userManager.AddToRoleAsync(user, role.Name);

                    response.Status = true;
                    response.Message = "User Created Successfully";
                    response.Data = result;
                }
                else
                {
                    response.Errors = result.Errors;
                }
            }

            return Ok(response);
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var response = new ApiResponse<object>();
            response.Message = "Unable to access user list";
            ClaimsPrincipal claimsPrincipal = this.User;

            var user = await _userManager.GetUserAsync(this.User);
            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Admin"))
            {
                response.Status = true;
                response.Message = "";
                response.Data = await _userManager.Users.ToListAsync();
            }           

            return Ok(response);
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "An error occur while fetch the user info";
          
            var currentUser = await _userManager.GetUserAsync(this.User);

            if (currentUser != null)
            {
                var roles = await _userManager.GetRolesAsync(currentUser);

                response.Status = true;
                response.Message = "";
                response.Data = new
                {
                    User = currentUser,
                    Role = roles
                };
            }

            return Ok(response);
        }

        [HttpPost("update/status")]
        [Authorize]
        public async Task<IActionResult> UpdateActiveStatus([FromBody] UpdateStatusDto userStatusDto)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "An error occur while fetch the user info";

            var currentUser = await _userManager.GetUserAsync(this.User);

            if (currentUser == null)
            {
                return Ok(response);
            }

            var roles = await _userManager.GetRolesAsync(currentUser);

            if (!roles.Contains("Admin"))
            {
                response.Message = "You don't have permission to update the user status";
                return Ok(response);
            }

            if (userStatusDto == null)
            {
                response.Message = "Invalid request";
                return Ok(response);
            }

            var user = await _userManager.FindByIdAsync(userStatusDto.UserId);

            if (user == null)
            {
                response.Message = "User not found";
                return Ok(response);
            }

            if ((userStatusDto.Status == "active" && user.isActive == true) || (userStatusDto.Status == "deactive" && user.isActive == false))
            {
                response.Message = "User is already " + userStatusDto.Status;
                return Ok(response);
            }

            user.isActive = !user.isActive;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.Message = "Unable to update user status";
                return Ok(response);
            }

            response.Status = true;
            response.Message = "";
            response.Data = user;

            return Ok(response);
        }
    }
}
