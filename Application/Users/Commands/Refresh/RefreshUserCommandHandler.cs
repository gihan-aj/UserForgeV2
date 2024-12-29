using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Settings;
using Domain.Users;
using Microsoft.Extensions.Options;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Refresh
{
    internal sealed class RefreshUserCommandHandler : ICommandHandler<RefreshUserCommand, RefreshUserResponse>
    {
        private readonly IRefreshTokenRepository _refreshRepository;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly TokenSettings _tokenSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshUserCommandHandler(
            IRefreshTokenRepository refreshRepository,
            IUserService userService,
            ITokenService tokenService,
            IOptions<TokenSettings> tokenSettings,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _refreshRepository = refreshRepository;
            _userService = userService;
            _tokenService = tokenService;
            _tokenSettings = tokenSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<RefreshUserResponse>> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
        {
            var hashedDeviceIdentifier = _tokenService.Hash(request.DeviceIdentifier);

            var existingToken = await _refreshRepository.GetAsync(request.RefreshToken, hashedDeviceIdentifier);
            if(existingToken is null)
            {
                return Result.Failure<RefreshUserResponse>(UserErrors.Token.MissingRefreshToken);
            }

            if(existingToken.IsExpired || existingToken.IsRevoked)
            {
                return Result.Failure<RefreshUserResponse>(UserErrors.Token.InvalidRefreshToken);
            }

            var userResult = await _userService.GetByIdAsync(existingToken.UserId);
            if (userResult.IsFailure)
            {
                return Result.Failure<RefreshUserResponse>(userResult.Error);
            }

            var user = userResult.Value;

            var rolesResult = await _userService.GetRolesAsync(user);
            var roles = rolesResult.Value;

            var accessToken = _tokenService.CreateJwtToken(user, roles);

            string refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(_tokenSettings.RefreshToken.ExpiresInDays);

            var tokenRenewResult = await _refreshTokenRepository.RenewAsync(existingToken.Id, refreshToken, refreshTokenExpiryDate);
            if (tokenRenewResult.IsFailure)
            {
                return Result.Failure<RefreshUserResponse>(tokenRenewResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var refreshUserResponse = new RefreshUserResponse(accessToken, refreshToken);
            return Result.Success(refreshUserResponse);
        }
    }
}
