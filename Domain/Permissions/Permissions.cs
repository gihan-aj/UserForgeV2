using System.Collections.Generic;

namespace Domain.Permissions
{
    public static class Permissions
    {
        public static readonly List<Permission> AllPermissions = new List<Permission> 
        {
            Permission.Create(PermissionConstants.HomeAccess, "Access to the home page"),
            Permission.Create(PermissionConstants.DashboardAccess, "Access to the dashboard"),

            // User Management Permissions
            Permission.Create(PermissionConstants.UsersAccess, "Access to user management"),
            Permission.Create(PermissionConstants.UsersCreate, "Create a new user"),
            Permission.Create(PermissionConstants.UsersRead, "View user details"),
            Permission.Create(PermissionConstants.UsersEdit, "Edit user details"),
            Permission.Create(PermissionConstants.UsersDelete, "Delete a user"),
            Permission.Create(PermissionConstants.UsersStatusChange, "Activate/Deactivate user"),
            Permission.Create(PermissionConstants.UsersReadRoles, "View user roles"),
            Permission.Create(PermissionConstants.UsersAssignRoles, "Assign roles to users"),

            // Role Management Permissions
            Permission.Create(PermissionConstants.RolesAccess, "Access to role management"),
            Permission.Create(PermissionConstants.RolesCreate, "Create a new role"),
            Permission.Create(PermissionConstants.RolesRead, "View role details"),
            Permission.Create(PermissionConstants.RolesEdit, "Edit a role"),
            Permission.Create(PermissionConstants.RolesStatusChange, "Activate/Deactivate a role"),
            Permission.Create(PermissionConstants.RolesDelete, "Delete a role"),
            Permission.Create(PermissionConstants.RolesReadPermissions, "View permissions for roles"),
            Permission.Create(PermissionConstants.RolesManagePermissions, "Manage permissions for roles"),

            // App Management Permissions
            Permission.Create(PermissionConstants.AppsAccess, "Access to apps management"),
            Permission.Create(PermissionConstants.AppsCreate, "Create a new app"),
            Permission.Create(PermissionConstants.AppsRead, "View app details"),
            Permission.Create(PermissionConstants.AppsEdit, "Edit app details"),
            Permission.Create(PermissionConstants.AppsStatusChange, "Activate/Deactivate apps"),
            Permission.Create(PermissionConstants.AppsDelete, "Delete apps"),

            // Permission Management Permissions
            Permission.Create(PermissionConstants.PermissionsAccess, "Access to permissions management"),
            Permission.Create(PermissionConstants.PermissionsRead, "View permissions"),
            Permission.Create(PermissionConstants.PermissionsEdit, "Edit a permission"),
            Permission.Create(PermissionConstants.PermissionsStatusChange, "Activate/Deactivate a permission"),

            // App Portal Permissions
            Permission.Create(PermissionConstants.AppPortalAccess, "Access to the app portal"),

            // Audit Logs Permissions
            Permission.Create(PermissionConstants.AuditLogsAccess, "View audit logs"),
            Permission.Create(PermissionConstants.AuditLogsExport, "Export audit logs"),

            // Settings Management Permissions
            Permission.Create(PermissionConstants.SettingsManage, "Manage application settings")
        };
    }
}
