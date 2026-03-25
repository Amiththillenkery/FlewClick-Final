using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioAndInstagram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "app_users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "instagram_connections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instagram_user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    access_token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    token_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_sync_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instagram_connections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instagram_media_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    media_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    media_url = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    thumbnail_url = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    caption = table.Column<string>(type: "character varying(2200)", maxLength: 2200, nullable: true),
                    permalink = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    posted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_items", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_instagram_connections_instagram_user_id",
                table: "instagram_connections",
                column: "instagram_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_instagram_connections_professional_profile_id",
                table: "instagram_connections",
                column: "professional_profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_items_professional_profile_id",
                table: "portfolio_items",
                column: "professional_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_portfolio_items_professional_profile_id_instagram_media_id",
                table: "portfolio_items",
                columns: new[] { "professional_profile_id", "instagram_media_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "instagram_connections");

            migrationBuilder.DropTable(
                name: "portfolio_items");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "app_users");
        }
    }
}
