using Microsoft.AspNetCore.Identity;

namespace UserManagement.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool isActive { get; set; } = false;

        //public UserRole UserRoles { get; set; }
    }
}
