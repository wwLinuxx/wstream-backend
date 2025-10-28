using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UzTube.Core.Entities;

namespace UzTube.DataAccess.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
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


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}