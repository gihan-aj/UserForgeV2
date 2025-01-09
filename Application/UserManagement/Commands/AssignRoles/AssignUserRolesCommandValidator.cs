using FluentValidation;

namespace Application.UserManagement.Commands.AssignRoles
{
    public class AssignUserRolesCommandValidator : AbstractValidator<AssignUserRolesCommand>
    {
        public AssignUserRolesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
}
