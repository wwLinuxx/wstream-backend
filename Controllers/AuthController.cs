using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Login(LoginDTO dto)
    {
        return _userRepository.Login(dto);
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDTO dto)
    {
        return _userRepository.Register(dto);
    }

    [HttpGet("profile/{userId}")]
    public IActionResult UserProfile([FromRoute] int userId)
    {
        return _userRepository.UserProfile(userId);
    }
}
