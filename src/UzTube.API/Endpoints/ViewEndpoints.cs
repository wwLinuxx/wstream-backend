using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.View;
using UzTube.Application.Services;

namespace UzTube.API.Endpoints;

public static class ViewEndpoints
{
    public static IEndpointRouteBuilder MapViewEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/view");

        endpoints.MapPost("{postId:guid}", ViewPostAsync);

        return app;
    }

    private static async Task<IResult> ViewPostAsync(
        [FromRoute] Guid postId,
        [FromServices] IViewService viewService)
    {
        return Results.Ok(ApiResult<ViewResponseModel>.Success(
            await viewService.ViewPostAsync(postId)));
    }
}
