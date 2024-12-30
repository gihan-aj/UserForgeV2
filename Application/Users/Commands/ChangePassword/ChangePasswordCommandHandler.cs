using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ChangePassword
{
    internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IUserService _userService;

        public ChangePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.ChangePasswordAsync(
                request.UserId, 
                request.CurrentPassword, 
                request.NewPassword);

            return result;
        }
    }
}
