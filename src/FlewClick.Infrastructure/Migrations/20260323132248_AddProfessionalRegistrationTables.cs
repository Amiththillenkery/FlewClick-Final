using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionalRegistrationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drone_configs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    drone_model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    license_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    has_flight_certification = table.Column<bool>(type: "boolean", nullable: false),
                    max_flight_altitude_meters = table.Column<int>(type: "integer", nullable: true),
                    capabilities = table.Column<string>(type: "jsonb", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drone_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "editing_configs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    software_tools = table.Column<string>(type: "jsonb", nullable: false),
                    specializations = table.Column<string>(type: "jsonb", nullable: false),
                    output_formats = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_editing_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "photography_configs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    styles = table.Column<string>(type: "jsonb", nullable: false),
                    camera_gear = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    shoot_types = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    has_studio = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_photography_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "professional_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    years_of_experience = table.Column<int>(type: "integer", nullable: true),
                    hourly_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    portfolio_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_registration_complete = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_equipments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    equipment_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    daily_rental_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    condition_notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_equipments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_drone_configs_professional_profile_id",
                table: "drone_configs",
                column: "professional_profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_editing_configs_professional_profile_id",
                table: "editing_configs",
                column: "professional_profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_photography_configs_professional_profile_id",
                table: "photography_configs",
                column: "professional_profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_professional_profiles_app_user_id",
                table: "professional_profiles",
                column: "app_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rental_equipments_professional_profile_id",
                table: "rental_equipments",
                column: "professional_profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drone_configs");

            migrationBuilder.DropTable(
                name: "editing_configs");

            migrationBuilder.DropTable(
                name: "photography_configs");

            migrationBuilder.DropTable(
                name: "professional_profiles");

            migrationBuilder.DropTable(
                name: "rental_equipments");
        }
    }
}
