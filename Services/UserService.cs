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

    public bool Exists(int userId)
        => _context.Users.Any(u => u.Id == userId);

    public User GetById(int userId)
        => _context.Users.FirstOrDefault(u => u.Id == userId);

    public IEnumerable<User> Users()
        => _context.Users;

    public IEnumerable<string> Permissions(int userId)
    {
        return _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct();
    }

    public IEnumerable<string> Roles()
    {
        return _context.UserRoles
            .Select(ur => ur.Role.Name)
            .Distinct();
    }

    public IEnumerable<string> Roles(int userId)
    {
        return _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .Distinct();
    }

}
