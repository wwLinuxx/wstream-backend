using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateToken(User user);
}