using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Apps;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Apps.Commands.Delete
{
    internal sealed class DeleteAppsCommandHandler : ICommandHandler<DeleteAppsCommand, List<string>>
    {
        private readonly IAppsRepository _appsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAppsCommandHandler(IAppsRepository appsRepository, IUnitOfWork unitOfWork)
        {
            _appsRepository = appsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<string>>> Handle(DeleteAppsCommand request, CancellationToken cancellationToken)
        {
            var appsToDelete = new List<App>();
            foreach(var id in request.Ids)
            {
                var app = await _appsRepository.GetByIdAsync(id);
                if(app is not null)
                {
                    if (ProtectedApps.Names.Contains(app.Name))
                    {
                        return Result.Failure<List<string>>(AppErrors.Conflict.ProtectedApp(app.Name));
                    }
                    appsToDelete.Add(app);
                }
            }

            if(appsToDelete.Count == 0)
            {
                return request.Ids.Count == 1
                    ? Result.Failure<List<string>>(AppErrors.NotFound.AppNotFound(request.Ids[0]))
                    : Result.Failure<List<string>>(AppErrors.NotFound.AppsNotFoundToDelete);
            }

            var deletedAppNames = new List<string>();
            foreach(var app in appsToDelete)
            {
                app.DeletedBy = request.DeletedBy;
                deletedAppNames.Add(app.Name);
                _appsRepository.Remove(app);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return deletedAppNames;
        }
    }
}
