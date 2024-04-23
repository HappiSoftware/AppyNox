using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCouponHistorIdStatic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("30a40580-1b9f-4154-b69c-42b0a9b36db3"));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("f8a63ca2-5c0b-4cf9-9000-3b54ac51e6bf"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 100, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("f8a63ca2-5c0b-4cf9-9000-3b54ac51e6bf"));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("30a40580-1b9f-4154-b69c-42b0a9b36db3"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 100, null, null });
        }
    }
}
