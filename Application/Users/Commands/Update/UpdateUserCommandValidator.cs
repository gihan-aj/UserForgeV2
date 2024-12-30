using FluentValidation;
using System.Text.RegularExpressions;
using System;

namespace Application.Users.Commands.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(256).WithMessage("First Name cannot exceed 256 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("First Name can only contain alphabetic characters.");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(256).WithMessage("Last Name cannot exceed 256 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last Name can only contain alphabetic characters.");

            RuleFor(x => x.PhoneNumber)
               .Cascade(CascadeMode.Stop)
                .Must(BeAValidPhoneNumber).When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Please provide a valid phone number.");

            RuleFor(x => x.DateOfBirth)
                .Must(BeAValidAge).When(x => x.DateOfBirth.HasValue)
                .WithMessage("You must be at least 16 years old.")
                .When(x => x.DateOfBirth.HasValue);
        }

        private bool BeAValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            return Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$"); // E.164 format
        }

        private bool BeAValidAge(DateOnly? dateOfBirth)
        {
            if (!dateOfBirth.HasValue) return false; // No date provided, not valid.

            // Calculate the age based on today's date
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dateOfBirth.Value.Year;

            // Adjust if the birthday hasn't occurred yet this year
            if (today < dateOfBirth.Value.AddYears(age))
            {
                age--;
            }

            return age >= 16; // Validate the age threshold
        }
    }
}
