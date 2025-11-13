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
            .MinimumLength(PostValidatorConfiguration.MinimumDescriptionLength)
            .WithMessage(
                $"Description should have minimum {PostValidatorConfiguration.MinimumDescriptionLength} characters.")
            .MaximumLength(PostValidatorConfiguration.MaximumDescriptionLength)
            .WithMessage(
                $"Description should have maximum {PostValidatorConfiguration.MaximumDescriptionLength} characters.");

        RuleFor(p => p.ThumbnailUrl)
            .NotEmpty().WithMessage("ThumbnailFileUrl is required.");

        RuleFor(p => p.IsPrivate)
            .NotEmpty().WithMessage("IsPrivate is required.");
    }
}