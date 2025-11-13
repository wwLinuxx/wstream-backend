using UzTube.Core.Enums;

namespace UzTube.Application.Models.Translate;

public record BaseTranslateModel
{
    public Guid Id { get; init; }
    public SystemLanguages LanguageId { get; init; }
    public string ColumnName { get; init; } = null!;
    public string TranslateText { get; init; } = null!;
}