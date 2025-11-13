using FluentValidation;
using UzTube.Application.Models.User;

namespace UzTube.Application.Models.Validators.User;

public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordRequest>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(u => u.OldPassword)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
            .WithMessage($"Old password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
            .WithMessage($"Old password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");
        
        RuleFor(u => u.NewPassword)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
            .WithMessage($"New Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
            .WithMessage($"New password  should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");
        
        RuleFor(u => u.ConfirmPassword)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
            .WithMessage($"Confirm password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
            .WithMessage($"Confirm password  should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.");
    }
}