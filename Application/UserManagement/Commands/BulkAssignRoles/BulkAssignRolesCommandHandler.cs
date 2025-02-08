using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.BulkAssignRoles
{
    internal sealed class BulkAssignRolesCommandHandler : ICommandHandler<BulkAssignRolesCommand>
    {
        private readonly IUserManagementService _userManagementService;

        public BulkAssignRolesCommandHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<Result> Handle(BulkAssignRolesCommand request, CancellationToken cancellationToken)
        {
            foreach (var userId in request.UserIds)
            {
                var result = await _userManagementService.AssignUserRolesAsync(
                    userId,
                    request.RoleNames.ToList(),
                    request.AssignedBy,
                    cancellationToken);

                if (result.IsFailure)
                {
                    return result.Error;
                }
            }
            

            return Result.Success();
        }
    }
}
