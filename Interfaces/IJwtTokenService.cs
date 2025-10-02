using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(User user);
}