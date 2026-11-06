using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashcardApp.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""Users"" (""Id"", ""Username"", ""TotalXp"", ""DailyLimit"", ""DesiredRetention"", ""TopicXp"", ""Deck"")
                VALUES ('6d5f81e1-9a0e-4d50-8bf2-7cf8d2ae7d51', 'admin', 0, 20, 0.90, '{}'::jsonb, '[]'::jsonb)
                ON CONFLICT (""Username"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""Users"" WHERE ""Username"" = 'admin';
            ");
        }
    }
}
