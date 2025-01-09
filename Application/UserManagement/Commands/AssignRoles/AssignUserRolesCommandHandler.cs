using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.AssignRoles
{
    internal sealed class AssignUserRolesCommandHandler : ICommandHandler<AssignUserRolesCommand, List<string>>
    {
        private readonly IUserManagementService _userManagementService;

        public AssignUserRolesCommandHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<Result<List<string>>> Handle(AssignUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _userManagementService.AssignUserRolesAsync(
                request.UserId, 
                request.RoleNames, 
                request.ModifiedBy, 
                cancellationToken);

            return result; 
        }
    }
}
