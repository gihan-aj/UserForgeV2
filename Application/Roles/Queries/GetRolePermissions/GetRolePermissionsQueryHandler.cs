using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Permissions;
using Domain.Roles;
using SharedKernal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Queries.GetRolePermissions
{
    internal sealed class GetRolePermissionsQueryHandler : IQueryHandler<GetRolePermissionsQuery, RolePermissionsResponse>
    {
        private readonly IRoleManagementService _roleManagementService;

        public GetRolePermissionsQueryHandler(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public async Task<Result<RolePermissionsResponse>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManagementService.GetRoleWithRolePermissionsAsync(request.RoleId);
            if(role is null)
            {
                return Result.Failure<RolePermissionsResponse>(RoleErrors.NotFound.Role(request.RoleId));
            }

            if(role.RolePermissions.Count == 0)
            {
                return Result.Failure<RolePermissionsResponse>(RoleErrors.Permissions.NoPermissionsFound(role.Name!));
            }

            List<RolePermissionResponse> permissions = role.RolePermissions
                .Select(rp => rp.Permission)
                .Select(p => new RolePermissionResponse(
                    Id: p.Id,
                    Name: p.Name,
                    Description: p.Description))
                .ToList();

            return new RolePermissionsResponse(role.Id, role.Name!, role.Description, role.IsActive, role.AppId, role.App!.Name, permissions);
        }
    }
}
