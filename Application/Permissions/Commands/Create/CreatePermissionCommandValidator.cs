using FluentValidation;

namespace Application.Permissions.Commands.Create
{
    internal class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(450).WithMessage("Description must not exceed 450 characters.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("CreatedBy is required.")
                .MaximumLength(256).WithMessage("CreatedBy must not exceed 256 characters.");
        }
    }
}
