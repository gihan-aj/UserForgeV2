using FluentValidation;

namespace Application.Permissions.Commands.Activate
{
    public class ActivatePermissionsCommandValidator : AbstractValidator<ActivatePermissionsCommand>
    {
        public ActivatePermissionsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .Must(x => x.Count > 0).WithMessage("Permission Ids are required.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("Modifiying user id is required.");
        }
    }
}
