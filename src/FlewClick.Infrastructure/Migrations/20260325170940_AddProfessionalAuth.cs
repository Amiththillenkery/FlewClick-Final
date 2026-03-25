using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionalAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "app_users",
                newName: "password_hash");

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "app_users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    revoked_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    replaced_by_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["leaf_count"] = 40, ["size"] = "A4" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["size"] = "16x20", ["frame"] = "black" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["size"] = "8x10", ["frame_material"] = "wood" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "full", ["format"] = "JPEG" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "RAW+JPEG" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000006"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_days"] = 90 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000007"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["capacity_gb"] = 32 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000008"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_hours"] = 3, ["props_included"] = true });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_minutes"] = 5, ["resolution"] = "4K" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "4K" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_seconds"] = 60, ["aspect_ratio"] = "9:16" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "4K" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_minutes"] = 3 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000006"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "MP4" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000007"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "Blu-ray", ["copies"] = 2 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["level"] = "advanced" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["style"] = "cinematic" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["revisions"] = 2 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["photos_count"] = 5 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["pages"] = 40, ["style"] = "magazine" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000006"),
                column: "configurable_attrs",
                value: new Dictionary<string, object>());

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "48MP" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "4K", ["duration_minutes"] = 10 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "equirectangular" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["area_acres"] = 5 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["platform"] = "YouTube", ["duration_hours"] = 2 });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_app_user_id",
                table: "refresh_tokens",
                column: "app_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "app_users",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "app_users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["leaf_count"] = 40, ["size"] = "A4" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["size"] = "16x20", ["frame"] = "black" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["size"] = "8x10", ["frame_material"] = "wood" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "full", ["format"] = "JPEG" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "RAW+JPEG" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000006"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_days"] = 90 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000007"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["capacity_gb"] = 32 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000008"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_hours"] = 3, ["props_included"] = true });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_minutes"] = 5, ["resolution"] = "4K" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "4K" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_seconds"] = 60, ["aspect_ratio"] = "9:16" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "4K" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["duration_minutes"] = 3 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000006"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "MP4" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000007"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "Blu-ray", ["copies"] = 2 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["level"] = "advanced" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["style"] = "cinematic" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["revisions"] = 2 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["photos_count"] = 5 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["pages"] = 40, ["style"] = "magazine" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000006"),
                column: "configurable_attrs",
                value: new Dictionary<string, object>());

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000001"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "48MP" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000002"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["resolution"] = "4K", ["duration_minutes"] = 10 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000003"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["format"] = "equirectangular" });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000004"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["area_acres"] = 5 });

            migrationBuilder.UpdateData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000005"),
                column: "configurable_attrs",
                value: new Dictionary<string, object> { ["platform"] = "YouTube", ["duration_hours"] = 2 });
        }
    }
}
