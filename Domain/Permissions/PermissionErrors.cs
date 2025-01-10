using SharedKernal;

namespace Domain.Permissions
{
    public static class PermissionErrors
    {
        public static class NotFound
        {
            public static Error PermissionNotFound(string permissionName) => new("PermissionNotFound", $"Permission with name '{permissionName}' not found.");
        }

        public static class  Confilct
        {
            public static Error PermissionNameAlreadyExists(string permissionName) => new("PermissionNameAlreadyExists", $"Permission with name '{permissionName}' already exists.");
        }
    }
}
