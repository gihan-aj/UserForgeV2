using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permissions.Commands.Create
{
    internal sealed class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, string>
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePermissionCommandHandler(IPermissionsRepository permissionsRepository, IUnitOfWork unitOfWork)
        {
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            if(await _permissionsRepository.ExistsAsync(request.Name))
            {
                return Result.Failure<string>(PermissionErrors.Confilct.PermissionNameAlreadyExists(request.Name));
            }

            var permission = Permission.Create(request.Name, request.Description, request.CreatedBy);
            _permissionsRepository.Add(permission);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return permission.Id;
        }
    }
}
