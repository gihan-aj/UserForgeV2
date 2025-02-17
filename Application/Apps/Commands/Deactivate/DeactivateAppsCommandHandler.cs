using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Apps;
using SharedKernal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Apps.Commands.Deactivate
{
    internal sealed class DeactivateAppsCommandHandler : ICommandHandler<DeactivateAppsCommand, List<string>>
    {
        private readonly IAppManagementService _appManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateAppsCommandHandler(IUnitOfWork unitOfWork, IAppManagementService appManagementService)
        {
            _unitOfWork = unitOfWork;
            _appManagementService = appManagementService;
        }

        public async Task<Result<List<string>>> Handle(DeactivateAppsCommand request, CancellationToken cancellationToken)
        {
            var appsToDeactivate = await _appManagementService.GetAppsToDeactivateAsync(request.Ids, cancellationToken);

            if (appsToDeactivate is null || appsToDeactivate.Count == 0)
            {
                return Result.Failure<List<string>>(AppErrors.NotFound.AppsNotFoundToDeactivate);
            }

            foreach (var app in appsToDeactivate)
            {
                app.Deactivate(request.ModifiedBy);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return appsToDeactivate.Select(a => a.Name).ToList();
        }
    }
}
