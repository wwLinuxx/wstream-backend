using Microsoft.AspNetCore.Mvc;
using UzTube.Attributes;
using UzTube.Interfaces;
using UzTube.Models.DTO;

namespace UzTube.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        return await _userRepository.Login(dto);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        return await _userRepository.Register(dto);
    }

    [RequirePermission(SystemPermissions.Authorize)]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        return await _userRepository.Me();
    }
}
