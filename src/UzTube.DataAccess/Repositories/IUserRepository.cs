using UzTube.Entities;

namespace UzTube.DataAccess.Repositories;

public interface IUserRepository
{
    IQueryable<User> QueryUsers();

    Task<List<User>> GetAllUsersAsync();

    Task<User?> GetUserByIdAsync(Guid? id);

    Task<User?> GetUserByEmailAsync(string? email);

    Task<List<string>> GetUserAllPermissionsAsync(Guid id);

    Task<List<string>> GetUserAllRolesAsync(Guid id);

    Task<List<UserRole>> GetUserAllRolesListAsync(Guid id);

    Task<bool> CheckUserByIdAsync(Guid id);

    Task<bool> CheckUserByEmailAsync(string email);

    Task AddAsync(User user);

    Task AddRangeAsync(List<User> users);

    Task UpdateAsync(User user);

    Task UpdateRangeAsync(List<User> users);

    Task DeleteAsync(User user);
    
    Task DeleteRangeAsync(List<User> users);
}