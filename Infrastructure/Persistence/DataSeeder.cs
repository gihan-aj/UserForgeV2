using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Domain.Roles;
using Application.Abstractions.Repositories;
using Application.Abstractions.Data;

namespace Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var userSettingsRepository = scope.ServiceProvider.GetRequiredService<IUserSettingsRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            // Seed roles
            var roles = new[] { Roles.Admin, Roles.Manager, Roles.User };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role(role, null, "default"));
                }
            }

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
                    var userSettings = new UserSettings(adminUser.Id);
                    userSettingsRepository.Add(userSettings);
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}
