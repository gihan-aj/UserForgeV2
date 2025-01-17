using Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
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

            if (userId is null)
            {
                return;
            }

            using IServiceScope scope = _scopeFactory.CreateScope();

            IPermissionService permissionsRepository = scope.ServiceProvider
                .GetRequiredService<IPermissionService>();

            HashSet<string> permissions =  await permissionsRepository
                .GetPermissionsAsync(userId);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
