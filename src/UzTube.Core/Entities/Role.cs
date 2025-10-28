using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
}