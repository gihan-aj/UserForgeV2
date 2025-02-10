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

        public const string HomeAccess = "home.access";

        public const string DashboardAccess = "dashboard.access";

        public const string UsersAccess = "users.access";               // Acccess to user management
        public const string UsersCreate = "users.create";               // Create a new user
        public const string UsersRead = "users.read";                   // View user details
        public const string UsersEdit = "users.edit";                   // Edit user details
        public const string UsersDelete = "users.delete";               // Delete a user
        public const string UsersStatusChange = "users.status";         // Activate/Deactivate user
        public const string UsersReadRoles = "users.read.roles";        // View user roles
        public const string UsersAssignRoles = "users.manage.roles";    // Assign roles to users

        public const string RolesAccess = "roles.access";                           // Access to role management
        public const string RolesCreate = "roles.create";                           // Create a new role
        public const string RolesRead = "roles.read";                               // View role details
        public const string RolesEdit = "roles.edit";                               // Edit a role
        public const string RolesStatusChange = "roles.status";                     // activate/deactivate role
        public const string RolesDelete = "roles.delete";                           // Delete a role
        public const string RolesReadPermissions = "roles.read.permissions";        // Manage permissions for roles
        public const string RolesManagePermissions = "roles.manage.permissions";    // Manage permissions for roles

        public const string AppsAccess = "apps.access";         // Access to apps management
        public const string AppsCreate = "apps.create";         // Create a new app
        public const string AppsRead = "apps.read";             // View app details
        public const string AppsEdit = "apps.edit";             // Edit app details
        public const string AppsStatusChange = "apps.status";   // Activate, deactivate apps
        public const string AppsDelete = "apps.delete";         // Delete apps

        public const string PermissionsAccess = "permissions.access";       // Access permissions
        public const string PermissionsRead = "permissions.read";           // View permissions
        public const string PermissionsEdit = "permissions.edit";           // Edit a permission
        public const string PermissionsStatusChange = "permissions.status"; // Activate/deactivate a permission

        public const string AppPortalAccess = "app-portal.access";

        public const string AuditLogsAccess = "audit.access";   // View audit logs
        public const string AuditLogsExport = "audit.export";   // Export audit logs

        public const string SettingsManage = "settings.manage"; // Manage application settings

    }
}
