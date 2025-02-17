using System.Collections.Generic;

namespace Domain.Permissions
{
    public class SsoPermissions
    {
        public SsoPermissions(int appId) 
        {
            AllPermissions = new List<Permission>
            {
                Permission.Create(SsoPermissionConstants.HomeAccess, "View the home page", 1, appId),
                Permission.Create(SsoPermissionConstants.DashboardAccess, "View the dashboard", 2, appId),

                // User Management Permissions
                Permission.Create(SsoPermissionConstants.UsersAccess, "View users", 3, appId),
                Permission.Create(SsoPermissionConstants.UsersCreate, "Create a new user", 4, appId),
                Permission.Create(SsoPermissionConstants.UsersEdit, "Edit user details", 5, appId),
                Permission.Create(SsoPermissionConstants.UsersDelete, "Delete users", 6, appId),
                Permission.Create(SsoPermissionConstants.UsersStatusChange, "Activate/Deactivate user", 7, appId),
                Permission.Create(SsoPermissionConstants.UsersRolesAccess, "View user roles", 8, appId),
                Permission.Create(SsoPermissionConstants.UsersRolesManage, "Manage user roles", 9, appId),

                // Role Management Permissions
                Permission.Create(SsoPermissionConstants.RolesAccess, "View roles", 10, appId),
                Permission.Create(SsoPermissionConstants.RolesCreate, "Create a new role", 11, appId),
                Permission.Create(SsoPermissionConstants.RolesEdit, "Edit role details", 12, appId),
                Permission.Create(SsoPermissionConstants.RolesStatusChange, "Activate/Deactivate a role", 13, appId),
                Permission.Create(SsoPermissionConstants.RolesDelete, "Delete a role", 14, appId),
                Permission.Create(SsoPermissionConstants.RolesPermissionsAccess, "View permissions for roles", 15, appId),
                Permission.Create(SsoPermissionConstants.RolesPermissionsManage, "Manage permissions for roles", 16, appId),

                // App Management Permissions
                Permission.Create(SsoPermissionConstants.AppsAccess, "Access to apps management", 17, appId),
                Permission.Create(SsoPermissionConstants.AppsCreate, "Create a new app", 18, appId),
                Permission.Create(SsoPermissionConstants.AppsEdit, "Edit app details", 19, appId),
                Permission.Create(SsoPermissionConstants.AppsStatusChange, "Activate/Deactivate apps", 20, appId),
                Permission.Create(SsoPermissionConstants.AppsDelete, "Delete apps", 21, appId),

                // Permission Management Permissions
                Permission.Create(SsoPermissionConstants.PermissionsAccess, "Access to permissions management", 22, appId),

                // App Portal Permissions
                Permission.Create(SsoPermissionConstants.AppPortalAccess, "Access to the app portal", 23, appId),

                // Audit Logs Permissions
                Permission.Create(SsoPermissionConstants.AuditLogsAccess, "View audit logs", 24, appId),
                Permission.Create(SsoPermissionConstants.AuditLogsExport, "Export audit logs", 25, appId),

                // Settings Management Permissions
                Permission.Create(SsoPermissionConstants.SettingsAccess, "View application settings", 26, appId),
                Permission.Create(SsoPermissionConstants.SettingsManage, "Manage application settings", 27, appId)
            };
        }

        public List<Permission> AllPermissions { get; }
    }
}
