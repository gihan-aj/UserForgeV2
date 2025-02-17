using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Apps;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Apps.Commands.Create
{
    internal sealed class CreateAppCommandHandler : ICommandHandler<CreateAppCommand, int>
    {
        private readonly IAppsRepository _appsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAppCommandHandler(IAppsRepository appsRepository, IUnitOfWork unitOfWork)
        {
            _appsRepository = appsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateAppCommand request, CancellationToken cancellationToken)
        {
            if(await _appsRepository.AppNameExists(request.Name))
            {
                return Result.Failure<int>(AppErrors.Conflict.AppNameAlreadyExists(request.Name));
            }

            var app = App.Create(
                request.Name,
                string.IsNullOrWhiteSpace(request.Description) ? null : request.Description,
                string.IsNullOrWhiteSpace(request.BaseUrl) ? null : request.BaseUrl,
                request.Createdby);

            _appsRepository.Add(app);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(app.Id);
        }
    }
}
