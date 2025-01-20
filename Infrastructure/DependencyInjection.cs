using Application.Abstractions.Data;
using Domain.Roles;
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
            services.AddDbContext<Persistence.ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped((System.Func<System.IServiceProvider, Application.Abstractions.Data.IApplicationDbContext>)(sp => sp.GetRequiredService<Persistence.ApplicationDbContext>()));

            services.AddScoped((System.Func<System.IServiceProvider, IUnitOfWork>)(sp => sp.GetRequiredService<Persistence.ApplicationDbContext>()));

            // Add Identity Configuration
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;

                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<Persistence.ApplicationDbContext>()
                .AddPasswordValidator<PasswordValidator<User>>()
                .AddTokenProvider<EmailTokenProvider<User>>(TokenOptions.DefaultEmailProvider)
                .AddTokenProvider<PasswordResetTokenProvider<User>>(TokenOptions.DefaultProvider);

            // Register services
            services.AddServiceRegistrations();

            return services;
        }
    }
}
