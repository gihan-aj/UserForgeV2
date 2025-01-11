using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Permissions;
using SharedKernal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Permissions.Commands.Delete
{
    internal sealed class DeletePermissionsCommandHandler : ICommandHandler<DeletePermissionsCommand, List<string>>
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePermissionsCommandHandler(IPermissionsRepository permissionsRepository, IUnitOfWork unitOfWork)
        {
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<string>>> Handle(DeletePermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissions = new List<Permission>();

            if (request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    var permission = await _permissionsRepository.GetByIdAsync(id);
                    if (permission is not null)
                    {
                        permissions.Add(permission);
                    }
                }

                var deletedPermissionIds = new List<string>();
                if (permissions.Count > 0)
                {
                    foreach (var permission in permissions)
                    {
                        permission.DeletedBy = request.ModifiedBy;
                        _permissionsRepository.Remove(permission);
                        deletedPermissionIds.Add(permission.Id);
                    }

                    await _unitOfWork.SaveChangesAsync();

                    return deletedPermissionIds;
                }
            }

            if (request.Ids.Count == 1)
            {
                return Result.Failure<List<string>>(PermissionErrors.NotFound.PermissionNotFound(request.Ids[0]));
            }

            return Result.Failure<List<string>>(PermissionErrors.NotFound.PermissionsNotFound);
        }
    }
}
