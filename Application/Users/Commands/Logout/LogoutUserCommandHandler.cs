using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Logout
{
    internal sealed class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutUserCommandHandler(IUserService userService, IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var hashedDeviceIdentifier = _tokenService.Hash(request.DeviceIdentifier);

            var result = await _refreshTokenRepository.RemoveAsync(request.UserId, hashedDeviceIdentifier);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
