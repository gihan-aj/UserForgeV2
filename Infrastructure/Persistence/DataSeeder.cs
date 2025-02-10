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
using Domain.RolePermissions;

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
                // Seed permissions
                var permissions = Permissions.AllPermissions;
                var dbPermissions = await context.Permissions.ToListAsync();
                var permissionsToAdd = permissions.Except(dbPermissions);
                var permissionsToRemove = dbPermissions.Except(permissions);

                foreach (var permission in permissionsToRemove)
                {
                    context.Permissions.Remove(permission);
                }

                foreach (var permission in permissionsToAdd)
                {
                    context.Permissions.Add(permission);
                }

                await context.SaveChangesAsync();

                // Seed roles and assign permissions
                foreach (var roleMapping in DefaultRolePermissions.RolePermissionMappings)
                {
                    var roleName = roleMapping.Key;
                    var rolePermissionNames = roleMapping.Value;

                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role is null)
                    {
                        role = new Role(roleName, null, createdBy);
                        var roleCreatedResult = await roleManager.CreateAsync(role);
                        if (!roleCreatedResult.Succeeded)
                        {
                            var errors = string.Join(", ", roleCreatedResult.Errors.Select(e => e.Description));
                            throw new Exception($"Failed to create role: {errors}");
                        }

                        foreach (var rolePermissionName in rolePermissionNames)
                        {
                            var rolePermisssion = permissions.FirstOrDefault(p => p.Name == rolePermissionName);
                            if (rolePermisssion is not null)
                            {
                                role.RolePermissions.Add(new RolePermission
                                {
                                    RoleId = role.Id,
                                    PermissionId = rolePermisssion.Id
                                });
                            }
                        }
                    }
                    else
                    {
                        var existingRolePermissions = await context.RolePermissions
                            .Where(rp => rp.RoleId == role.Id)
                            .Include(rp => rp.Permission)
                            .ToListAsync();

                        var existingRolePermissionNames = existingRolePermissions
                            .Select(rp => rp.Permission)
                            .Select(p => p.Name);

                        var rolePermissionsToAdd = rolePermissionNames
                            .Except(existingRolePermissionNames);

                        var rolePermissionsToRemove = existingRolePermissionNames
                            .Except(rolePermissionNames);

                        if (rolePermissionsToRemove.Count() > 0)
                        {
                            foreach (var rolePermissionToRemove in rolePermissionsToRemove)
                            {
                                var permissionToRemove = existingRolePermissions
                                    .FirstOrDefault(rp => rp.Permission.Name == rolePermissionToRemove);
                                if (permissionToRemove is not null)
                                {
                                    role.RolePermissions.Remove(permissionToRemove);
                                }
                            }
                        }

                        if (rolePermissionsToAdd.Count() > 0)
                        {
                            foreach (var rolePermissionToAdd in rolePermissionsToAdd)
                            {
                                var permissionToAdd = permissions.FirstOrDefault(p => p.Name == rolePermissionToAdd);
                                if (permissionToAdd is not null)
                                {
                                    role.RolePermissions.Add(new RolePermission
                                    {
                                        RoleId = role.Id,
                                        PermissionId = permissionToAdd.Id
                                    });
                                }
                            }
                        }
                    }

                    await context.SaveChangesAsync();
                }

                // Users
                // Super Admin
                var superadminFirstName = "system";
                var superadminLastName = "superadministrator";
                var superadminEmail = "superadmin@userforge.com";
                var superadminPassword = "SuperAdmin@123";

                User superadmin = User.Create(superadminFirstName, superadminLastName, superadminEmail, createdBy);
                superadmin.EmailConfirmed = true;

                if (!await context.Users.AnyAsync(u => u.Email == superadminEmail))
                {
                    var userCreatedResult = await userManager.CreateAsync(superadmin, superadminPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        await userManager.AddToRolesAsync(superadmin, [DefaultRoleConstants.SuperAdmin]);
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

                User admin = User.Create(adminFirstName, adminLastName, adminEmail, createdBy);
                admin.EmailConfirmed = true;

                if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
                {
                    var userCreatedResult = await userManager.CreateAsync(admin, adminPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        await userManager.AddToRolesAsync(admin, [DefaultRoleConstants.Admin]);
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

                User manager = User.Create(managerFirstName, managerLastName, managerEmail, createdBy);
                manager.EmailConfirmed = true;

                if (!await context.Users.AnyAsync(u => u.Email == managerEmail))
                {
                    var userCreatedResult = await userManager.CreateAsync(manager, managerPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        await userManager.AddToRolesAsync(manager, [DefaultRoleConstants.Manager]);
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

                User user = User.Create(userFirstName, userLastName, userEmail, createdBy);
                user.EmailConfirmed = true;

                if (!await userManager.Users.AnyAsync(u => u.Email == userEmail))
                {
                    var userCreatedResult = await userManager.CreateAsync(user, userPassword);
                    if (userCreatedResult.Succeeded)
                    {
                        await userManager.AddToRolesAsync(user, [DefaultRoleConstants.User]);
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
