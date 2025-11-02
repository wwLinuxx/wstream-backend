using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileModel>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(u => u.FirstName)
            .MinimumLength(UserValidatorConfiguration.MinimumFirstNameLength)
            .WithMessage($"FirstName should have minimum {UserValidatorConfiguration.MinimumFirstNameLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumFirstNameLength)
            .WithMessage($"FirstName should have maximum {UserValidatorConfiguration.MaximumFirstNameLength} characters.");

        RuleFor(u => u.LastName)
            .MinimumLength(UserValidatorConfiguration.MinimumLastNameLength)
            .WithMessage($"LastName should have minimum {UserValidatorConfiguration.MinimumLastNameLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumLastNameLength)
            .WithMessage($"LastName should have maximum {UserValidatorConfiguration.MaximumLastNameLength} characters.");

        RuleFor(u => u.PhoneNumber)
            .MinimumLength(UserValidatorConfiguration.MinimumPhoneNumberLength)
            .WithMessage($"PhoneNumber should have minimum {UserValidatorConfiguration.MinimumPhoneNumberLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPhoneNumberLength)
            .WithMessage($"PhoneNumber should have maximum {UserValidatorConfiguration.MaximumPhoneNumberLength} characters.");

        RuleFor(u => u.Age)
            .InclusiveBetween(UserValidatorConfiguration.MinimumAge, UserValidatorConfiguration.MaximumAge)
            .WithMessage($"Age must be between {UserValidatorConfiguration.MinimumAge} and {UserValidatorConfiguration.MaximumEmailLength} years old.");
    }
}