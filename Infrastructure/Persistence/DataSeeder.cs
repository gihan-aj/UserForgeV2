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
using Domain.RolePermissions;
using Domain.Apps;

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

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var ssoApp = await context.Apps.FirstOrDefaultAsync(a => a.Name == AppNames.SsoApp);
                if(ssoApp is null)
                {
                    ssoApp = App.Create(AppNames.SsoApp, "Centralized user management and authentication platform", createdBy);
                    context.Apps.Add(ssoApp);
                    await unitOfWork.SaveChangesAsync();
                }

                var ssoPermissions = new SsoPermissions(ssoApp.Id);

                if (!await context.Permissions.AnyAsync(p => p.AppId == ssoApp.Id))
                {
                    foreach(var permission in ssoPermissions.AllPermissions)
                    {
                        context.Permissions.Add(permission);
                    }

                    await unitOfWork.SaveChangesAsync();
                }

                foreach(var roleMappings in SsoAppDefaultRolePermissions.RolePermissionMappings)
                {
                    var roleName = roleMappings.Key;
                    if(!await context.Roles.AnyAsync(r => r.Name == roleName && r.AppId == ssoApp.Id))
                    {
                        var role = new Role(roleName, null, ssoApp.Id, createdBy);
                        role.NormalizedName = roleManager.NormalizeKey(roleName);
                        context.Roles.Add(role);

                        var rolePermissions = roleMappings.Value;
                        foreach(var rolePermissionName in rolePermissions)
                        {
                            var rolePermission = ssoPermissions.AllPermissions.FirstOrDefault(p => p.Name == rolePermissionName);
                            if(rolePermission is not null)
                            {
                                role.RolePermissions.Add(new RolePermission
                                {
                                    RoleId = role.Id,
                                    PermissionId = rolePermission.Id
                                });
                            }
                        }
                        await unitOfWork.SaveChangesAsync();
                    }

                }

                // Users
                // Super Admin
                var superadminFirstName = "system";
                var superadminLastName = "superadministrator";
                var superadminEmail = "superadmin@userforge.com";
                var superadminPassword = "SuperAdmin@123";
             
                if (!await context.Users.AnyAsync(u => u.Email == superadminEmail))
                {
                    User superadmin = User.Create(superadminFirstName, superadminLastName, superadminEmail, createdBy);
                    superadmin.EmailConfirmed = true;

                    var userCreatedResult = await userManager.CreateAsync(superadmin, superadminPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        var superAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == SsoAppDefaultRoleConstants.SuperAdmin && r.AppId == ssoApp.Id);
                        if(superAdminRole is not null)
                        {
                            superadmin.UserRoles.Add(new Domain.UserRoles.UserRole
                            {
                                UserId = superadmin.Id,
                                RoleId = superAdminRole.Id
                            });

                            ssoApp.Users.Add(superadmin);

                            await unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var errors = string.Join(", ", userCreatedResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to create superadmin user: {errors}");
                    }
                }

                // Admin
                var adminFirstName = "system";
                var adminLastName = "administrator";
                var adminEmail = "admin@userforge.com";
                var adminPassword = "Admin@123";

                if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
                {
                    User admin = User.Create(adminFirstName, adminLastName, adminEmail, createdBy);
                    admin.EmailConfirmed = true;

                    var userCreatedResult = await userManager.CreateAsync(admin, adminPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == SsoAppDefaultRoleConstants.Admin && r.AppId == ssoApp.Id);
                        if (adminRole is not null)
                        {
                            admin.UserRoles.Add(new Domain.UserRoles.UserRole
                            {
                                UserId = admin.Id,
                                RoleId = adminRole.Id
                            });

                            ssoApp.Users.Add(admin);

                            await unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var errors = string.Join(", ", userCreatedResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to create admin user: {errors}");
                    }
                }

                //Manager
                var managerFirstName = "system";
                var managerLastName = "manager";
                var managerEmail = "manager@userforge.com";
                var managerPassword = "Manager@123";

                if (!await context.Users.AnyAsync(u => u.Email == managerEmail))
                {
                    User manager = User.Create(managerFirstName, managerLastName, managerEmail, createdBy);
                    manager.EmailConfirmed = true;

                    var userCreatedResult = await userManager.CreateAsync(manager, managerPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        var managerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == SsoAppDefaultRoleConstants.Manager && r.AppId == ssoApp.Id);
                        if (managerRole is not null)
                        {
                            manager.UserRoles.Add(new Domain.UserRoles.UserRole
                            {
                                UserId = manager.Id,
                                RoleId = managerRole.Id
                            });

                            ssoApp.Users.Add(manager);

                            await unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var errors = string.Join(", ", userCreatedResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to create manager user: {errors}");
                    }
                }

                await unitOfWork.SaveChangesAsync();

                //User
                var userFirstName = "system";
                var userLastName = "user";
                var userEmail = "user@userforge.com";
                var userPassword = "User@123";
             
                if (!await userManager.Users.AnyAsync(u => u.Email == userEmail))
                {
                    User user = User.Create(userFirstName, userLastName, userEmail, createdBy);
                    user.EmailConfirmed = true;

                    var userCreatedResult = await userManager.CreateAsync(user, userPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == SsoAppDefaultRoleConstants.User && r.AppId == ssoApp.Id);
                        if (userRole is not null)
                        {
                            user.UserRoles.Add(new Domain.UserRoles.UserRole
                            {
                                UserId = user.Id,
                                RoleId = userRole.Id
                            });

                            ssoApp.Users.Add(user);

                            await unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var errors = string.Join(", ", userCreatedResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to create user: {errors}");
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Seeding failed. Rolling back changes.", ex);
            }

            
        }
    }
}
