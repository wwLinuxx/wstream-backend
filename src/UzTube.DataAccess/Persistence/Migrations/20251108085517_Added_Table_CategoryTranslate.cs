using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzTube.DataAccess.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Table_CategoryTranslate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryTranslates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    ColumnName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslates_Categories_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedOn",
                value: new DateTime(2025, 11, 8, 8, 55, 16, 429, DateTimeKind.Utc).AddTicks(4030));

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslates_OwnerId",
                table: "CategoryTranslates",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTranslates");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedOn",
                value: new DateTime(2025, 11, 6, 10, 36, 18, 592, DateTimeKind.Utc).AddTicks(1780));
        }
    }
}
