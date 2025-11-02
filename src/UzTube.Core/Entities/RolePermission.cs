using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public int PermissionId { get; set; }

    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
