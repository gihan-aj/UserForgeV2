using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly IUserService _userService;

        public ConfirmEmailCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _userService.ConfirmEmailAsync(request.UserId, request.Token);
        }
    }
}
