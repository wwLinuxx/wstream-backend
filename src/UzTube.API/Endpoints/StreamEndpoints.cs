using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.LiveStream;
using UzTube.Application.Services;

namespace UzTube.API.Endpoints;

public static class StreamEndpoints
{
    public static IEndpointRouteBuilder MapStreamEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/streams");

        endpoints.MapPost(string.Empty, CreateStreamAsync)
            .Accepts<CreateStreamRequest>("multipart/form-data")
            .DisableAntiforgery()
            .RequirePermission();

        endpoints.MapGet("{id:guid}", GetStreamAsync);

        endpoints.MapPost("get-streams", GetStreamsAsync);

        endpoints.MapGet("key/{streamId:guid}", GetSreamKeyAsync)
            .RequirePermission();

        // ===================== Webhook =====================

        endpoints.MapGet("webhook/on", WebhookReadyAsync);

        endpoints.MapGet("webhook/off", WebhookDoneAsync);

        return app;
    }

    private static async Task<IResult> CreateStreamAsync(
        [FromForm] CreateStreamRequest request,
        [FromServices] IStreamService streamService)
    {
        return Results.Ok(ApiResult<StreamResponseModel>.Success(
            await streamService.CreateStreamAsync(request)));
    }

    private static async Task<IResult> GetStreamAsync(
        [FromRoute] Guid id,
        [FromServices] IStreamService streamService)
    {
        return Results.Ok(ApiResult<StreamResponseModel>.Success(
            await streamService.GetStreamAsync(id)));
    }

    private static async Task<IResult> GetStreamsAsync(
        [FromBody] PageOption option,
        [FromServices] IStreamService streamService)
    {
        return Results.Ok(ApiResult<PaginatedList<StreamResponseModel>>.Success(
            await streamService.GetStreamsAsync(option)));
    }

    private static async Task<IResult> GetSreamKeyAsync(
        [FromRoute] Guid streamId,
        [FromServices] IStreamService streamService)
    {
        return Results.Ok(ApiResult<StreamKeyResponseModel>.Success(
            await streamService.GetStreamKeyAsync(streamId)));
    }

    // ===================== Webhook =====================

    private static async Task<IResult> WebhookReadyAsync(
        [FromQuery] string streamKey,
        [FromServices] IStreamService streamService)
    {
        return Results.Ok(ApiResult<bool>.Success(
            await streamService.SetOnlineAsync(streamKey)));
    }

    private static async Task<IResult> WebhookDoneAsync(
        [FromQuery] string streamKey,
        [FromServices] IStreamService streamService)
    {
        return Results.Ok(ApiResult<bool>.Success(
            await streamService.SetOfflineAsync(streamKey)));
    }
}
