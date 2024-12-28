using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Register
{
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, string>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public RegisterUserCommandHandler(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUserResult = await _userService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth,
                request.Password);

            if (newUserResult.IsFailure)
            {
                return Result.Failure<string>(newUserResult.Error);
            }

            var newUser = newUserResult.Value;

            var tokenResult = await _userService.GenerateEmailConfirmationTokenAsync(newUser);
            if (tokenResult.IsFailure)
            {
                return Result.Failure<string>(tokenResult.Error);
            }

            // Confirmation link via email
            var emailResult = await _emailService.SendConfirmationEmailAsync(newUser, tokenResult.Value);
            if (emailResult.IsFailure)
            {
                return Result.Failure<string>(emailResult.Error);
            }

            return newUser.Id;       
        }
    }
}
