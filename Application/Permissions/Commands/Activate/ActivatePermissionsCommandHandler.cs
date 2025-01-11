using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using SharedKernal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permissions.Commands.Activate
{
    internal sealed class ActivatePermissionsCommandHandler : ICommandHandler<ActivatePermissionsCommand, List<string>>
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivatePermissionsCommandHandler(IPermissionsRepository permissionsRepository, IUnitOfWork unitOfWork)
        {
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<string>>> Handle(ActivatePermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();

            if(request.Ids.Count > 0)
            {
                foreach(var id in request.Ids)
                {
                    var permission = await _permissionsRepository.GetByIdAsync(id);
                    if(permission is not null)
                    {
                        permissions.Add(permission);
                    }
                }

                var activatedPermissionIds = new List<string>();
                if(permissions.Count > 0)
                {
                    foreach(var permission in permissions)
                    {
                        if (!permission.IsActive)
                        {
                            permission.Activate(request.ModifiedBy);
                            activatedPermissionIds.Add(permission.Id);  
                        }
                    }

                    if(activatedPermissionIds.Count > 0)
                    {
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        return activatedPermissionIds;
                    }
                    else
                    {
                        return Result.Failure<List<string>>(PermissionErrors.General.OperationFailed("No permissions to activate"));
                    }
                }
            }

            if(request.Ids.Count  == 1)
            {
                return Result.Failure<List<string>>(PermissionErrors.NotFound.PermissionNotFound(request.Ids[0]));
            }

            return Result.Failure<List<string>>(PermissionErrors.NotFound.PermissionsNotFound);

        }
    }
}
