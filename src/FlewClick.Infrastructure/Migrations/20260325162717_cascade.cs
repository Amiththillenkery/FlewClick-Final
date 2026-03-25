using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddForeignKey(
                name: "FK_package_deliverables_packages_package_id",
                table: "package_deliverables",
                column: "package_id",
                principalTable: "packages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_package_pricings_packages_package_id",
                table: "package_pricings",
                column: "package_id",
                principalTable: "packages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_package_deliverables_packages_package_id",
                table: "package_deliverables");

            migrationBuilder.DropForeignKey(
                name: "FK_package_pricings_packages_package_id",
                table: "package_pricings");

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
