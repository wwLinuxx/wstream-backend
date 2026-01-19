using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UzTube.Core.Entities;
using Stream = UzTube.Core.Entities.Stream;

namespace UzTube.DataAccess.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Follower> Followers => Set<Follower>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Stream> Streams => base.Set<Stream>();
    public DbSet<View> Views => Set<View>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryTranslate> CategoryTranslates => Set<CategoryTranslate>();
    public DbSet<PostCategory> PostCategories => Set<PostCategory>();
    public DbSet<PlaylistPost> PlaylistsPosts => Set<PlaylistPost>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}