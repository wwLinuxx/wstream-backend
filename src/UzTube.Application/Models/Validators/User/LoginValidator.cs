using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class LoginValidator : AbstractValidator<LoginUserRequest>
{
    public LoginValidator()
    {
        RuleFor(u => u.Password)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
                .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
                .WithMessage($"Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.")
            .Matches(@"[A-Z]")
                .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordBigLetterLength} big characters.")
            .Matches(@"[a-z]")
                .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordSmallLetterLength} small characters.")
            .Matches(@"\d")
                .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordNumberLength} number characters.")
            .Matches(@"[^\w\d\s]")
                .WithMessage($"Password should have minimum {UserValidatorConfiguration.MinimumPasswordCharacterLength} characters.");
    }
}