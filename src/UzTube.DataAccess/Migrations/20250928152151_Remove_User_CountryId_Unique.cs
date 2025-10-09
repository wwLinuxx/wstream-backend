using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzTube.Migrations
{
    /// <inheritdoc />
    public partial class Remove_User_CountryId_Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles",
                column: "CountryId",
                unique: false);
        }

    }
}
