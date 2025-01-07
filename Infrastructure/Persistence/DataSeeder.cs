using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Domain.Roles;

namespace Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // Seed roles
            var roles = new[] { Roles.Admin, Roles.Manager, Roles.User };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed admin user
            var adminEmail = "admin@userforge.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = User.Create("system", "administrator", adminEmail);
                adminUser.EmailConfirmed = true;

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    //await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    await userManager.AddToRolesAsync(adminUser, [Roles.Admin, Roles.User]);
                }
            }
        }
    }
}
