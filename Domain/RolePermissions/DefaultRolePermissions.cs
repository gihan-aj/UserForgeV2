using Domain.Permissions;
using Domain.Roles;
using System.Collections.Generic;

namespace Domain.RolePermissions
{
    public static class DefaultRolePermissions
    {
        public static readonly Dictionary<string, List<string>> RolePermissionMappings = new()
        {
            {
                DefaultRoleConstants.SuperAdmin,
                new List<string>
                {
                    PermissionConstants.HomeAccess,
                    PermissionConstants.DashboardAccess,
                    PermissionConstants.UsersAccess,
                    PermissionConstants.UsersCreate,
                    PermissionConstants.UsersRead,
                    PermissionConstants.UsersEdit,
                    PermissionConstants.UsersDelete,
                    PermissionConstants.UsersStatusChange,
                    PermissionConstants.UsersReadRoles,
                    PermissionConstants.UsersAssignRoles,
                    PermissionConstants.RolesAccess,
                    PermissionConstants.RolesCreate,
                    PermissionConstants.RolesRead,
                    PermissionConstants.RolesEdit,
                    PermissionConstants.RolesStatusChange,
                    PermissionConstants.RolesDelete,
                    PermissionConstants.RolesReadPermissions,
                    PermissionConstants.RolesManagePermissions,
                    PermissionConstants.AppsAccess,
                    PermissionConstants.AppsCreate,
                    PermissionConstants.AppsRead,
                    PermissionConstants.AppsEdit,
                    PermissionConstants.AppsStatusChange,
                    PermissionConstants.AppsDelete,
                    PermissionConstants.PermissionsAccess,
                    PermissionConstants.PermissionsRead,
                    PermissionConstants.AppPortalAccess,
                    PermissionConstants.AuditLogsAccess,
                    PermissionConstants.AuditLogsExport,
                    PermissionConstants.SettingsManage
                }
            },
            {
                DefaultRoleConstants.Admin,
                new List<string>
                {
                    PermissionConstants.HomeAccess,
                    PermissionConstants.DashboardAccess,
                    PermissionConstants.UsersAccess,
                    PermissionConstants.UsersCreate,
                    PermissionConstants.UsersRead,
                    PermissionConstants.UsersEdit,
                    PermissionConstants.UsersStatusChange,
                    PermissionConstants.UsersReadRoles,
                    PermissionConstants.UsersAssignRoles,
                    PermissionConstants.RolesAccess,
                    PermissionConstants.RolesRead,
                    PermissionConstants.RolesReadPermissions,
                    PermissionConstants.AppsAccess,
                    PermissionConstants.AppsRead,
                    PermissionConstants.AppsEdit,
                    PermissionConstants.AppsStatusChange,
                    PermissionConstants.PermissionsAccess,
                    PermissionConstants.PermissionsRead,
                    PermissionConstants.AppPortalAccess,
                    PermissionConstants.AuditLogsAccess
                }
            },
            {
                DefaultRoleConstants.Manager,
                new List<string>
                {
                    PermissionConstants.HomeAccess,
                    PermissionConstants.DashboardAccess,
                    PermissionConstants.UsersAccess,
                    PermissionConstants.UsersRead,
                    PermissionConstants.UsersEdit,
                    PermissionConstants.UsersStatusChange,
                    PermissionConstants.UsersReadRoles,
                    PermissionConstants.AppsAccess,
                    PermissionConstants.AppsRead,
                    PermissionConstants.AppsEdit,
                    PermissionConstants.AppPortalAccess
                }
            },
            {
                DefaultRoleConstants.User,
                new List<string>
                {
                    PermissionConstants.HomeAccess,
                    PermissionConstants.DashboardAccess,
                    PermissionConstants.UsersRead,
                    PermissionConstants.AppsRead,
                    PermissionConstants.AppPortalAccess
                }
            }
        };
    }
}
