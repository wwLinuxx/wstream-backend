using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Follower> Followers { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<View> Views { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTranslate> CategoryTranslates { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<PlaylistPost> PlaylistsPosts { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}