using Microsoft.EntityFrameworkCore;
using UzTube.Entities;

namespace UzTube.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserFollower> UserFollowers { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserCountry> UserCountries { get; set; }
    public DbSet<UserPlaylist> UserPlaylists { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostView> PostViews { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostCommentLike> PostCommentLikes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<PlaylistPost> PlaylistsPosts { get; set; }
    public DbSet<PostRating> PostRatings { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Permissions
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(p => p.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        // Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(r => r.Description)
                .HasMaxLength(100)
                .IsUnicode(false);

            // Seed Roles
            /*entity.HasData(
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
        });

        // Role Permissions
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(rp => new
            {
                rp.RoleId,
                rp.PermissionId
            });

            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(p => p.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            //entity.HasIndex(u => u.Username)
            //    .IsUnique();

            //entity.Property(u => u.Username)
            //    .HasMaxLength(50)
            //    .IsUnicode(false);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        // User Roles
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new
            {
                ur.UserId,
                ur.RoleId
            });

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User Profile
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(up => up.UserId);

            entity.HasOne(up => up.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(up => up.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(up => up.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasIndex(up => up.PhoneNumber)
                .IsUnique();

            entity.ToTable(tb => 
                tb.HasCheckConstraint("CK_UserProfile_Age_Min_7_Max_90", "\"Age\" BETWEEN 7 AND 90"));
            
            entity.HasOne(up => up.Country)
                .WithOne(c => c.UserProfile)
                .HasForeignKey<UserProfile>(up => up.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // User Follower
        modelBuilder.Entity<UserFollower>(entity =>
        {
            entity.HasKey(uf => new
            {
                uf.FollowingId,
                uf.FollowerId
            });

            entity.HasOne(uf => uf.Follower)
                .WithMany(f => f.UserFollowers)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(uf => uf.Following)
                .WithMany(f => f.UserFollowing)
                .HasForeignKey(uf => uf.FollowingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Country
        modelBuilder.Entity<UserCountry>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        // Post
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.User)
                .WithMany(u => u.UserPosts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(p => p.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.Property(p => p.Duration)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(p => p.PhotoUrl)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.Property(p => p.VideoUrl)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        // Post Views
        modelBuilder.Entity<PostView>(entity =>
        {
            entity.HasKey(pv => new
            {
                pv.Id,
                pv.UserId,
                pv.PostId
            });

            entity.HasOne(pv => pv.User)
                .WithMany(u => u.PostViews)
                .HasForeignKey(pv => pv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pv => pv.Post)
                .WithMany(p => p.PostViews)
                .HasForeignKey(pv => pv.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Post Likes
        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(l => new
            {
                l.PostId,
                l.UserId
            });

            entity.HasOne(l => l.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Post Comments
        modelBuilder.Entity<PostComment>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasOne(c => c.Post)
                .WithMany(p => p.PostComments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.User)
                .WithMany(u => u.PostComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(c => c.Comment)
                .HasMaxLength(1500)
                .IsUnicode(false);
        });

        // Post Comment Likes
        modelBuilder.Entity<PostCommentLike>(entity =>
        {
            entity.HasKey(l => new
            {
                l.CommentId,
                l.UserId
            });
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        // Post Category
        modelBuilder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(pc => new
            {
                pc.PostId,
                pc.CategoryId
            });

            entity.HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User Playlist
        modelBuilder.Entity<UserPlaylist>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.HasOne(up => up.User)
                .WithMany(u => u.UserPlaylists)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(up => up.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        // Playlist Post 
        modelBuilder.Entity<PlaylistPost>(entity =>
        {
            entity.HasKey(p => new
            {
                p.PlaylistId,
                p.PostId
            });
        });

        // Post Rating
        modelBuilder.Entity<PostRating>(entity =>
        {
            entity.HasKey(r => new
            {
                r.Id,
                r.PostId,
                r.UserId
            });

            entity.HasOne(r => r.Post)
                .WithMany(p => p.PostRatings)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.User)
                .WithMany(u => u.PostRatings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}