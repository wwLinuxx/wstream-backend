using UzTube.DataAccess.Persistence;
using UzTube.Entities;

namespace UzTube.DataAccess.Repositories.Impl;

public class UserRoleRepository(DatabaseContext _context) : IUserRoleRepository
{
    public async Task AddAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<UserRole> userRoles)
    {
        await _context.UserRoles.AddRangeAsync(userRoles);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserRole userRole)
    {
        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(List<UserRole> userRoles)
    {
        _context.UserRoles.RemoveRange(userRoles);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserRole userRole)
    {
        _context.UserRoles.Update(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<UserRole> userRoles)
    {
        _context.UserRoles.UpdateRange(userRoles);
        await _context.SaveChangesAsync();
    }
}
