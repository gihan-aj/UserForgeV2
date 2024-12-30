using FluentValidation;

namespace Application.Users.Commands.SendEmailChange
{
    internal class SendEmailChangeCommandValidator : AbstractValidator<SendEmailChangeCommand>
    {
        public SendEmailChangeCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required.");

            RuleFor(x => x.NewEmail)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("New Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
