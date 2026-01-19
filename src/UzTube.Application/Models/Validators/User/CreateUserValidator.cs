using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UzTube.Application.Models.User;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Models.Validators.User;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    private readonly DatabaseContext _db;

    public CreateUserValidator(DatabaseContext db)
    {
        _db = db;

        RuleFor(x => x.Username)
            .MinimumLength(UserValidatorConfiguration.MinimumUsernameLength)
                .WithMessage($"Username should have minimum {UserValidatorConfiguration.MinimumUsernameLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumUsernameLength)
                .WithMessage($"Username should have maximum {UserValidatorConfiguration.MaximumUsernameLength} characters.")
            .Must(UsernameAddressIsUnique)
                .WithMessage("Email address is already in use.")
            .Matches(UserValidatorConfiguration.UsernameRegexPattern)
                .WithMessage("Username can only contain letters, numbers, and underscores.");

        RuleFor(u => u.Email)
            .MinimumLength(UserValidatorConfiguration.MinimumEmailLength)
                .WithMessage($"Email should have minimum {UserValidatorConfiguration.MinimumEmailLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumEmailLength)
                .WithMessage($"Email should have maximum {UserValidatorConfiguration.MaximumEmailLength} characters.")
            .Must(EmailAddressIsUnique)
                .WithMessage("Email address is already in use.")
            .Matches(UserValidatorConfiguration.EmailAddressRegexPattern)
                .WithMessage("Email is not valid");

        RuleFor(u => u.Password)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
                .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
                .WithMessage($"Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");

        RuleFor(u => u.CountryId)
            .NotEmpty()
                .WithMessage("Country is not valid.")
            .Must(HasCountry)
                .WithMessage("Country not found.");
    }

    private bool UsernameAddressIsUnique(string username) =>
        _db.Users.Any(u => u.Username != username);

    private bool EmailAddressIsUnique(string email) =>
        _db.Users.Any(u => u.Email != email);

    private bool HasCountry(Guid countryId) =>
        _db.Categories.Any(u => u.Id != countryId);
}