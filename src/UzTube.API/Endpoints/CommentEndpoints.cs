using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.Comment;
using UzTube.Application.Services;
using UzTube.Core.Enums;

namespace UzTube.API.Endpoints;

public static class CommentEndpoints
{
    public static IEndpointRouteBuilder MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/comments");

        endpoints.MapPost("{postId:guid}", CreateCommentAsync)
            .RequirePermission();

        endpoints.MapGet("{commentId:guid}", GetCommentAsync);

        endpoints.MapPost(@"get-comments/{postId:guid}", GetCommentsAsync);

        return app;
    }

    private static async Task<IResult> CreateCommentAsync(
        [FromRoute] Guid postId,
        [FromBody] CreateCommentRequest request,
        [FromServices] ICommentService commentService)
    {
        return Results.Ok(ApiResult<CreateCommentResponseModel>.Success(
            await commentService.CreateCommentAsync(postId, request)));
    }

    private static async Task<IResult> GetCommentAsync(
        [FromRoute] Guid commentId,
        [FromServices] ICommentService commentService)
    {
        return Results.Ok(ApiResult<CommentResponseModel>.Success(
            await commentService.GetCommentAsync(commentId)));
    }

    private static async Task<IResult> GetCommentsAsync(
        [FromRoute] Guid postId,
        [FromBody] PageOption option,
        [FromServices] ICommentService commentService)
    {
        return Results.Ok(ApiResult<PaginatedList<CommentResponseModel>>.Success(
            await commentService.GetCommentsAsync(postId, option)));
    }
}