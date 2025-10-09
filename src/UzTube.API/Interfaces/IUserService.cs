using UzTube.Entities;

namespace UzTube.Interfaces;

public interface IUserService
{
    Task<bool> ExistsAsync(int userId);
    Task<bool> ExistsAsync(string email);
    Task<User> GetUserByIdAsync(int userId);
    Task<User> GetUserByEmailAsync(string email);
    Task<List<User>> GetAllUsersAsync();
    Task<List<string>> GetAllRolesAsync();
    Task<List<string>> GetUserAllRolesAsync(int userId);
    Task<List<string>> GetUserAllPermissionsAsync(int userId);
}