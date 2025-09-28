using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IUserService
{
    Task<bool> ExistsAsync(int userId);
    Task<bool> ExistsAsync(string email);
    Task<User> GetByIdAsync(int userId);
    Task<User> GetByEmailAsync(string email);
    Task<List<User>> Users();
    Task<List<string>> Roles();
    Task<List<string>> Roles(int userId);
    Task<List<string>> Permissions(int userId);
}