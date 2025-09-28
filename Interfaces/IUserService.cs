using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IUserService
{
    Task<bool> ExistsAsync(int userId);
    Task<bool> ExistsAsync(string email);
    Task<User> GetByIdAsync(int userId);
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> Users();
    Task<IEnumerable<string>> Roles();
    Task<IEnumerable<string>> Roles(int userId);
    Task<IEnumerable<string>> Permissions(int userId);
}