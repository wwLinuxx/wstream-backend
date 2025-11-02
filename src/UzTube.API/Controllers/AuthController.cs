using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.User;
using UzTube.Application.Services;

namespace UzTube.API.Controllers;

public class AuthController(
    IUserService userService
) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync(CreateUserModel model)
    {
        return Ok(ApiResult<CreateUserResponseModel>.Success(
            await userService.CreateAsync(model)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserModel model)
    {
        return Ok(ApiResult<LoginResponseModel>.Success(
            await userService.LoginAsync(model)));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync()
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.GetMeAsync()));
    }
}