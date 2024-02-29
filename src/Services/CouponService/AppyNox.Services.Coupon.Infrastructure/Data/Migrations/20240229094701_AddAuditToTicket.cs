using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Tickets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Tickets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 29, 9, 47, 1, 158, DateTimeKind.Utc).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 29, 9, 47, 1, 152, DateTimeKind.Utc).AddTicks(7121));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 29, 9, 47, 1, 144, DateTimeKind.Utc).AddTicks(814));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 29, 9, 47, 1, 144, DateTimeKind.Utc).AddTicks(1682));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreatedBy", "CreationDate", "ReportDate", "UpdateDate", "UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 29, 9, 47, 1, 158, DateTimeKind.Utc).AddTicks(5411), new DateTime(2024, 2, 29, 9, 47, 1, 158, DateTimeKind.Utc).AddTicks(5410), null, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tickets");

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

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                column: "ReportDate",
                value: new DateTime(2024, 2, 28, 12, 16, 7, 834, DateTimeKind.Utc).AddTicks(4727));
        }
    }
}
