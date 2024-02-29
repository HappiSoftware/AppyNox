using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAndTicketTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketTags_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 12, 16, 7, 834, DateTimeKind.Utc).AddTicks(1262));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 12, 16, 7, 827, DateTimeKind.Utc).AddTicks(9211));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 12, 16, 7, 818, DateTimeKind.Utc).AddTicks(2044));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 12, 16, 7, 818, DateTimeKind.Utc).AddTicks(2271));

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Content", "ReportDate", "Title" },
                values: new object[] { new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"), "Ticket content", new DateTime(2024, 2, 28, 12, 16, 7, 834, DateTimeKind.Utc).AddTicks(4727), "Title" });

            migrationBuilder.InsertData(
                table: "TicketTags",
                columns: new[] { "Id", "Description", "TicketId" },
                values: new object[] { new Guid("6125498b-ca83-4d9f-ae4d-55b97d98b47d"), "Tag Description", new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0") });

            migrationBuilder.CreateIndex(
                name: "IX_TicketTags_TicketId",
                table: "TicketTags",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketTags");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 7, 9, 7, 621, DateTimeKind.Utc).AddTicks(8103));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 7, 9, 7, 616, DateTimeKind.Utc).AddTicks(6918));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 7, 9, 7, 603, DateTimeKind.Utc).AddTicks(425));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 7, 9, 7, 603, DateTimeKind.Utc).AddTicks(677));
        }
    }
}
