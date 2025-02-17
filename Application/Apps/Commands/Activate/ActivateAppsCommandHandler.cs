using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Apps;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Apps.Commands.Activate
{
    internal sealed class ActivateAppsCommandHandler : ICommandHandler<ActivateAppsCommand, List<string>>
    {
        private readonly IAppManagementService _appManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateAppsCommandHandler(IUnitOfWork unitOfWork, IAppManagementService appManagementService)
        {
            _unitOfWork = unitOfWork;
            _appManagementService = appManagementService;
        }

        public async Task<Result<List<string>>> Handle(ActivateAppsCommand request, CancellationToken cancellationToken)
        {
            var appsToActivate = await _appManagementService.GetAppsToActivateAsync(request.Ids, cancellationToken);

            if(appsToActivate is null || appsToActivate.Count == 0)
            {
                return Result.Failure<List<string>>(AppErrors.NotFound.AppsNotFoundToActivate);
            }

            foreach(var app in appsToActivate)
            {
                app.Activate(request.ModifiedBy);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return appsToActivate.Select(a => a.Name).ToList();
        }
    }
}
