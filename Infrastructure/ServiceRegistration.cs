using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Infrastructure.Authentication;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserManagementService, UserManagementService>();
            services.AddTransient<IRoleManagementService, RoleManagementService>();
            services.AddTransient<IAppManagementService, AppManagementService>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
            services.AddScoped<IPermissionsRepository, PermissionsRepository>();
            services.AddScoped<IAppsRepository, AppsRepository>();

            services.AddTransient<IPermissionService, PermissionService>();

            return services;
        }
    }
}
