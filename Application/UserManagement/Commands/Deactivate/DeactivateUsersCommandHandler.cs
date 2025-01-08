using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserManagement.Commands.Deactivate
{
    internal sealed class DeactivateUsersCommandHandler : ICommandHandler<DeactivateUsersCommand, List<string>>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateUsersCommandHandler(
            IUserManagementService userManagementService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _userManagementService = userManagementService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<string>>> Handle(DeactivateUsersCommand request, CancellationToken cancellationToken)
        {
            var result = await _userManagementService.DeactivateUsersAsync(request.Ids, request.UserId, cancellationToken);
            if(result.IsSuccess)
            {
                var deactivatedIds = result.Value;
                if(deactivatedIds.Count > 0)
                {
                    foreach (var id in deactivatedIds)
                    {
                        await _refreshTokenRepository.LogoutFromAllDevicesAsync(id, cancellationToken);
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            return result;
        }
    }
}
