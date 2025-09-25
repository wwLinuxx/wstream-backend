using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}