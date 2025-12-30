using Microsoft.EntityFrameworkCore;
using UzTube.Application.Exceptions;
using UzTube.Application.Models;
using UzTube.Application.Models.Category;
using UzTube.Application.Models.Translate;
using UzTube.Core.Entities;
using UzTube.Core.Enums;
using UzTube.DataAccess.Persistence;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class CategoryService(
    DatabaseContext context,
    IClaimService claimService
) : ICategoryService
{
    public async Task<CreateCategoryResponseModel> CreateCategory(CreateCategoryRequest request)
    {
        if (await context.Categories.AnyAsync(c => c.Name == request.Name))
            throw new BadRequestException("Category already exists.");

        Category newCategory = new Category
        {
            Name = request.Name,
            Translates = request.Translates.Select(t => new CategoryTranslate
            {
                LanguageId = t.LanguageId,
                ColumnName = t.ColumnName,
                TranslateText = t.TranslateText
            })
                .ToList()
        };

        await context.Categories.AddAsync(newCategory);
        await context.SaveChangesAsync();

        return new CreateCategoryResponseModel { Id = newCategory.Id };
    }

    public async Task<CategoryResponseModel> GetCategory(Guid id)
    {
        SystemLanguages userLanguage = claimService.GetUserLanguage();

        Category category = await context.Categories
                                .Include(c => c.Translates)
                                .FirstOrDefaultAsync(c => c.Id == id)
                            ?? throw new NotFoundException("Category not found.");

        CategoryResponseModel response = new CategoryResponseModel
        {
            Id = category.Id,
            Name = category.Name,
            CreatedOn = category.CreatedOn,
            UpdatedOn = category.UpdatedOn,
            Translates = category.Translates
                .Where(t => t.LanguageId == userLanguage)
                .Select(t => new BaseTranslateModel
                {
                    Id = t.Id,
                    LanguageId = t.LanguageId,
                    ColumnName = t.ColumnName,
                    TranslateText = t.TranslateText
                })
                .ToList()
        };

        return response;
    }

    public async Task<PaginatedList<CategoryResponseModel>> GetCategories(PageOption option)
    {
        SystemLanguages userLanguage = claimService.GetUserLanguage();

        IQueryable<Category> query = context.Categories;

        if (!string.IsNullOrWhiteSpace(option.Search))
            query = query.Where(c => c.Name.Contains(option.Search, StringComparison.OrdinalIgnoreCase));

        List<CategoryResponseModel> categories = await query
            .Skip(option.PageSize * (option.PageNumber - 1))
            .Take(option.PageSize)
            .Select(c => new CategoryResponseModel
            {
                Id = c.Id,
                Name = c.Name,
                CreatedOn = c.CreatedOn,
                UpdatedOn = c.UpdatedOn,
                Translates = c.Translates
                    .Where(t => t.LanguageId == userLanguage)
                    .Select(t => new BaseTranslateModel
                    {
                        Id = t.Id,
                        LanguageId = t.LanguageId,
                        ColumnName = t.ColumnName,
                        TranslateText = t.TranslateText
                    })
                    .ToList()
            })
            .ToListAsync();

        if (categories.Count == 0)
            throw new BadRequestException("Categories not found.");

        int countCategories = await query.CountAsync();

        return PaginatedList<CategoryResponseModel>.Create(
            categories,
            countCategories,
            option.PageNumber,
            option.PageSize);
    }

    public async Task<UpdateCategoryResponseModel> UpdateCategory(Guid id, UpdateCategoryRequest request)
    {
        Category category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id)
                            ?? throw new NotFoundException("Category not found.");

        category.Name = request.Name;
        category.UpdatedOn = DateTime.UtcNow;
        category.Translates = request.Translates
            .Select(t => new CategoryTranslate
            {
                Id = t.Id,
                LanguageId = t.LanguageId,
                ColumnName = t.ColumnName,
                TranslateText = t.TranslateText
            })
            .ToList();

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return new UpdateCategoryResponseModel { Id = category.Id };
    }

    public async Task<DeleteCategoryResponseModel> DeleteCategory(Guid id)
    {
        if (!await context.Categories.AnyAsync(c => c.Id == id))
            throw new NotFoundException("Category not found.");

        await context.Categories
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        return new DeleteCategoryResponseModel("Success");
    }
}