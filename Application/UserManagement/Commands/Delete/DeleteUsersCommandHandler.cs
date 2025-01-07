using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.Delete
{
    internal sealed class DeleteUsersCommandHandler : ICommandHandler<DeleteUsersCommand, List<string>>
    {
        private readonly IUserManagementService _userManagementService;

        public DeleteUsersCommandHandler(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<Result<List<string>>> Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
        {
            var result = await _userManagementService.DeleteUsers(request.UserIds, request.DeletedBy, cancellationToken);
            return result;
        }
    }
}
