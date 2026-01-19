using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.Subscribe;
using UzTube.Application.Services;

namespace UzTube.API.Endpoints;

public static class SubscribeEndpoints
{
    public static IEndpointRouteBuilder MapSubscribeEndpoinsts(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/subscribes");

        endpoints.MapPost("{userId:guid}", SubscribeAsync)
            .RequirePermission();

        endpoints.MapDelete("{userId:guid}", UnSubscribeAsync)
            .RequirePermission();

        endpoints.MapGet("{userId:guid}", SubscribeStatusAsync)
            .RequirePermission();

        return app;
    }

    private static async Task<IResult> SubscribeAsync(
        [FromRoute] Guid userId,
        [FromServices] ISubscribeService subscribeService)
    {
        return Results.Ok(ApiResult<SubscribeResponseModel>.Success(
            await subscribeService.SubscribeAsync(userId)));
    }

    private static async Task<IResult> UnSubscribeAsync(
        [FromRoute] Guid userId,
        [FromServices] ISubscribeService subscribeService)
    {
        return Results.Ok(ApiResult<SubscribeResponseModel>.Success(
            await subscribeService.UnSubscribeAsync(userId)));
    }

    private static async Task<IResult> SubscribeStatusAsync(
        [FromRoute] Guid userId,
        [FromServices] ISubscribeService subscribeService)
    {
        return Results.Ok(ApiResult<SubscribeResponseModel>.Success(
            await subscribeService.SubscribeStatusAsync(userId)));
    }
}
