using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDeliverableMasters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "deliverable_masters",
                columns: new[] { "id", "category", "configurable_attrs", "created_at_utc", "description", "is_active", "name", "role_type", "updated_at_utc" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-000000000001"), "Print", new Dictionary<string, object> { ["leaf_count"] = 40, ["size"] = "A4" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Printed photo album with customizable pages", true, "Photo Album", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), "Print", new Dictionary<string, object> { ["size"] = "16x20", ["frame"] = "black" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gallery-wrapped canvas photo print", true, "Canvas Print", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), "Print", new Dictionary<string, object> { ["size"] = "8x10", ["frame_material"] = "wood" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Single framed photograph with mat", true, "Framed Photo", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000004"), "Digital", new Dictionary<string, object> { ["resolution"] = "full", ["format"] = "JPEG" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Professionally edited high-resolution digital photos", true, "Edited Digital Photos", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000005"), "Digital", new Dictionary<string, object> { ["format"] = "RAW+JPEG" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All raw unedited photos from the shoot", true, "Raw Unedited Photos", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), "Digital", new Dictionary<string, object> { ["duration_days"] = 90 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Password-protected online gallery for sharing", true, "Online Gallery", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000007"), "Physical", new Dictionary<string, object> { ["capacity_gb"] = 32 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All photos delivered on a branded USB drive", true, "USB Drive", "Photographer", null },
                    { new Guid("a0000001-0000-0000-0000-000000000008"), "Service", new Dictionary<string, object> { ["duration_hours"] = 3, ["props_included"] = true }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "On-site photo booth with props and instant prints", true, "Photo Booth Setup", "Photographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000001"), "Digital", new Dictionary<string, object> { ["duration_minutes"] = 5, ["resolution"] = "4K" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Short cinematic highlight video of the event", true, "Highlight Reel", "Videographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000002"), "Digital", new Dictionary<string, object> { ["resolution"] = "4K" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Complete uncut video coverage of the event", true, "Full Event Video", "Videographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000003"), "Digital", new Dictionary<string, object> { ["duration_seconds"] = 60, ["aspect_ratio"] = "9:16" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "30-60 second social media teaser clip", true, "Teaser Video", "Videographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000004"), "Digital", new Dictionary<string, object> { ["resolution"] = "4K" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aerial video footage captured by drone", true, "Drone Aerial Footage", "Videographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000005"), "Service", new Dictionary<string, object> { ["duration_minutes"] = 3 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Quick-turnaround edited video delivered same day", true, "Same Day Edit", "Videographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000006"), "Digital", new Dictionary<string, object> { ["format"] = "MP4" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "All raw unedited video clips from the shoot", true, "Raw Video Footage", "Videographer", null },
                    { new Guid("a0000002-0000-0000-0000-000000000007"), "Physical", new Dictionary<string, object> { ["format"] = "Blu-ray", ["copies"] = 2 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finished video on physical disc with custom label", true, "DVD/Blu-ray", "Videographer", null },
                    { new Guid("a0000003-0000-0000-0000-000000000001"), "Digital", new Dictionary<string, object> { ["level"] = "advanced" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Professional retouching per photo (skin, color, exposure)", true, "Photo Retouching", "Editor", null },
                    { new Guid("a0000003-0000-0000-0000-000000000002"), "Digital", new Dictionary<string, object> { ["style"] = "cinematic" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cinematic color grading for video footage", true, "Color Grading", "Editor", null },
                    { new Guid("a0000003-0000-0000-0000-000000000003"), "Digital", new Dictionary<string, object> { ["revisions"] = 2 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full video editing with transitions, music, and titles", true, "Video Editing", "Editor", null },
                    { new Guid("a0000003-0000-0000-0000-000000000004"), "Digital", new Dictionary<string, object> { ["photos_count"] = 5 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Composite multiple photos into artistic collage", true, "Photo Compositing", "Editor", null },
                    { new Guid("a0000003-0000-0000-0000-000000000005"), "Digital", new Dictionary<string, object> { ["pages"] = 40, ["style"] = "magazine" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Custom album layout design with professional typography", true, "Album Design", "Editor", null },
                    { new Guid("a0000003-0000-0000-0000-000000000006"), "Digital", new Dictionary<string, object>(), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Professional background removal and replacement", true, "Background Removal", "Editor", null },
                    { new Guid("a0000004-0000-0000-0000-000000000001"), "Digital", new Dictionary<string, object> { ["resolution"] = "48MP" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High-resolution aerial still photographs", true, "Aerial Photography", "DroneOwner", null },
                    { new Guid("a0000004-0000-0000-0000-000000000002"), "Digital", new Dictionary<string, object> { ["resolution"] = "4K", ["duration_minutes"] = 10 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cinematic aerial video footage", true, "Aerial Video", "DroneOwner", null },
                    { new Guid("a0000004-0000-0000-0000-000000000003"), "Digital", new Dictionary<string, object> { ["format"] = "equirectangular" }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aerial 360-degree panoramic photo", true, "360 Panorama", "DroneOwner", null },
                    { new Guid("a0000004-0000-0000-0000-000000000004"), "Service", new Dictionary<string, object> { ["area_acres"] = 5 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aerial mapping and survey flight for property", true, "Property Survey", "DroneOwner", null },
                    { new Guid("a0000004-0000-0000-0000-000000000005"), "Service", new Dictionary<string, object> { ["platform"] = "YouTube", ["duration_hours"] = 2 }, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Real-time aerial live streaming of the event", true, "Live Streaming", "DroneOwner", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "deliverable_masters",
                keyColumn: "id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000005"));
        }
    }
}
