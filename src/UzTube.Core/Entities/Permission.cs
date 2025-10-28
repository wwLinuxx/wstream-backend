using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}