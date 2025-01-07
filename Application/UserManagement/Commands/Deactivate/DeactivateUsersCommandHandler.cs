using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.Deactivate
{
    internal sealed class DeactivateUsersCommandHandler : ICommandHandler<DeactivateUsersCommand, List<string>>
    {
        private readonly IUserManagementService _userManagementService;

        public DeactivateUsersCommandHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public Task<Result<List<string>>> Handle(DeactivateUsersCommand request, CancellationToken cancellationToken)
        {
            var result = _userManagementService.DeactivateUsers(request.Ids, cancellationToken);
            return result;
        }
    }
}
