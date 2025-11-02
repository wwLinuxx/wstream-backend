using FluentValidation;
using UzTube.Application.Models.User;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Models.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserModel>
{
    private readonly DatabaseContext _context;

    public CreateUserValidator(DatabaseContext context)
    {
        _context = context;

        RuleFor(u => u.Email)
            .MinimumLength(UserValidatorConfiguration.MinimumEmailLength)
            .WithMessage($"Email should have minimum {UserValidatorConfiguration.MinimumEmailLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumEmailLength)
            .WithMessage($"Email should have maximum {UserValidatorConfiguration.MaximumEmailLength} characters.")
            .Must(EmailAddressIsUnique)
            .WithMessage("Email address is already in use.");

        RuleFor(u => u.Password)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
            .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
            .WithMessage($"Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");

        RuleFor(u => u.PhoneNumber)
            .MinimumLength(UserValidatorConfiguration.MinimumPhoneNumberLength)
            .WithMessage(
                $"Phone number should have minimum {UserValidatorConfiguration.MinimumPhoneNumberLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPhoneNumberLength)
            .WithMessage($"Phone number should have maximum {UserValidatorConfiguration.MaximumPhoneNumberLength} characters.")
            .Must(PhoneNumberIsUnique)
            .WithMessage("Phone number is already in user.");

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

        RuleFor(u => u.Age)
            .InclusiveBetween(UserValidatorConfiguration.MinimumAge, UserValidatorConfiguration.MaximumAge)
            .WithMessage($"Age must be between {UserValidatorConfiguration.MinimumAge} and {UserValidatorConfiguration.MaximumEmailLength} years old.");

        RuleFor(u => u.CountryId)
            .NotEmpty()
            .WithMessage("Country is not valid");
    }

    private bool EmailAddressIsUnique(string email)
    {
        return _context.Users.Any(u => u.Email != email);
    }

    private bool PhoneNumberIsUnique(string phoneNumber)
    {
        return _context.Users.Any(u => u.Profile.PhoneNumber != phoneNumber);
    }
}