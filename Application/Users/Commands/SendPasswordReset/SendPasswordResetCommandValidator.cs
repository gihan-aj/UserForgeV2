using FluentValidation;

namespace Application.Users.Commands.SendPasswordReset
{
    public class SendPasswordResetCommandValidator : AbstractValidator<SendPasswordResetCommand>
    {
        public SendPasswordResetCommandValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");
        }
    }
}
