using FluentValidation;

namespace Application.Roles.Commands.Update
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(256).WithMessage("Role name cannot exceed 256 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Role name can only contain alphabetic characters.");

            RuleFor(x => x.Description)
                .MaximumLength(450).WithMessage("Description cannot exceed 450 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
