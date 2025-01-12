using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Permissions.Queries.GetAll;
using Domain.Permissions;
using Domain.Roles;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Queries.GetRolePermissions
{
    internal sealed class GetRolePermissionsQueryHandler : IQueryHandler<GetRolePermissionsQuery, GetRolePermissionsResponse>
    {
        private readonly IRoleManagementService _roleManagementService;
        private readonly IPermissionsRepository _permissionsRepository;

        public GetRolePermissionsQueryHandler(IRoleManagementService roleManagementService, IPermissionsRepository permissionsRepository)
        {
            _roleManagementService = roleManagementService;
            _permissionsRepository = permissionsRepository;
        }

        public async Task<Result<GetRolePermissionsResponse>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManagementService.GetRoleWithRolePermissionsAsync(request.RoleId);
            if(role is null)
            {
                return Result.Failure<GetRolePermissionsResponse>(RoleErrors.NotFound.Role(request.RoleId));
            }

            if(role.RolePermissions.Count == 0)
            {
                return Result.Failure<GetRolePermissionsResponse>(RoleErrors.Permissions.NoPermissionsFound(role.Name!));
            }

            var permissions = new List<GetAllPermissionsResponse>();

            foreach(var rolePermission in role.RolePermissions)
            {
                var permission = await _permissionsRepository.GetByIdAsync(rolePermission.PermissionId!);
                if(permission is null)
                {
                    return Result.Failure<GetRolePermissionsResponse>(PermissionErrors.NotFound.MissingPermissions);
                }

                permissions.Add(new GetAllPermissionsResponse(
                    permission.Id,
                    permission.Name,
                    permission.Description,
                    permission.IsActive));
            }

            return new GetRolePermissionsResponse(role.Id, role.Name!, role.Description, role.IsActive, permissions);
        }
    }
}
