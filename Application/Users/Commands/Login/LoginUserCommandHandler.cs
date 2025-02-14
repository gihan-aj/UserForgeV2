﻿using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Settings;
using Microsoft.Extensions.Options;
using SharedKernal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Login
{
    internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenSettings _tokenSettings;


        public LoginUserCommandHandler(
            IUserService userService,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IOptions<TokenSettings> tokenSettings)
        {
            _userService = userService;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var hashedDeviceIdentifier = _tokenService.Hash(request.DeviceIdentifier);

            var loginResult = await _userService.LoginAsync(request.Email, request.Password, hashedDeviceIdentifier);
            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginUserResponse>(loginResult.Error);
            }

            var user = loginResult.Value;

            string?[] roles = user.UserRoles
                .Select(ur => ur.Role)
                .Select(r => r.Name)
                .ToArray();

            var accessToken = _tokenService.CreateJwtToken(user, roles!);

            string refreshToken = _tokenService.GenerateRefreshToken();  

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(_tokenSettings.RefreshToken.ExpiresInDays);
         
            var existingToken = user.RefreshTokens.Any() ? user.RefreshTokens.ToArray()[0] : null;

            if (existingToken is null)
            {
                _refreshTokenRepository.Add(
                    refreshToken,
                    refreshTokenExpiryDate,
                    user,
                    hashedDeviceIdentifier);
            }
            else
            {
                var tokenRenewResult = await _refreshTokenRepository.RenewAsync(existingToken.Id, refreshToken, refreshTokenExpiryDate);
                if (tokenRenewResult.IsFailure)
                {
                    return Result.Failure<LoginUserResponse>(tokenRenewResult.Error);
                }
            } 

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var loginResponse = new LoginUserResponse(user, accessToken, refreshToken);

            return Result.Success(loginResponse);
        }
    }
}
