namespace UzTube.Core.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}