using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SendPasswordReset
{
    internal sealed class SendPasswordResetCommandHandler : ICommandHandler<SendPasswordResetCommand>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public SendPasswordResetCommandHandler(
            IUserService userService, 
            IEmailService emailService, 
            ITokenService tokenService)
        {
            _userService = userService;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Result> Handle(SendPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _userService.FindByEmailAsync(request.Email);
            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var user = userResult.Value;

            var token = await _userService.GeneratePasswordResetTokenAsync(user);
            if(token is null)
            {
                return UserErrors.General.OperationFailed("Password reset token generatiion failed.");
            }

            var emailResult = await _emailService.SendPasswordResetEmailAsync(user, token);
            if (emailResult.IsFailure)
            {
                return emailResult.Error;
            }

            return Result.Success();
        }
    }
}
