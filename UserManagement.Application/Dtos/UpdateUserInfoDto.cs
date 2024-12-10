using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Dtos
{
    public class UpdateUserInfoDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName {  get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Profile Picture is Required")]
        public string Photo { get; set; } = string.Empty;
    }
}
