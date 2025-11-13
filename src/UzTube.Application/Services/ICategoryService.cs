using UzTube.Application.Models;
using UzTube.Application.Models.Category;

namespace UzTube.Application.Services;

public interface ICategoryService
{
    Task<CreateCategoryResponseModel> CreateCategory(CreateCategoryRequest request);
    Task<CategoryResponseModel> GetCategory(Guid id);
    Task<PaginatedList<CategoryResponseModel>> GetCategories(PageOption option);
    Task<UpdateCategoryResponseModel> UpdateCategory(Guid id, UpdateCategoryRequest request);
    Task<DeleteCategoryResponseModel> DeleteCategory(Guid id);
}