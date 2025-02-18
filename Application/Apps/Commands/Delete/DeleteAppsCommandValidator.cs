using FluentValidation;

namespace Application.Apps.Commands.Delete
{
    public class DeleteAppsCommandValidator : AbstractValidator<DeleteAppsCommand>
    {
        public DeleteAppsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("App Ids are required.")
                .Must(x => x.Count > 0).WithMessage("App Ids must be selected to delete.");

            RuleFor(x => x.DeletedBy)
                .NotEmpty().WithMessage("User id is required.");
        }
    }
}
