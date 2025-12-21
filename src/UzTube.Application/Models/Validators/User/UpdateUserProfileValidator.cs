using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
                .WithMessage("Username is required.")
            .MinimumLength(UserValidatorConfiguration.MinimumUsernameLength)
                .WithMessage($"Username must be at least {UserValidatorConfiguration.MinimumUsernameLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumUsernameLength)
                .WithMessage($"Username must not exceed {UserValidatorConfiguration.MaximumUsernameLength} characters.")
            .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("Username can only contain letters, numbers, and underscores.");
    }
}