using Domain.Permissions;
using Domain.Roles;
using System.Collections.Generic;

namespace Domain.RolePermissions
{
    public static class SsoAppDefaultRolePermissions
    {
        public static readonly Dictionary<string, List<string>> RolePermissionMappings = new()
        {
            {
                SsoAppDefaultRoleConstants.SuperAdmin,
                new List<string>
                {
                    SsoPermissionConstants.HomeAccess,
                    SsoPermissionConstants.DashboardAccess,

                    SsoPermissionConstants.UsersAccess,
                    SsoPermissionConstants.UsersCreate,
                    SsoPermissionConstants.UsersEdit,
                    SsoPermissionConstants.UsersDelete,
                    SsoPermissionConstants.UsersStatusChange,
                    SsoPermissionConstants.UsersRolesAccess,
                    SsoPermissionConstants.UsersRolesManage,

                    SsoPermissionConstants.RolesAccess,
                    SsoPermissionConstants.RolesCreate,
                    SsoPermissionConstants.RolesEdit,
                    SsoPermissionConstants.RolesStatusChange,
                    SsoPermissionConstants.RolesDelete,
                    SsoPermissionConstants.RolesPermissionsAccess,
                    SsoPermissionConstants.RolesPermissionsManage,

                    SsoPermissionConstants.AppsAccess,
                    SsoPermissionConstants.AppsCreate,
                    SsoPermissionConstants.AppsEdit,
                    SsoPermissionConstants.AppsStatusChange,
                    SsoPermissionConstants.AppsDelete,

                    SsoPermissionConstants.PermissionsAccess,

                    SsoPermissionConstants.AppPortalAccess,

                    SsoPermissionConstants.AuditLogsAccess,
                    SsoPermissionConstants.AuditLogsExport,

                    SsoPermissionConstants.SettingsAccess,
                    SsoPermissionConstants.SettingsManage
                }
            },
            {
                SsoAppDefaultRoleConstants.Admin,
                new List<string>
                {
                    SsoPermissionConstants.HomeAccess,

                    SsoPermissionConstants.DashboardAccess,

                    SsoPermissionConstants.UsersAccess,
                    SsoPermissionConstants.UsersCreate,
                    SsoPermissionConstants.UsersEdit,
                    SsoPermissionConstants.UsersStatusChange,
                    SsoPermissionConstants.UsersRolesAccess,
                    SsoPermissionConstants.UsersRolesManage,

                    SsoPermissionConstants.RolesAccess,
                    SsoPermissionConstants.RolesPermissionsAccess,

                    SsoPermissionConstants.AppsAccess,
                    SsoPermissionConstants.AppsEdit,
                    SsoPermissionConstants.AppsStatusChange,

                    SsoPermissionConstants.PermissionsAccess,

                    SsoPermissionConstants.AppPortalAccess,

                    SsoPermissionConstants.AuditLogsAccess
                }
            },
            {
                SsoAppDefaultRoleConstants.Manager,
                new List<string>
                {
                    SsoPermissionConstants.HomeAccess,

                    SsoPermissionConstants.DashboardAccess,

                    SsoPermissionConstants.UsersAccess,
                    SsoPermissionConstants.UsersEdit,
                    SsoPermissionConstants.UsersStatusChange,
                    SsoPermissionConstants.UsersRolesAccess,

                    SsoPermissionConstants.AppsAccess,
                    SsoPermissionConstants.AppsEdit,

                    SsoPermissionConstants.AppPortalAccess
                }
            },
            {
                SsoAppDefaultRoleConstants.User,
                new List<string>
                {
                    SsoPermissionConstants.HomeAccess,

                    SsoPermissionConstants.DashboardAccess,

                    SsoPermissionConstants.UsersAccess,

                    SsoPermissionConstants.AppsAccess,

                    SsoPermissionConstants.AppPortalAccess
                }
            }
        };
    }
}
