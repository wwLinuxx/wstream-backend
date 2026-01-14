using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.Like;
using UzTube.Application.Services;
using UzTube.Core.Enums;

namespace UzTube.API.Endpoints;

public static class LikeEndpoints
{
    public static IEndpointRouteBuilder MapLikeEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/likes");

        endpoints.MapPost("{postId:guid}", LikeAsync);

        endpoints.MapGet("{postId:guid}", LikeStatusAsync);

        endpoints.MapDelete("{postId:guid}", UnLikeAsync);

        return app;
    }

    private static async Task<IResult> LikeAsync(
        [FromRoute] Guid postId,
        [FromServices] ILikeService likeService)
    {
        return Results.Ok(ApiResult<CreateLikeResponseModel>.Success(
            await likeService.LikeAsync(postId)));
    }

    private static async Task<IResult> LikeStatusAsync(
        [FromRoute] Guid postId,
        [FromServices] ILikeService likeService)
    {
        return Results.Ok(ApiResult<LikeResponseModel>.Success(
            await likeService.LikeStatusAsync(postId)));
    }

    private static async Task<IResult> UnLikeAsync(
        [FromRoute] Guid postId,
        [FromServices] ILikeService likeService)
    {
        return Results.Ok(ApiResult<DeleteLikeResponseModel>.Success(
            await likeService.UnLikeAsync(postId)));
    }
}