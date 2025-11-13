using UzTube.Application.Models.Translate;

namespace UzTube.Application.Models.Category;

public record CreateCategoryRequest(
    string Name,
    IReadOnlyCollection<BaseTranslateModel> Translates);

public record CreateCategoryResponseModel : BaseResponseModel;