using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Domain.Roles;
using Application.Abstractions.Data;
using Domain.Permissions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var createdBy = "default";

            // Permissions
            IEnumerable<Permission> permissions = PermissionConstants.AllPermissions
                .Select(p => Permission.Create(p!, null, createdBy));

            foreach (var permission in permissions)
            {
                if (!await context.Permissions.AnyAsync(p => p.Name == permission.Name))
                {
                    context.Permissions.Add(permission);
                }
            }

            await context.SaveChangesAsync();

            var allRolePermissions = new Dictionary<string, string[]>
            {
                {
                    RoleConstants.Admin,
                    new[]
                    {
                        PermissionConstants.UsersCreate,
                        PermissionConstants.UsersRead,
                        PermissionConstants.UsersEdit,
                        PermissionConstants.UsersDelete,
                        PermissionConstants.UsersStatusChange,
                        PermissionConstants.UsersAssignRoles,
                        PermissionConstants.RolesCreate,
                        PermissionConstants.RolesRead,
                        PermissionConstants.RolesEdit,
                        PermissionConstants.RolesDelete,
                        PermissionConstants.RolesManagePermissions,
                        PermissionConstants.PermissionsCreate,
                        PermissionConstants.PermissionsRead,
                        PermissionConstants.PermissionsEdit,
                        PermissionConstants.PermissionsDelete,
                        PermissionConstants.AuditLogsView,
                        PermissionConstants.AuditLogsExport,
                        PermissionConstants.SettingsManage,
                        PermissionConstants.DashboardAccess
                    }
                },
                {
                    RoleConstants.Manager,
                    new[]
                    {
                        PermissionConstants.UsersRead,
                        PermissionConstants.UsersEdit,
                        PermissionConstants.UsersStatusChange,
                        PermissionConstants.UsersAssignRoles,
                        PermissionConstants.RolesRead,
                        PermissionConstants.DashboardAccess
                    }
                },
                {
                    RoleConstants.User,
                    new[]
                    {
                        PermissionConstants.DashboardAccess,
                        PermissionConstants.UsersRead
                    }
                }
            };

            // Roles
            IEnumerable<Role> roles = RoleConstants.AllRoles
                .Select(r => new Role(r!, null, createdBy));

            foreach (var role in roles)
            {
                if(!await context.Roles.AnyAsync(r => r.Name == role.Name))
                {
                    await roleManager.CreateAsync(role);
                    //context.Roles.Add(role);
                }
            }

            await context.SaveChangesAsync();

            // Assign role permissions
            foreach(var role in roles)
            {
                var dbRole = await context.Roles
                    .Include(u => u.RolePermissions)
                    .FirstOrDefaultAsync(r => r.Name == role.Name);

                if(dbRole is not null)
                {
                    if(dbRole.RolePermissions.Count() == 0)
                    {
                        var permissionsToAdd = new List<Permission>();
                        foreach (string permissionName in allRolePermissions[dbRole.Name!])
                        {
                            Permission? permission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName);
                            if (permission is not null)
                            {
                                permissionsToAdd.Add(permission);
                            }
                        }

                        dbRole.AddRolePermissionsRange(permissionsToAdd, createdBy);
                    }
                    
                }
            }

            await unitOfWork.SaveChangesAsync();

            // Users
            // Admin
            var adminFirstName = "system";
            var adminLastName = "administrator";
            var adminEmail = "admin@userforge.com";
            var adminPassword = "Admin@123";

            User admin = User.Create(adminFirstName, adminLastName, adminEmail, createdBy);
            admin.EmailConfirmed = true;

            if(!await context.Users.AnyAsync(u => u.Email == adminEmail))
            {
                var userCreatedResult = await userManager.CreateAsync(admin, adminPassword);
                if (userCreatedResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, [RoleConstants.Admin, RoleConstants.User]);
                }
            }

            //Manager
            var managerFirstName = "system";
            var managerLastName = "manager";
            var managerEmail = "manager@userforge.com";
            var managerPassword = "Manager@123";

            User manager = User.Create(managerFirstName, managerLastName, managerEmail, createdBy);
            manager.EmailConfirmed = true;

            if(!await context.Users.AnyAsync(u => u.Email == managerEmail))
            {
                var userCreatedResult = await userManager.CreateAsync(manager, managerPassword);
                if (userCreatedResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(manager, [RoleConstants.Manager, RoleConstants.User]);
                }
            }

            await unitOfWork.SaveChangesAsync();

            //User
            var userFirstName = "system";
            var userLastName = "user";
            var userEmail = "user@userforge.com";
            var userPassword = "User@123";

            User user = User.Create(userFirstName, userLastName, userEmail, createdBy);
            user.EmailConfirmed = true;

            if(!await userManager.Users.AnyAsync(u => u.Email == userEmail))
            {
                var userCreatedResult = await userManager.CreateAsync(user, userPassword);
                if (userCreatedResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, [RoleConstants.User]);
                }
            }
        }
    }
}
