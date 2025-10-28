using Microsoft.AspNetCore.Mvc;
using UzTube.Application.Models;
using UzTube.Application.Models.User;
using UzTube.Application.Services;

namespace UzTube.API.Controllers;

public class AuthController(
    IUserService userService
) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserModel model)
    {
        return Ok(ApiResult<LoginResponseModel>.Success(
            await userService.LoginAsync(model)));
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync(CreateUserModel model)
    {
        return Ok(ApiResult<CreateUserResponseModel>.Success(
            await userService.CreateAsync(model)));
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync()
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await userService.GetMeAsync()));
    }
}