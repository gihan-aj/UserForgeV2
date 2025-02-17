using FluentValidation;

namespace Application.Apps.Commands.Activate
{
    public class ActivateAppsCommandValidator : AbstractValidator<ActivateAppsCommand>
    {
        public ActivateAppsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("App Ids are required.")
                .Must(x => x.Count > 0).WithMessage("App Ids must be selected to activate.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("User id is required.");
        }
    }
}
