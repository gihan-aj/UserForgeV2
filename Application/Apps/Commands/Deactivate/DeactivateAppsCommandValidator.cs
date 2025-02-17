using FluentValidation;

namespace Application.Apps.Commands.Deactivate
{
    public class DeactivateAppsCommandValidator : AbstractValidator<DeactivateAppsCommand>
    {
        public DeactivateAppsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("App Ids are required.")
                .Must(x => x.Count > 0).WithMessage("App Ids must be selected to deactivate.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("User id is required.");
        }
    }
}
