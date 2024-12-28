using Application.Abstractions.Data;
using Domain.Users;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            // Add Identity Configuration
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<IdentityRole<string>>()
                .AddRoleManager<RoleManager<IdentityRole<string>>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddPasswordValidator<PasswordValidator<User>>()
                .AddTokenProvider<EmailTokenProvider<User>>(TokenOptions.DefaultEmailProvider)
                .AddTokenProvider<PasswordResetTokenProvider<User>>(TokenOptions.DefaultProvider);

            // Register services
            services.AddServiceRegistrations();

            return services;
        }
    }
}
