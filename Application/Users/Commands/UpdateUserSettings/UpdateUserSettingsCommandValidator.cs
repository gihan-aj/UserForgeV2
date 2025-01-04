using FluentValidation;

namespace Application.Users.Commands.UpdateUserSettings
{
    public class UpdateUserSettingsCommandValidator : AbstractValidator<UpdateUserSettingsCommand>
    {
        public UpdateUserSettingsCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id is required.");
            RuleFor(x => x.Theme).NotEmpty().WithMessage("Theme is required.");
            RuleFor(x => x.Language).NotEmpty().WithMessage("Language is required.");
            RuleFor(x => x.DateFormat).NotEmpty().WithMessage("Date format is required.");
            RuleFor(x => x.TimeFormat).NotEmpty().WithMessage("Time format is required.");
            RuleFor(x => x.TimeZone).NotEmpty().WithMessage("Time zone is required.");
        }
    }
}
