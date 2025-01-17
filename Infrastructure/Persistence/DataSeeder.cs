using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Domain.Roles;
using Application.Abstractions.Repositories;
using Application.Abstractions.Data;
using System.ComponentModel.DataAnnotations;
using Domain.Permissions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            // Seed permissions
            var permissions = DefaultPermissions.AllPermissions
                .Select(p => Permission.Create(p, null, "default"));

            foreach ( var permission in permissions )
            {
                if(!await context.Permissions.AnyAsync(p => p.Name == permission.Name))
                {
                    await context.Permissions.AddAsync(permission);
                }
            }

            // Seed roles
            var roles = new[] { Roles.Admin, Roles.Manager, Roles.User };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role(role, null, "default"));
                }
            }

            //var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
            //if (adminRole is not null)
            //{
            //    foreach (var permission in permissions)
            //    {
            //        if (!await context.RolePermissions.AnyAsync(rp => rp.RoleId == adminRole.Id && rp.PermissionId == permission.Id))
            //        {
            //            var rolePermission = new Domain.RolePermissions.RolePermission
            //            {
            //                RoleId = adminRole.Id,
            //                PermissionId = permission.Id
            //            };

            //            adminRole.RolePermissions.Add(rolePermission);
            //        }

            //    }

            //    await roleManager.UpdateAsync(adminRole);
            //}

            // Seed admin user
            var adminEmail = "admin@userforge.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = User.Create("system", "administrator", adminEmail, "default");
                adminUser.EmailConfirmed = true;

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    //await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    await userManager.AddToRolesAsync(adminUser, [Roles.Admin, Roles.User]);
                    //await unitOfWork.SaveChangesAsync();
                }
            }

        }
    }
}
