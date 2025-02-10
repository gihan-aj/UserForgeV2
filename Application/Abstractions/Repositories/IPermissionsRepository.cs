using Domain.Permissions;
using Domain.Roles;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Repositories
{
    public interface IPermissionsRepository
    {
        Task<Result<List<Permission>>> GetAllPermissionsWithAssignedRoles(CancellationToken cancellationToken);

        Task<Result> AssignRolePermissionsAsync(
            Role role,
            List<string> permissionIds,
            string modifiedBy,
            CancellationToken cancellationToken);
    }
}
