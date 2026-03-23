using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitPhotographerVideographerRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_app_users_professional_role",
                table: "app_users");

            migrationBuilder.DropColumn(
                name: "professional_role",
                table: "app_users");

            migrationBuilder.AddColumn<string>(
                name: "professional_roles",
                table: "app_users",
                type: "jsonb",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "professional_roles",
                table: "app_users");

            migrationBuilder.AddColumn<string>(
                name: "professional_role",
                table: "app_users",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_users_professional_role",
                table: "app_users",
                column: "professional_role");
        }
    }
}
