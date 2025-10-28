using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UzTube.Core.Entities;

namespace UzTube.Application.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(
        User user,
        IEnumerable<string> permissions,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("JwtConfigurations");
        var secretKey = jwtSection["SecretKey"];
        var expireHours = Convert.ToInt32(jwtSection["ExpireHours"]);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("permissions", JsonSerializer.Serialize(permissions))
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? throw new InvalidOperationException("SecretKey is empty."))),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expireHours),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}