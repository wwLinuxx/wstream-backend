using FluentValidation;
using UzTube.Application.Models.LiveStream;
using UzTube.DataAccess.Persistence;

namespace UzTube.Application.Models.Validators.Stream;

public class CreateStreamValidator : AbstractValidator<CreateStreamRequest>
{
    private readonly DatabaseContext _db;

    public CreateStreamValidator(DatabaseContext db)
    {
        _db = db;

        RuleFor(p => p.Title)
           .MinimumLength(StreamValidatorConfiguration.MinimumTitleLength)
           .WithMessage($"Title should have minimum {StreamValidatorConfiguration.MinimumTitleLength} characters.")
           .MaximumLength(StreamValidatorConfiguration.MaximumTitleLength)
           .WithMessage($"Title should have maximum {StreamValidatorConfiguration.MaximumTitleLength} characters.");

        RuleFor(p => p.Description)
            .MaximumLength(StreamValidatorConfiguration.MaximumDescriptionLength)
            .WithMessage($"Description should have maximum {StreamValidatorConfiguration.MaximumDescriptionLength} characters.");

        RuleFor(p => p.IsPrivate)
            .NotNull().WithMessage("IsPrivate is required.");
    }
}
