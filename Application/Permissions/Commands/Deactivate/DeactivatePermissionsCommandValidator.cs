using FluentValidation;

namespace Application.Permissions.Commands.Deactivate
{
    public class DeactivatePermissionsCommandValidator : AbstractValidator<DeactivatePermissionsCommand>
    {
        public DeactivatePermissionsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .Must(x => x.Count > 0).WithMessage("Permission Ids are required.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("Modifiying user id is required.");
        }
    }
}
