using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permissions.Queries.GetAll
{
    internal sealed class GetPermissionsQueryHandler : IQueryHandler<GetAllPermissionsQuery, List<PermissionDetails>>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public GetPermissionsQueryHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task<Result<List<PermissionDetails>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            Result<List<Permission>> result = await _permissionsRepository.GetAllPermissionsWithAssignedRoles(cancellationToken);

            var permissionWithRoleNames = result.Value
                .Select(p => new PermissionDetails(
                    Id: p.Id,
                    Name: p.Name,
                    Description: p.Description,
                    RoleNames: p.RolePermissions
                        .Select(rp => rp.Role)
                        .Select(r => r.Name)
                        .ToArray()))
                .ToList();

            return Result.Success(permissionWithRoleNames);
        }
    }
}
