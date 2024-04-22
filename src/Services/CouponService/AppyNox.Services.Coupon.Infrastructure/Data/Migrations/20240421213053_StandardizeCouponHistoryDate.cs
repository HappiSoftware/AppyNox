using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StandardizeCouponHistoryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("899cbbc8-fdf5-40f0-9aa2-7efabad887ab"));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("d9bd5f7e-990a-4260-8acf-3015bfa241d5"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Utc), 100, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("d9bd5f7e-990a-4260-8acf-3015bfa241d5"));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("899cbbc8-fdf5-40f0-9aa2-7efabad887ab"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 4, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 21, 1, 51, 43, 752, DateTimeKind.Utc).AddTicks(1644), 100, null, null });
        }
    }
}
