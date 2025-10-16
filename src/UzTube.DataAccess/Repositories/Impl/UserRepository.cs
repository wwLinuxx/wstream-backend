using Microsoft.EntityFrameworkCore;
using UzTube.DataAccess.Persistence;
using UzTube.Entities;

namespace UzTube.DataAccess.Repositories.Impl;

public class UserRepository(DatabaseContext _context) : IUserRepository
{
    public IQueryable<User> QueryUsers()
         => _context.Users.AsQueryable();

    public async Task<List<User>> GetAllUsersAsync()
         => await _context.Users.ToListAsync();

    public async Task<User?> GetUserByIdAsync(Guid? id)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserByEmailAsync(string? email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<List<string>> GetUserAllPermissionsAsync(Guid id)
        => await _context.UserRoles
            .Where(u => u.UserId == id)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();

    public async Task<List<string>> GetUserAllRolesAsync(Guid id)
        => await _context.UserRoles
            .Where(u => u.UserId == id)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

    public async Task<List<UserRole>> GetUserAllRolesListAsync(Guid id)
        => await _context.UserRoles
            .Where(u => u.UserId == id)
            .ToListAsync();

    public async Task<bool> CheckUserByIdAsync(Guid id)
        => await _context.Users.AnyAsync(u => u.Id == id);

    public async Task<bool> CheckUserByEmailAsync(string email)
        => await _context.Users.AnyAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<User> users)
    {
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<User> users)
    {
        _context.Users.UpdateRange(users);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(List<User> users)
    {
        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync();
    }
}