using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Settings;
using Microsoft.Extensions.Options;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Login
{
    internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenSettings _tokenSettings;


        public LoginUserCommandHandler(
            IUserService userService,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IOptions<TokenSettings> tokenSettings,
            IUserSettingsRepository userSettingsRepository)
        {
            _userService = userService;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _tokenSettings = tokenSettings.Value;
            _userSettingsRepository = userSettingsRepository;
        }

        public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _userService.LoginAsync(request.Email, request.Password);
            if (loginResult.IsFailure)
            {
                return Result.Failure<LoginUserResponse>(loginResult.Error);
            }

            var user = loginResult.Value;

            var rolesResult = await _userService.GetRolesAsync(user);
            var roles = rolesResult.Value;

            var accessToken = _tokenService.CreateJwtToken(user, roles);

            string refreshToken = _tokenService.GenerateRefreshToken();  

            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(_tokenSettings.RefreshToken.ExpiresInDays);

            var hashedDeviceIdentifier = _tokenService.Hash(request.DeviceIdentifier);

            var existingToken = await _refreshTokenRepository.GetByUserIdAndDeviceAsync(user.Id, hashedDeviceIdentifier);

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

            var loginResponse = new LoginUserResponse(user, roles, accessToken, refreshToken);

            var settings = await _userSettingsRepository.GetByUserIdAsync(user.Id);
            if(settings is not null)
            {
                loginResponse.UserSettings = new BasicUserSettings(
                    settings.Theme,
                    settings.Language,
                    settings.DateFormat,
                    settings.TimeFormat,
                    settings.TimeZone);
            }

            return Result.Success(loginResponse);
        }
    }
}
