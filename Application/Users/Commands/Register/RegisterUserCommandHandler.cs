using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Register
{
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, User>
    {
        private readonly IUsersService _usersService;

        public RegisterUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Result<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUserResult = await _usersService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth,
                request.Password);

            return newUserResult;       
        }
    }
}
