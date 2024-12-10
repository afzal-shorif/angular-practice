using System.ComponentModel.DataAnnotations;
using UserManagement.Core.Entities;
using UserManagement.Application.Configurations.ValidationAttributes;
using System.Runtime.InteropServices;

namespace UserManagement.Application.Dtos
{
    public class UserRegistrationDto
    {
        [Required (ErrorMessage = "First name is required")]
        [MaxLength(15, ErrorMessage = "First name should be less then 15 characture")]
        [MinLength(3, ErrorMessage = "First name should be greater then 15 characture")]
        [FirstLastName (ErrorMessage = "First name should contain only Uppercase and Lowercase alphabet and space")]
        public string FirstName { get; set; } = string.Empty;

        [Required (ErrorMessage = "Last name is required")]
        [MaxLength(15, ErrorMessage = "Last name should be less then 15 characture")]
        [MinLength(3, ErrorMessage = "Last name should be greater then 15 characture")]
        [FirstLastName(ErrorMessage = "Last name should contain only uppercase and lowercase alphabet and space")]
        public string LastName { get; set; } = string.Empty;

        [Required (ErrorMessage = "Username is Required")]
        [MaxLength(15, ErrorMessage = "Username should be less then 15 characture")]
        [MinLength(3, ErrorMessage = "Username should be greater then 15 characture")]
        [Username(ErrorMessage = "Username should contain only uppercase and lowercase characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(15, ErrorMessage = "Password can not be greater then 15 characture")]
        [MinLength(8, ErrorMessage = "Password should at least 8 character long")]
        //[Password(ErrorMessage = "Password should contain alphabet, numeric and special character")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string RoleId { get; set; }
    }
}
