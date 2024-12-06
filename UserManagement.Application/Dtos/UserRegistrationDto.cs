using System.ComponentModel.DataAnnotations;
using UserManagement.Core.Entities;

namespace UserManagement.Application.Dtos
{
    public class UserRegistrationDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; }
    }
}
