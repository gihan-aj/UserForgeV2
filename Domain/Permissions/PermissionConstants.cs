using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Permissions
{
    public static class PermissionConstants
    {
        public static readonly string?[] AllPermissions = typeof(PermissionConstants)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string?)field.GetValue(null))
            .ToArray();

        //private static readonly string[] Permissions =
        //{
        //    "user.access",
        //    "user.read",
        //    "user.status",
        //    "user.edit",
        //    "user.delete"
        //};

        //public static IEnumerable<string> GetPermissions()
        //{
        //    foreach (var permission in Permissions)
        //    {
        //        yield return permission;
        //    }
        //}

        public const string UsersCreate = "users.create";         // Create a new user
        public const string UsersRead = "users.read";             // View user details
        public const string UsersEdit = "users.edit";             // Edit user details
        public const string UsersDelete = "users.delete";         // Delete a user
        public const string UsersStatusChange = "users.status";   // Activate/Deactivate user
        public const string UsersAssignRoles = "users.assign.roles"; // Assign roles to users

        public const string RolesCreate = "roles.create";         // Create a new role
        public const string RolesRead = "roles.read";             // View role details
        public const string RolesEdit = "roles.edit";             // Edit a role
        public const string RolesStatusChange = "roles.status";   // activate/deactivate role
        public const string RolesDelete = "roles.delete";         // Delete a role
        public const string RolesReadPermissions = "roles.read.permissions"; // Manage permissions for roles
        public const string RolesManagePermissions = "roles.manage.permissions"; // Manage permissions for roles

        public const string PermissionsCreate = "permissions.create"; // Create a new permission
        public const string PermissionsRead = "permissions.read";     // View permissions
        public const string PermissionsEdit = "permissions.edit";     // Edit a permission
        public const string PermissionStatusChange = "permission.status";
        public const string PermissionsDelete = "permissions.delete"; // Delete a permission

        public const string AuditLogsView = "audit.view";       // View audit logs
        public const string AuditLogsExport = "audit.export";   // Export audit logs

        public const string SettingsManage = "settings.manage"; // Manage application settings

        public const string DashboardAccess = "dashboard.access";

    }
}
