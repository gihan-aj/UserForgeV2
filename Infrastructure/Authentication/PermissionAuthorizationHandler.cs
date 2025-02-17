using Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionRequirement requirement)
        {
            //string? userId = context.User.Claims.FirstOrDefault(
            //    x => x.Type == JwtRegisteredClaimNames.NameId)?.Value;

            string? userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? appIdString = context.User.FindFirst("appId")?.Value;

            if (userId is null || appIdString is null)
            {
                return;
            }

            if(!int.TryParse(appIdString, out int appId))
            {
                return;
            }

            using IServiceScope scope = _scopeFactory.CreateScope();

            IPermissionService permissionsService = scope.ServiceProvider
                .GetRequiredService<IPermissionService>();

            HashSet<string> permissions =  await permissionsService
                .GetPermissionsAsync(userId, appId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
