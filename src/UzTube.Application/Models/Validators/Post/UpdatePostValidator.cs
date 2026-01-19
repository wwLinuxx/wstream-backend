using FluentValidation;
using UzTube.Application.Models.Post;

namespace UzTube.Application.Models.Validators.Post;

public class UpdatePostValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostValidator()
    {
        RuleFor(p => p.Title)
            .MinimumLength(PostValidatorConfiguration.MaximumTitleLength)
            .WithMessage($"Title should have minimum {PostValidatorConfiguration.MaximumTitleLength} characters.")
            .MaximumLength(PostValidatorConfiguration.MaximumTitleLength)
            .WithMessage($"Title should have maximum {PostValidatorConfiguration.MaximumTitleLength} characters.");

        RuleFor(p => p.Description)
            .MaximumLength(PostValidatorConfiguration.MaximumDescriptionLength)
            .WithMessage($"Description should have maximum {PostValidatorConfiguration.MaximumDescriptionLength} characters.");

        RuleFor(p => p.IsPrivate)
            .NotNull().WithMessage("IsPrivate is required.");
    }
}