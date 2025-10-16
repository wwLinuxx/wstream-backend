using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public AuthController(
        IHttpContextAccessor httpContextAccessor,
        IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserModel dto)
        => Ok(ApiResult<LoginResponseModel>.Success(
            await _userService.LoginAsync(dto)));

    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync(CreateUserModel dto)
        => Ok(ApiResult<CreateUserResponseModel>.Success(
            await _userService.CreateAsync(dto)));

    [RequirePermission(SystemPermissions.Authorize)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync()
    {
        return Ok(ApiResult<UserResponseModel>.Success(
            await _userService.GetMeAsync()));
    }
}