using System.Collections.Generic;

namespace Domain.Permissions
{
    public static class Permissions
    {
        public static readonly List<Permission> AllPermissions = new List<Permission> 
        {
            Permission.Create(PermissionConstants.HomeAccess, "Access to the home page", 1),
            Permission.Create(PermissionConstants.DashboardAccess, "Access to the dashboard", 2),

            // User Management Permissions
            Permission.Create(PermissionConstants.UsersAccess, "Access to user management", 3),
            Permission.Create(PermissionConstants.UsersCreate, "Create a new user", 4),
            Permission.Create(PermissionConstants.UsersRead, "View user details", 5),
            Permission.Create(PermissionConstants.UsersEdit, "Edit user details", 6),
            Permission.Create(PermissionConstants.UsersDelete, "Delete a user", 7),
            Permission.Create(PermissionConstants.UsersStatusChange, "Activate/Deactivate user", 8),
            Permission.Create(PermissionConstants.UsersReadRoles, "View user roles", 9),
            Permission.Create(PermissionConstants.UsersAssignRoles, "Assign roles to users", 10),

            // Role Management Permissions
            Permission.Create(PermissionConstants.RolesAccess, "Access to role management", 11),
            Permission.Create(PermissionConstants.RolesCreate, "Create a new role", 12),
            Permission.Create(PermissionConstants.RolesRead, "View role details", 13),
            Permission.Create(PermissionConstants.RolesEdit, "Edit a role", 14),
            Permission.Create(PermissionConstants.RolesStatusChange, "Activate/Deactivate a role", 15),
            Permission.Create(PermissionConstants.RolesDelete, "Delete a role", 16),
            Permission.Create(PermissionConstants.RolesReadPermissions, "View permissions for roles", 17),
            Permission.Create(PermissionConstants.RolesManagePermissions, "Manage permissions for roles", 18),

            // App Management Permissions
            Permission.Create(PermissionConstants.AppsAccess, "Access to apps management", 19),
            Permission.Create(PermissionConstants.AppsCreate, "Create a new app", 20),
            Permission.Create(PermissionConstants.AppsRead, "View app details", 21),
            Permission.Create(PermissionConstants.AppsEdit, "Edit app details", 22),
            Permission.Create(PermissionConstants.AppsStatusChange, "Activate/Deactivate apps", 23),
            Permission.Create(PermissionConstants.AppsDelete, "Delete apps", 24),

            // Permission Management Permissions
            Permission.Create(PermissionConstants.PermissionsAccess, "Access to permissions management", 25),
            Permission.Create(PermissionConstants.PermissionsRead, "View permissions", 26),

            // App Portal Permissions
            Permission.Create(PermissionConstants.AppPortalAccess, "Access to the app portal", 27),

            // Audit Logs Permissions
            Permission.Create(PermissionConstants.AuditLogsAccess, "View audit logs", 28),
            Permission.Create(PermissionConstants.AuditLogsExport, "Export audit logs", 29),

            // Settings Management Permissions
            Permission.Create(PermissionConstants.SettingsManage, "Manage application settings", 30)
        };
    }
}
