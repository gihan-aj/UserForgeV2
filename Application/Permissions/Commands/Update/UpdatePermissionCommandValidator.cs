using FluentValidation;

namespace Application.Permissions.Commands.Update
{
    internal class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(450).WithMessage("Description must not exceed 450 characters.");
        }
    }
}
