using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Roles;
using SharedKernal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Roles.Commands.Create
{
    internal sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, string>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(IRolesRepository rolesRepository, IUnitOfWork unitOfWork)
        {
            _rolesRepository = rolesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (await _rolesRepository.RoleNameExists(request.RoleName, request.AppId))
            {
                return Result.Failure<string>(RoleErrors.Conflict.RoleNameAlreadyExists(request.RoleName));
            }

            var role = new Role(
                request.RoleName,
                string.IsNullOrWhiteSpace(request.Description) 
                ? null 
                : request.Description,
                request.AppId,
                request.UserId);

            role.NormalizedName = _rolesRepository.NormalizeRoleName(request.RoleName);

            _rolesRepository.Add(role);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(role.Id);
        }
    }
}
