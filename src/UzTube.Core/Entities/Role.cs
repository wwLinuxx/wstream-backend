using UzTube.Core.Common;

namespace UzTube.Entities;

public class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RolePermission> RolePermissions { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
