using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UzTube.Migrations
{
    /// <inheritdoc />
    public partial class TriggerRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    CREATE OR REPLACE FUNCTION update_post_avg_rating()
    RETURNS TRIGGER AS $$
    BEGIN
      UPDATE ""posts""
      SET rating = COALESCE((
          SELECT AVG(r.rating)::INT
          FROM ""PostRatings"" r
          WHERE r.post_id = COALESCE(NEW.post_id, OLD.post_id)
      ), 0)
      WHERE post_id = COALESCE(NEW.post_id, OLD.post_id);

      RETURN NEW;
    END;
    $$ LANGUAGE plpgsql;
");

            migrationBuilder.Sql(@"
    CREATE TRIGGER trg_update_post_avg_rating
    AFTER INSERT OR UPDATE OR DELETE ON ""PostRatings""
    FOR EACH ROW
    EXECUTE FUNCTION update_post_avg_rating();
");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_update_post_avg_rating ON ""PostRatings"";");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_post_avg_rating;");
        }
    }
}
