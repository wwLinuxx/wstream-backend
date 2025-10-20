using FluentValidation;
using UzTube.DataAccess.Persistence;
using UzTube.Models.DTO;

namespace UzTube.Application.Models.Validators.User;

public class LoginValidator : AbstractValidator<LoginUserModel>
{
    private readonly DatabaseContext _context;

    public LoginValidator(DatabaseContext context)
    {
        _context = context;

        RuleFor(u => u.Email)
            .MinimumLength(UserValidatorConfiguration.MinimumEmailLength)
            .WithMessage($"Email address should have minimum {UserValidatorConfiguration.MinimumEmailLength} characters")
            .MaximumLength(UserValidatorConfiguration.MaximumEmailLength)
            .WithMessage($"Email address should have maximum {UserValidatorConfiguration.MaximumEmailLength} characters")
            .EmailAddress()
            .WithMessage("Email address is not valid"); 

        RuleFor(u => u.Password)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
            .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
            .WithMessage($"Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters");
    }
}
