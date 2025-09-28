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

    [UserOrAdminProfileView]
    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> UserProfile([FromRoute] int userId)
    {
        return await _userRepository.UserProfile(userId);
    }
}
