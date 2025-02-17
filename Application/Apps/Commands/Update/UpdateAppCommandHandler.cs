using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Apps;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Apps.Commands.Update
{
    internal sealed class UpdateAppCommandHandler : ICommandHandler<UpdateAppCommand>
    {
        private readonly IAppsRepository _appsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAppCommandHandler(IAppsRepository appsRepository, IUnitOfWork unitOfWork)
        {
            _appsRepository = appsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAppCommand request, CancellationToken cancellationToken)
        {
            var app = await _appsRepository.GetByIdAsync(request.Id);
            if(app is null)
            {
                return AppErrors.NotFound.AppNotFound(request.Id);
            }

            if(await _appsRepository.AppNameExists(request.Name))
            {
                return AppErrors.Conflict.AppNameAlreadyExists(request.Name);
            }

            app.Update(
                request.Name,
                string.IsNullOrWhiteSpace(request.Description) ? null : request.Description,
                string.IsNullOrWhiteSpace(request.BaseUrl) ? null : request.BaseUrl,
                request.ModifiedBy);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
