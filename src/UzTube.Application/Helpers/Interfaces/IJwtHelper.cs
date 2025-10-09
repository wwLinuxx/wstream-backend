using UzTube.Entities;

namespace UzTube.Application.Helpers.Interfaces;

public interface IJwtHelper
{
    Task<string> GenerateTokenAsync(User user);
}