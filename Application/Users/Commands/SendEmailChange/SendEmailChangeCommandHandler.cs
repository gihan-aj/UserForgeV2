using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.SendEmailChange
{
    internal sealed class SendEmailChangeCommandHandler : ICommandHandler<SendEmailChangeCommand>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public SendEmailChangeCommandHandler(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(SendEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _userService.GetByIdAsync(request.UserId);
            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var user = userResult.Value;

            if(await _userService.CheckPasswordAsync(user, request.Password))
            {
                var token = await _userService.GenerateChangeEmailTokenAsync(user, request.NewEmail);
                if (token is null)
                {
                    return UserErrors.General.OperationFailed("Email change token generation failed.");
                }

                var emailResult = await _emailService.SendEmailChangeEmailAsync(user, token, request.NewEmail);
                if (emailResult.IsFailure)
                {
                    return emailResult.Error;
                }

                return Result.Success();
            }
            else
            {
                return UserErrors.Validation.InvalidPassword;
            }
        }
    }
}
