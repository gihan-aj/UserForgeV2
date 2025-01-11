using SharedKernal;

namespace Domain.Permissions
{
    public static class PermissionErrors
    {
        public static class NotFound
        {
            public static Error PermissionNotFound(string permissionName) => new("PermissionNotFound", $"Permission with name '{permissionName}' not found.");
            public static Error PermissionsNotFound => new("PermissionsNotFound", "Permissions with gievn ids not found.");
        }

        public static class  Confilct
        {
            public static Error PermissionNameAlreadyExists(string permissionName) => new("PermissionNameAlreadyExists", $"Permission with name '{permissionName}' already exists.");
        }

        public static class General
        {
            public static Error OperationFailed(string reason) => new("OperationFailed", $"The operation failed. Reason: {reason}.");
            public static Error UnexpectedError => new("UnexpectedError", "An unexpected error occurred. Please try again later.");
        }
    }
}
