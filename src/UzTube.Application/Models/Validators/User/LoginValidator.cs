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
                .WithMessage($"Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");
    }
}