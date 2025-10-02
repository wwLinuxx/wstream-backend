using Microsoft.EntityFrameworkCore;
using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;

namespace UzTube.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(int userId)
        => await _context.Users.AnyAsync(u => u.Id == userId);

    public async Task<bool> ExistsAsync(string email)
        => await _context.Users.AnyAsync(u => u.Email == email);

    public async Task<User?> GetUserByIdAsync(int userId)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User?> GetUserByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<List<User>> GetAllUsersAsync()
        => await _context.Users.ToListAsync();

    public async Task<List<string>> GetUserAllPermissionsAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<string>> GetAllRolesAsync()
    {
        return await _context.UserRoles
            .Select(ur => ur.Role.Name)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<string>> GetUserAllRolesAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .Distinct()
            .ToListAsync();
    }
}
