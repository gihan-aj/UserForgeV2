using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return _userService.ConfirmEmailAsync(request.UserId, request.Token);
        }
    }
}
