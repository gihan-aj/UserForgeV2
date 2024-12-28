using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ResendEmailConfirmation
{
    internal sealed class ResendEmailConfirmationCommandHandler : ICommandHandler<ResendEmailConfirmationCommand>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public ResendEmailConfirmationCommandHandler(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _userService.FindByEmailAsync(request.Email);
            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var user = userResult.Value;
            if(user.EmailConfirmed)
            {
                return UserErrors.Conflict.EmailAlreadyConfirmed(request.Email);
            }

            var tokenResult = await _userService.GenerateEmailConfirmationTokenAsync(user);
            if (tokenResult.IsFailure)
            {
                return tokenResult.Error;
            }

            // Confirmation link via email
            var emailResult = await _emailService.SendConfirmationEmailAsync(user, tokenResult.Value);
            if (emailResult.IsFailure)
            {
                return emailResult.Error;
            }

            return Result.Success();
        }
    }
}
