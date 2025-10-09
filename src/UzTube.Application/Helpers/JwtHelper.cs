using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UzTube.Application.Helpers.Interfaces;
using UzTube.Entities;
using UzTube.Interfaces;

namespace UzTube.Application.Helpers;

public class JwtHelper : IJwtHelper
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public JwtHelper(
        IConfiguration configuration,
        IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        IConfigurationSection jwtSection = _configuration.GetSection("JwtOptions");

        Claim[] claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, JsonSerializer.Serialize(await _userRepository.GetUserAllRolesAsync(user.Id))),
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
