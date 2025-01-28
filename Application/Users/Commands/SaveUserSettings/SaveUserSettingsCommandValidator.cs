using FluentValidation;

namespace Application.Users.Commands.SaveUserSettings
{
    public sealed class SaveUserSettingsCommandValidator : AbstractValidator<SaveUserSettingsCommand>
    {
        public SaveUserSettingsCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required.");

            RuleForEach(x => x.UserSettings)
                .SetValidator(new UserSettingValidator());
        }
    }

    public sealed class UserSettingValidator : AbstractValidator<SaveUserSettingsRequest>
    {
        public UserSettingValidator()
        {
            RuleFor(x => x.Key)
           .NotEmpty()
           .WithMessage("Key is required.");

            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("Value is required.");

            RuleFor(x => x.DataType)
                .NotEmpty()
                .WithMessage("DataType is required.")
                .When(x => !string.IsNullOrEmpty(x.DataType)); // Optional field but requires validation if provided
        }
    }
}
