using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;

namespace UzTube.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public JwtTokenService(
        AppDbContext context,
        IConfiguration configuration,
        IUserService userService)
    {
        _context = context;
        _configuration = configuration;
        _userService = userService;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        IConfigurationSection jwtSection = _configuration.GetSection("JwtOptions");

        Claim[] claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, JsonSerializer.Serialize(await _userService.GetUserAllRolesAsync(user.Id))),
            new Claim("permissions", JsonSerializer.Serialize(await _userService.GetUserAllPermissionsAsync(user.Id)))
        };

        SigningCredentials signingCredentions = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SecretKey"])),
            SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentions,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(jwtSection["ExpireHours"]))
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
