using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permissions.Commands.Update
{
    internal sealed class UpdatePermissionCommandHandler : ICommandHandler<UpdatePermissionCommand>
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePermissionCommandHandler(IPermissionsRepository permissionsRepository, IUnitOfWork unitOfWork)
        {
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            if(await _permissionsRepository.NameExistsAsync(request.Name))
            {
                return PermissionErrors.Confilct.PermissionNameAlreadyExists(request.Name);
            }

            var permission = await _permissionsRepository.GetByIdAsync(request.Id);
            if (permission is null)
            {
                return PermissionErrors.NotFound.PermissionNotFound(request.Id);
            }

            permission.Update(request.Name, request.Description, request.ModifiedBy);

            _permissionsRepository.Update(permission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
