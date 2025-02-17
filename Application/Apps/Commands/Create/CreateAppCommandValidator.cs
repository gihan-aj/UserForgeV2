using FluentValidation;

namespace Application.Apps.Commands.Create
{
    public class CreateAppCommandValidator : AbstractValidator<CreateAppCommand>
    {
        public CreateAppCommandValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("App name is required.")
                .MaximumLength(256).WithMessage("App name cannot exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(450).WithMessage("Description cannot exceed 450 characters.");

            RuleFor(x => x.Createdby)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
