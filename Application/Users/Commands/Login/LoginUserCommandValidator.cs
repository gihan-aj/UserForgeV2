using FluentValidation;

namespace Application.Users.Commands.Login
{
    public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.DeviceInfo)
                .NotEmpty().WithMessage("Device information is required.");
        }
    }
}
