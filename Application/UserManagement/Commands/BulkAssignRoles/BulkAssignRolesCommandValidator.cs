using FluentValidation;

namespace Application.UserManagement.Commands.BulkAssignRoles
{
    public class BulkAssignRolesCommandValidator : AbstractValidator<BulkAssignRolesCommand>
    {
        public BulkAssignRolesCommandValidator()
        {
            RuleFor(x => x.UserIds)
                .NotEmpty().WithMessage("Atleast one user id is required.")
                .Must(ids => ids.Length > 0).WithMessage("Atleast one user id is required.");

            RuleFor(x => x.RoleNames)
                .NotEmpty().WithMessage("Atleast one role should be asssigned.")
                .Must(ids => ids.Length > 0).WithMessage("Atleast one role should be asssigned.");

            RuleFor(x => x.AssignedBy)
                .NotEmpty().WithMessage("Assigned user id is required.");
        }
    }
}
