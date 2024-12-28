using FluentValidation;

namespace Application.Users.Commands.Refresh
{
    public sealed class RefreshUserCommandValidator : AbstractValidator<RefreshUserCommand>
    {
        public RefreshUserCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
            RuleFor(x => x.DeviceInfo).NotEmpty().WithMessage("Device information is required.");
        }
    }
}
