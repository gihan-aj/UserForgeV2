using Domain.Permissions;
using Domain.Roles;

namespace Domain.RolePermissions
{
    public class RolePermission
    {
        public string? RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public string? PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
