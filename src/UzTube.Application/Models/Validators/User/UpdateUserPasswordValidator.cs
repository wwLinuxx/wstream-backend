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
                .WithMessage($"Old Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.")
            .Matches(@"[A-Z]")
                .WithMessage($"Old Password should have minimum {UserValidatorConfiguration.MinimumPasswordBigLetterLength} big characters.")
            .Matches(@"[a-z]")
                .WithMessage($"Old Password should have minimum {UserValidatorConfiguration.MinimumPasswordSmallLetterLength} small characters.")
            .Matches(@"\d")
                .WithMessage($"Old Password should have minimum {UserValidatorConfiguration.MinimumPasswordNumberLength} number characters.");

        RuleFor(u => u.NewPassword)
            .MinimumLength(UserValidatorConfiguration.MinimumPasswordLength)
                .WithMessage($"New Password should have minimum {UserValidatorConfiguration.MinimumPasswordLength} characters.")
            .MaximumLength(UserValidatorConfiguration.MaximumPasswordLength)
                .WithMessage($"New Password should have maximum {UserValidatorConfiguration.MaximumPasswordLength} characters.")
            .Matches(@"[A-Z]")
                .WithMessage($"New Password should have minimum {UserValidatorConfiguration.MinimumPasswordBigLetterLength} big characters.")
            .Matches(@"[a-z]")
                .WithMessage($"New Password should have minimum {UserValidatorConfiguration.MinimumPasswordSmallLetterLength} small characters.")
            .Matches(@"\d")
                .WithMessage($"New Password should have minimum {UserValidatorConfiguration.MinimumPasswordNumberLength} number characters.");
    }
}