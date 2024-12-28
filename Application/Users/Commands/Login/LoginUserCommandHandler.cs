using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Data;
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

            var loginResponse = new LoginUserResponse(user, roles, accessToken, refreshToken);

            _refreshTokenRepository.Add(
                refreshToken,
                DateTime.UtcNow.AddDays(_tokenSettings.RefreshToken.ExpiresInDays),
                user,
                request.DeviceInfo);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(loginResponse);
        }
    }
}
