using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageAndRentalStoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "deliverable_masters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    configurable_attrs = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliverable_masters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "package_deliverables",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deliverable_master_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    configuration = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package_deliverables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "package_pricings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pricing_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    base_price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    discount_percentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    final_price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    duration_hours = table.Column<int>(type: "integer", nullable: true),
                    is_negotiable = table.Column<bool>(type: "boolean", nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package_pricings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "packages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    package_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    coverage_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_product_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_product_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_product_pricings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hourly_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    daily_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    weekly_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    monthly_rate = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    deposit_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_product_pricings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rental_store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    model = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    condition = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    specifications = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rental_stores",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    store_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    policies = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_stores", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliverable_masters_role_type",
                table: "deliverable_masters",
                column: "role_type");

            migrationBuilder.CreateIndex(
                name: "IX_deliverable_masters_role_type_category",
                table: "deliverable_masters",
                columns: new[] { "role_type", "category" });

            migrationBuilder.CreateIndex(
                name: "IX_package_deliverables_package_id",
                table: "package_deliverables",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_package_deliverables_package_id_deliverable_master_id",
                table: "package_deliverables",
                columns: new[] { "package_id", "deliverable_master_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_package_pricings_package_id",
                table: "package_pricings",
                column: "package_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_packages_professional_profile_id",
                table: "packages",
                column: "professional_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_packages_professional_profile_id_package_type",
                table: "packages",
                columns: new[] { "professional_profile_id", "package_type" });

            migrationBuilder.CreateIndex(
                name: "IX_rental_product_images_rental_product_id",
                table: "rental_product_images",
                column: "rental_product_id");

            migrationBuilder.CreateIndex(
                name: "IX_rental_product_pricings_rental_product_id",
                table: "rental_product_pricings",
                column: "rental_product_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rental_products_rental_store_id",
                table: "rental_products",
                column: "rental_store_id");

            migrationBuilder.CreateIndex(
                name: "IX_rental_products_rental_store_id_category",
                table: "rental_products",
                columns: new[] { "rental_store_id", "category" });

            migrationBuilder.CreateIndex(
                name: "IX_rental_stores_professional_profile_id",
                table: "rental_stores",
                column: "professional_profile_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deliverable_masters");

            migrationBuilder.DropTable(
                name: "package_deliverables");

            migrationBuilder.DropTable(
                name: "package_pricings");

            migrationBuilder.DropTable(
                name: "packages");

            migrationBuilder.DropTable(
                name: "rental_product_images");

            migrationBuilder.DropTable(
                name: "rental_product_pricings");

            migrationBuilder.DropTable(
                name: "rental_products");

            migrationBuilder.DropTable(
                name: "rental_stores");
        }
    }
}
