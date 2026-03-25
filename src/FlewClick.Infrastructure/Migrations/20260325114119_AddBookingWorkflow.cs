using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlewClick.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agreement_deliverables",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    agreement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deliverable_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    configuration = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agreement_deliverables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "agreements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    package_snapshot = table.Column<string>(type: "jsonb", nullable: false),
                    event_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    event_location = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    event_description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    total_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    terms = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    conditions = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agreements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "booking_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    event_location = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    decline_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cancellation_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    cancelled_by = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_requests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "booking_status_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    to_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    changed_by = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    changed_by_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    reason = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_status_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    content = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platform_fee_payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    professional_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    agreement_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    fee_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    fee_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    razorpay_order_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    razorpay_payment_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    razorpay_signature = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_fee_payments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_agreement_deliverables_agreement_id",
                table: "agreement_deliverables",
                column: "agreement_id");

            migrationBuilder.CreateIndex(
                name: "IX_agreements_booking_request_id",
                table: "agreements",
                column: "booking_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_agreements_booking_request_id_version",
                table: "agreements",
                columns: new[] { "booking_request_id", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_booking_requests_consumer_id",
                table: "booking_requests",
                column: "consumer_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_requests_professional_profile_id",
                table: "booking_requests",
                column: "professional_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_requests_status",
                table: "booking_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_history_booking_request_id",
                table: "booking_status_history",
                column: "booking_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_conversation_id_created_at_utc",
                table: "chat_messages",
                columns: new[] { "conversation_id", "created_at_utc" });

            migrationBuilder.CreateIndex(
                name: "IX_conversations_booking_request_id",
                table: "conversations",
                column: "booking_request_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_platform_fee_payments_booking_request_id",
                table: "platform_fee_payments",
                column: "booking_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_platform_fee_payments_professional_profile_id_status",
                table: "platform_fee_payments",
                columns: new[] { "professional_profile_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_platform_fee_payments_razorpay_order_id",
                table: "platform_fee_payments",
                column: "razorpay_order_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agreement_deliverables");

            migrationBuilder.DropTable(
                name: "agreements");

            migrationBuilder.DropTable(
                name: "booking_requests");

            migrationBuilder.DropTable(
                name: "booking_status_history");

            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "platform_fee_payments");
        }
    }
}
