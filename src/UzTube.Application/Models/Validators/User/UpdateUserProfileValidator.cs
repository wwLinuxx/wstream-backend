using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(3)
                .WithMessage($"Username should have minimum {UserValidatorConfiguration.MinimumUsernameLength} characters.")
            .MaximumLength(20)
                .WithMessage($"Username should have maximum {UserValidatorConfiguration.MaximumUsernameLength} characters.")
            .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("Username should have letter, number and _");
    }
}