using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Permissions
{
    public static class SsoPermissionConstants
    {
        public static readonly string?[] AllPermissions = typeof(SsoPermissionConstants)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string?)field.GetValue(null))
            .ToArray();

        public const string HomeAccess = "home.access";

        public const string DashboardAccess = "dashboard.access";

        public const string UsersAccess = "users.access";               // View users
        public const string UsersCreate = "users.create";               // Create a new user
        public const string UsersEdit = "users.edit";                   // Edit user details
        public const string UsersDelete = "users.delete";               // Delete a user
        public const string UsersStatusChange = "users.status";         // Activate/Deactivate user
        public const string UsersRolesAccess = "users.roles.access";        // View user roles
        public const string UsersRolesManage = "users.roles.manage";    // Assign roles to users

        public const string RolesAccess = "roles.access";                           // View roles
        public const string RolesCreate = "roles.create";                           // Create a new role
        public const string RolesEdit = "roles.edit";                               // Edit a role
        public const string RolesStatusChange = "roles.status";                     // activate/deactivate role
        public const string RolesDelete = "roles.delete";                           // Delete a role
        public const string RolesPermissionsAccess = "roles.permissions.access";        // Manage permissions for roles
        public const string RolesPermissionsManage = "roles.permissions.manage";    // Manage permissions for roles

        public const string AppsAccess = "apps.access";         // Access to apps management
        public const string AppsCreate = "apps.create";         // Create a new app
        public const string AppsEdit = "apps.edit";             // Edit app details
        public const string AppsStatusChange = "apps.status";   // Activate, deactivate apps
        public const string AppsDelete = "apps.delete";         // Delete apps

        public const string PermissionsAccess = "permissions.access";       // Access permissions

        public const string AppPortalAccess = "app-portal.access";

        public const string AuditLogsAccess = "audit.access";   // View audit logs
        public const string AuditLogsExport = "audit.export";   // Export audit logs

        public const string SettingsAccess = "settings.access"; // View application settings
        public const string SettingsManage = "settings.manage"; // Manage application settings

    }
}
