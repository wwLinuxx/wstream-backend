using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.Category;
using UzTube.Application.Services;
using UzTube.Core.Enums;

namespace UzTube.API.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/categories");

        endpoints.MapPost(string.Empty, CreateCategoryAsync)
            .RequirePermission(SystemPermissions.CategoryCreate, SystemPermissions.CategoryUpdate);

        endpoints.MapGet("{id:guid}", GetCategoryAsync)
            .RequirePermission(SystemPermissions.CategoryView);

        endpoints.MapPost("get-categories", GetCategoriesAsync)
            .RequirePermission(SystemPermissions.CategoryView)
            .RequireAuthorization();

        endpoints.MapPut("{id:guid}", UpdateCategoryAsync)
            .RequirePermission(SystemPermissions.CategoryUpdate);

        endpoints.MapDelete("{id:guid}", DeleteCategoryAsync)
            .RequirePermission(SystemPermissions.CategoryDelete);

        return app;
    }

    private static async Task<IResult> CreateCategoryAsync(
        [FromBody] CreateCategoryRequest request,
        [FromServices] ICategoryService categoryService)
    {
        return Results.Ok(ApiResult<CreateCategoryResponseModel>.Success(
            await categoryService.CreateCategory(request)));
    }

    private static async Task<IResult> GetCategoryAsync(
        [FromRoute] Guid id,
        [FromServices] ICategoryService categoryService)
    {
        return Results.Ok(ApiResult<CategoryResponseModel>.Success(
            await categoryService.GetCategory(id)));
    }

    private static async Task<IResult> GetCategoriesAsync(
        [FromBody] PageOption option,
        [FromServices] ICategoryService categoryService)
    {
        return Results.Ok(ApiResult<PaginatedList<CategoryResponseModel>>.Success(
            await categoryService.GetCategories(option)));
    }

    private static async Task<IResult> UpdateCategoryAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest request,
        [FromServices] ICategoryService categoryService)
    {
        return Results.Ok(ApiResult<UpdateCategoryResponseModel>.Success(
            await categoryService.UpdateCategory(id, request)));
    }

    private static async Task<IResult> DeleteCategoryAsync(
        [FromRoute] Guid id,
        [FromServices] ICategoryService categoryService)
    {
        return Results.Ok(ApiResult<DeleteCategoryResponseModel>.Success(
            await categoryService.DeleteCategory(id)));
    }
}