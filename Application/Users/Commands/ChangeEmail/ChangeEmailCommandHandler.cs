using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ChangeEmail
{
    internal sealed class ChangeEmailCommandHandler : ICommandHandler<ChangeEmailCommand>
    {
        private readonly IUserService _userService;

        public ChangeEmailCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _userService.GetByIdAsync(request.UserId);
            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var user = userResult.Value;

            if(await _userService.CheckPasswordAsync(user, request.Password))
            {
                var result = await _userService.ChangeEmailAsync(user, request.NewEmail, request.Token);
                if (result.IsFailure)
                {
                    return result.Error;
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
