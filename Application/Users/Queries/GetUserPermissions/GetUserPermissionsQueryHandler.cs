using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Permissions;
using SharedKernal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserPermissions
{
    internal sealed class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, List<string>>
    {
        private readonly IPermissionService _permissionService;

        public GetUserPermissionsQueryHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<Result<List<string>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            HashSet<string> result = await _permissionService.GetPermissionsAsync(request.UserId, cancellationToken);
            if (result.Count == 0)
            {
                return Result.Failure<List<string>>(PermissionErrors.NotFound.MissingUserPermissions);
            }

            return result.ToList();
        }
    }
}
