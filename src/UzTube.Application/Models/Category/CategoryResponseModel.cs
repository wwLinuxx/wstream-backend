using UzTube.Application.Models.Translate;

namespace UzTube.Application.Models.Category;

public record CategoryResponseModel : BaseResponseModel
{
    public string Name { get; init; } = null!;
    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public IReadOnlyCollection<BaseTranslateModel> Translates { get; init; } = [];
}
