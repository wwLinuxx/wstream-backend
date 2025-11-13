using UzTube.Core.Enums;

namespace UzTube.Core.Common;

public abstract class BaseTranslateEntity<TOwnerEntity> : BaseEntity
    where TOwnerEntity : BaseEntity
{
    public Guid OwnerId { get; set; }
    public SystemLanguages LanguageId { get; set; } = SystemLanguages.Uzbek;
    public string ColumnName { get; set; } = null!;
    public string TranslateText { get; set; } = null!;

    public TOwnerEntity Owner { get; set; } = null!;
}