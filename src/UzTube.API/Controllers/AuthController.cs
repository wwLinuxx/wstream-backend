using Microsoft.AspNetCore.Mvc;
using UzTube.API.Controllers;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Controllers;

public class AuthController : ApiController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _service;

    public AuthController(
        IHttpContextAccessor httpContextAccessor,
        IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _service = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserModel model)
    {
        return Ok(ApiResult<LoginResponseModel>.Success(
            await _service.LoginAsync(model)));
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync(CreateUserModel model)
    {
        return Ok(ApiResult<CreateUserResponseModel>.Success(
            await _service.CreateAsync(model)));
    }

    [RequirePermission(SystemPermissions.Authorize)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync()
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await _service.GetMeAsync()));
    }
}