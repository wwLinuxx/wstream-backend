using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UzTube.Entities;

namespace UzTube.DataAccess.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(r => r.Description)
            .HasMaxLength(100)
            .IsUnicode(false);

        #region
        // Seed Roles
        /*builder.HasData(
            new Role
            {
                Id = 1,
                Name = "root",
                Description = "root - Role",
                RolePermissions = new List<RolePermission>
                {
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewUsers },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ManageUser },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewRoles },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ManageRoles },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewPermissions },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewAuditLogs },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ManageSystem },
                    new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewPost }
                }
            },
            new Role
            {
                Id = 2,
                Name = "Admin",
                Description = "Admin - Role",
                RolePermissions = new List<RolePermission>
                {
                    new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ViewUsers },
                    new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ManageUser },
                    new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ViewRoles },
                    new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ManageRoles }
                }
            }
        );*/
        #endregion
    }
}
