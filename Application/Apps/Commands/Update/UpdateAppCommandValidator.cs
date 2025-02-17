using FluentValidation;

namespace Application.Apps.Commands.Update
{
    public class UpdateAppCommandValidator : AbstractValidator<UpdateAppCommand>
    {
        public UpdateAppCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("App Id is required.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("App name is required.")
                .MaximumLength(256).WithMessage("App name cannot exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(450).WithMessage("Description cannot exceed 450 characters.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
