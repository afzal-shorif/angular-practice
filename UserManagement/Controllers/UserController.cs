using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Dtos;
using UserManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Application.Services.User;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        // need mapper

        public UserController(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IUserService userService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _roleManager = roleManager;
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

            response = await _userService.RegisterUser(userRegDto, role.Name);

            return Ok(response);
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {        
            var response = await _userService.GetUsers(this.User);

            return Ok(response);
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            //return BadRequest();
            //return Unauthorized();
            
            var response = await _userService.GetCurrentUserInfo(this.User);

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

        [HttpPost("update/info")]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoDto userInfoDto)
        {
            var response = new ApiResponse<object>();
            response.Status = false;
            response.Message = "An error occur while fetch the user info";

            var currentUser = await _userManager.GetUserAsync(this.User);

            if (currentUser == null)
            {
                return Ok(response);
            }

            if (userInfoDto == null)
            {
                response.Message = "Invalid request";
                return Ok(response);
            }

            response  = await _userService.UpdateUserInfoAsync(currentUser, userInfoDto);

            return Ok(response);
        }
    }
}
