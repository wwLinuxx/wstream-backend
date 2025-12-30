using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordRequest>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(u => u.OldPassword)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
                .WithMessage($"Old Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
                .WithMessage($"Old Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");

        RuleFor(u => u.NewPassword)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
                .WithMessage($"New Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
                .WithMessage($"New Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");
    }
}