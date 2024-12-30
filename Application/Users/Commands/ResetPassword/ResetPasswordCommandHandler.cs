using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ResetPassword
{
    internal sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.ResetPasswordAsync(
                request.UserId, 
                request.Token, 
                request.NewPassword);

            return result;
        }
    }
}
