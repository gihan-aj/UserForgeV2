using FluentValidation;

namespace Application.Roles.Commands.AssignPermissions
{
    public class AssignRolePermissionsCommandValidator : AbstractValidator<AssignRolePermissionsCommand>
    {
        public AssignRolePermissionsCommandValidator() 
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role id is required.");

            RuleFor(x => x.ModifiedBy).NotEmpty().WithMessage("Modified user id is required.");
        }
    }
}
