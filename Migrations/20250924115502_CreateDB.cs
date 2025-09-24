using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UzTube.Migrations;

/// <inheritdoc />
public partial class CreateDB : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Permissions",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                Description = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                Description = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserCountries",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", unicode: false, maxLength: 50, nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserCountries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                Salt = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RolePermissions",
            columns: table => new
            {
                RoleId = table.Column<int>(type: "integer", nullable: false),
                PermissionId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_RolePermissions_Permissions_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RolePermissions_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Title = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "character varying(1000)", unicode: false, maxLength: 1000, nullable: false),
                Duration = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false),
                Rating = table.Column<int>(type: "integer", nullable: false),
                VideoUrl = table.Column<string>(type: "character varying(1000)", unicode: false, maxLength: 1000, nullable: false),
                PostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Posts_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserFollowers",
            columns: table => new
            {
                FollowerId = table.Column<int>(type: "integer", nullable: false),
                FollowingId = table.Column<int>(type: "integer", nullable: false),
                FollowedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserFollowers", x => new { x.FollowingId, x.FollowerId });
                table.ForeignKey(
                    name: "FK_UserFollowers_Users_FollowerId",
                    column: x => x.FollowerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserFollowers_Users_FollowingId",
                    column: x => x.FollowingId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserPlaylists",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                IsPrivate = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserPlaylists", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserPlaylists_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserProfiles",
            columns: table => new
            {
                UserId = table.Column<int>(type: "integer", nullable: false),
                FirstName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                PhoneNumber = table.Column<string>(type: "text", nullable: false),
                Age = table.Column<int>(type: "integer", nullable: false),
                CountryId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserProfiles", x => x.UserId);
                table.CheckConstraint("CK_UserProfile_Age_Min_7_Max_90", "\"Age\" BETWEEN 7 AND 90");
                table.ForeignKey(
                    name: "FK_UserProfiles_UserCountries_CountryId",
                    column: x => x.CountryId,
                    principalTable: "UserCountries",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_UserProfiles_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            columns: table => new
            {
                UserId = table.Column<int>(type: "integer", nullable: false),
                RoleId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostCategories",
            columns: table => new
            {
                PostId = table.Column<int>(type: "integer", nullable: false),
                CategoryId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostCategories", x => new { x.PostId, x.CategoryId });
                table.ForeignKey(
                    name: "FK_PostCategories_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostCategories_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostComments",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostId = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Comment = table.Column<string>(type: "character varying(1500)", unicode: false, maxLength: 1500, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostComments", x => x.Id);
                table.ForeignKey(
                    name: "FK_PostComments_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostComments_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostLikes",
            columns: table => new
            {
                PostId = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: false),
                LikedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostLikes", x => new { x.PostId, x.UserId });
                table.ForeignKey(
                    name: "FK_PostLikes_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostLikes_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostRatings",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                PostId = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: false),
                Rating = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostRatings", x => new { x.Id, x.PostId, x.UserId });
                table.ForeignKey(
                    name: "FK_PostRatings_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostRatings_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostViews",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                PostId = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: false),
                ViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostViews", x => new { x.Id, x.UserId, x.PostId });
                table.ForeignKey(
                    name: "FK_PostViews_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostViews_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PlaylistsPosts",
            columns: table => new
            {
                PlaylistId = table.Column<int>(type: "integer", nullable: false),
                PostId = table.Column<int>(type: "integer", nullable: false),
                UserPlaylistId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlaylistsPosts", x => new { x.PlaylistId, x.PostId });
                table.ForeignKey(
                    name: "FK_PlaylistsPosts_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PlaylistsPosts_UserPlaylists_UserPlaylistId",
                    column: x => x.UserPlaylistId,
                    principalTable: "UserPlaylists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostCommentLikes",
            columns: table => new
            {
                CommentId = table.Column<int>(type: "integer", nullable: false),
                UserId = table.Column<int>(type: "integer", nullable: false),
                PostCommentId = table.Column<int>(type: "integer", nullable: false),
                LikedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostCommentLikes", x => new { x.CommentId, x.UserId });
                table.ForeignKey(
                    name: "FK_PostCommentLikes_PostComments_PostCommentId",
                    column: x => x.PostCommentId,
                    principalTable: "PostComments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostCommentLikes_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Categories_Name",
            table: "Categories",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PlaylistsPosts_PostId",
            table: "PlaylistsPosts",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_PlaylistsPosts_UserPlaylistId",
            table: "PlaylistsPosts",
            column: "UserPlaylistId");

        migrationBuilder.CreateIndex(
            name: "IX_PostCategories_CategoryId",
            table: "PostCategories",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_PostCommentLikes_PostCommentId",
            table: "PostCommentLikes",
            column: "PostCommentId");

        migrationBuilder.CreateIndex(
            name: "IX_PostCommentLikes_UserId",
            table: "PostCommentLikes",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_PostComments_PostId",
            table: "PostComments",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_PostComments_UserId",
            table: "PostComments",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_PostLikes_UserId",
            table: "PostLikes",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_PostRatings_PostId",
            table: "PostRatings",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_PostRatings_UserId",
            table: "PostRatings",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_UserId",
            table: "Posts",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_PostViews_PostId",
            table: "PostViews",
            column: "PostId");

        migrationBuilder.CreateIndex(
            name: "IX_PostViews_UserId",
            table: "PostViews",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_RolePermissions_PermissionId",
            table: "RolePermissions",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_UserFollowers_FollowerId",
            table: "UserFollowers",
            column: "FollowerId");

        migrationBuilder.CreateIndex(
            name: "IX_UserPlaylists_UserId",
            table: "UserPlaylists",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserProfiles_CountryId",
            table: "UserProfiles",
            column: "CountryId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserProfiles_PhoneNumber",
            table: "UserProfiles",
            column: "PhoneNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId",
            table: "UserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_UserName",
            table: "Users",
            column: "UserName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PlaylistsPosts");

        migrationBuilder.DropTable(
            name: "PostCategories");

        migrationBuilder.DropTable(
            name: "PostCommentLikes");

        migrationBuilder.DropTable(
            name: "PostLikes");

        migrationBuilder.DropTable(
            name: "PostRatings");

        migrationBuilder.DropTable(
            name: "PostViews");

        migrationBuilder.DropTable(
            name: "RolePermissions");

        migrationBuilder.DropTable(
            name: "UserFollowers");

        migrationBuilder.DropTable(
            name: "UserProfiles");

        migrationBuilder.DropTable(
            name: "UserRoles");

        migrationBuilder.DropTable(
            name: "UserPlaylists");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "PostComments");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "UserCountries");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.DropTable(
            name: "Posts");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
