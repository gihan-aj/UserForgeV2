using SharedKernal;

namespace Domain.Roles
{
    public static class RoleErrors
    {
        public static class NotFound
        {
            public static Error Role(string roleId) => new("RoleNotFound", $"No role found with id: {roleId}.");
            public static Error Roles => new("RolesNotFound", "No roles find with given ids.");
            public static Error User(string userId) => new("UserRolesNotFound", $"No roles found for user id: {userId}");
        }
        public static class Conflict
        {
            public static Error RoleNameAlreadyExists(string roleName) => new("RoleNameAlreadyExists", $"A role with name: {roleName} already exists.");
        }
        public static class Permissions
        {
            public static Error NoPermissionsFound(string roleName) => new("NoPermissionsFound", $"No permissions found on role, {roleName}");
        }
    }
}
