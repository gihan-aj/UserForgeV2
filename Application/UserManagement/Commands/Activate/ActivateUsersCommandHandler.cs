using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.Activate
{
    internal sealed class ActivateUsersCommandHandler : ICommandHandler<ActivateUsersCommand, List<string>>
    {
        private readonly IUserManagementService _userManagementService;

        public ActivateUsersCommandHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public Task<Result<List<string>>> Handle(ActivateUsersCommand request, CancellationToken cancellationToken)
        {
            var result = _userManagementService.ActivateUsersAsync(request.Ids, request.UserId, cancellationToken);
            return result;
        }
    }
}
