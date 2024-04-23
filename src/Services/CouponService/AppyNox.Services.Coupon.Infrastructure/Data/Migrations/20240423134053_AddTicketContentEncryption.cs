using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketContentEncryption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("d9bd5f7e-990a-4260-8acf-3015bfa241d5"));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("30a40580-1b9f-4154-b69c-42b0a9b36db3"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 100, null, null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                column: "Content",
                value: "VJoH8lwQ25woMbFp2sf1vA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("30a40580-1b9f-4154-b69c-42b0a9b36db3"));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("d9bd5f7e-990a-4260-8acf-3015bfa241d5"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 100, null, null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                column: "Content",
                value: "Ticket content");
        }
    }
}
