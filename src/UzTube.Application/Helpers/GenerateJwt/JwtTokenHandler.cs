using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UzTube.Core.Common;
using UzTube.Core.Entities;

namespace UzTube.Application.Helpers.GenerateJwt;

public static class JwtTokenHandler
{
    public static string GenerateToken(
        User user,
        IEnumerable<string> permissions,
        IConfiguration configuration)
    {
        JwtSettings? jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        Claim[] claims = new[]
        {
            new Claim(CustomClaimNames.Id, user.Id.ToString()),
            new Claim(CustomClaimNames.Email, user.Email),
            new Claim(CustomClaimNames.Permissions, JsonSerializer.Serialize(permissions))
        };

        SigningCredentials signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecretKey)),
            SecurityAlgorithms.HmacSha256
        );

        JwtSecurityToken token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddSeconds(jwtSettings.ExpirationInSeconds),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}