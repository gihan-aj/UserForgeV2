using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ResendEmailConfirmation
{
    internal sealed class ResendEmailConfirmationCommandHandler : ICommandHandler<ResendEmailConfirmationCommand, User>
    {
        private readonly IUsersService _usersService;

        public ResendEmailConfirmationCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public Task<Result<User>> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            return _usersService.ResendEmailConfirmationLink(request.Email);
        }
    }
}
