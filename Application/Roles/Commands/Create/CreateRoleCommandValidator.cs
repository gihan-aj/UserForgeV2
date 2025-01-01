using FluentValidation;

namespace Application.Roles.Commands.Create
{
    internal class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(256).WithMessage("Role name cannot exceed 256 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Role name can only contain alphabetic characters.");
        }
    }
}
