using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzTube.Migrations
{
    /// <inheritdoc />
    public partial class Added_IsPrivate_Default_False_To_Post : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Posts");
        }
    }
}
