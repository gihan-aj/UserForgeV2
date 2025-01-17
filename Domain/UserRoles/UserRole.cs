using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Domain.UserRoles
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
