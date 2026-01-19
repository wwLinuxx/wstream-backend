using FluentValidation;
using UzTube.Application.Models.Post;

namespace UzTube.Application.Models.Validators.Post;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostValidator()
    {
        RuleFor(p => p.Title)
            .MinimumLength(PostValidatorConfiguration.MinimumTitleLength)
            .WithMessage($"Title should have minimum {PostValidatorConfiguration.MinimumTitleLength} characters.")
            .MaximumLength(PostValidatorConfiguration.MaximumTitleLength)
            .WithMessage($"Title should have maximum {PostValidatorConfiguration.MaximumTitleLength} characters.");

        RuleFor(p => p.Description)
            .MaximumLength(PostValidatorConfiguration.MaximumDescriptionLength)
            .WithMessage($"Description should have maximum {PostValidatorConfiguration.MaximumDescriptionLength} characters.");

        RuleFor(p => p.IsPrivate)
            .NotNull().WithMessage("IsPrivate is required.");
    }
}