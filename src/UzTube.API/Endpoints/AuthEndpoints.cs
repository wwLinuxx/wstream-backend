using Microsoft.AspNetCore.Mvc;
using UzTube.API.Extensions;
using UzTube.Application.Models;
using UzTube.Application.Models.OtpCode;
using UzTube.Application.Models.User;
using UzTube.Application.Services;

namespace UzTube.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder endpoints = app.MapGroup("api/auth");

        endpoints.MapPost("register", RegisterAsync)
            .Accepts<CreateUserRequest>("multipart/form-data")
            .DisableAntiforgery();

        endpoints.MapPost("login", LoginAsync);

        endpoints.MapGet("profile", GetProfileAsync)
            .RequirePermission();

        endpoints.MapPost("send-otp/{userId:guid}", SendOtpAsync)
            .RequirePermission();

        endpoints.MapPost("verify-otp/{userId:guid}", VerifyOtpAsync)
            .RequirePermission();

        return app;
    }

    private static async Task<IResult> RegisterAsync(
        [FromForm] CreateUserRequest request,
        [FromServices] IAuthService authService)
    {
        return Results.Ok(ApiResult<CreateUserResponseModel>.Success(
            await authService.CreateAsync(request)));
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginUserRequest request,
        [FromServices] IAuthService authService)
    {
        return Results.Ok(ApiResult<LoginResponseModel>.Success(
            await authService.LoginAsync(request)));
    }

    private static async Task<IResult> GetProfileAsync(
        [FromServices] IUserService userService)
    {
        return Results.Ok(ApiResult<UserResponseModel>.Success(
            await userService.GetProfileAsync()));
    }

    private static async Task<IResult> SendOtpAsync(
        [FromRoute] Guid userId,
        [FromServices] IOtpService otpService)
    {
        return Results.Ok(ApiResult<OtpResponseModel>.Success(
            await otpService.SendOtpAsync(userId)));
    }

    private static async Task<IResult> VerifyOtpAsync(
        [FromRoute] Guid userId,
        [FromQuery] string otpCode,
        [FromServices] IOtpService otpService)
    {
        return Results.Ok(ApiResult<OtpResponseModel>.Success(
            await otpService.VerifyOtpAsync(userId, otpCode)));
    }
}