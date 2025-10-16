using UzTube.Entities;

namespace UzTube.DataAccess.Repositories;

public interface IUserRoleRepository
{
    Task AddAsync(UserRole userRole);

    Task AddRangeAsync(List<UserRole> userRoles);

    Task UpdateAsync(UserRole userRole);

    Task UpdateRangeAsync(List<UserRole> userRoles);

    Task DeleteAsync(UserRole userRole);

    Task DeleteRangeAsync(List<UserRole> userRoles);
}
