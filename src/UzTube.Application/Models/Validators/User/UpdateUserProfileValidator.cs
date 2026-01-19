using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(UserValidatorConfiguration.MinimumUsernameLength)
                .WithMessage($"Username should have minimum {UserValidatorConfiguration.MinimumUsernameLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumUsernameLength)
                .WithMessage($"Username should have maximum {UserValidatorConfiguration.MaximumUsernameLength} characters.")
            .Matches(UserValidatorConfiguration.UsernameRegexPattern)
                .WithMessage("Username can only contain letters, numbers, and underscores.");
    }
}