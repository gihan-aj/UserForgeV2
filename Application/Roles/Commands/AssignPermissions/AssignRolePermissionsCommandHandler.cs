using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Roles;
using SharedKernal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.AssignPermissions
{
    internal sealed class AssignRolePermissionsCommandHandler : ICommandHandler<AssignRolePermissionsCommand>
    {
        private readonly IRoleManagementService _roleManagementService;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRolePermissionsCommandHandler(IPermissionsRepository permissionsRepository, IUnitOfWork unitOfWork, IRoleManagementService roleManagementService)
        {
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
            _roleManagementService = roleManagementService;
        }

        public async Task<Result> Handle(AssignRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManagementService.GetRoleWithRolePermissionsAsync(request.RoleId);
            if (role is null)
            {
                return RoleErrors.NotFound.Role(request.RoleId);
            }

            var result = await _permissionsRepository.AssignRolePermissionsAsync(
                role, 
                request.PermissionIds, 
                request.ModifiedBy, 
                cancellationToken);

            return result;
        }
    }
}
