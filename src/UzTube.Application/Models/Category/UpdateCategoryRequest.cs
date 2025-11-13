using UzTube.Application.Models.Translate;

namespace UzTube.Application.Models.Category;

public record UpdateCategoryRequest(
    string Name,
    IReadOnlyCollection<BaseTranslateModel> Translates);

public record UpdateCategoryResponseModel : BaseResponseModel;