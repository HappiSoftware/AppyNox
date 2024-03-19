using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketToTicketTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("9bb64f7a-f656-40eb-9206-845cbee05ef2"));

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 19, 13, 26, 42, 442, DateTimeKind.Utc).AddTicks(1305));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 19, 13, 26, 42, 440, DateTimeKind.Utc).AddTicks(3760));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("ab671c84-7bb8-4ba1-a60f-1e4442b25683"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 3, 19, 13, 26, 42, 443, DateTimeKind.Utc).AddTicks(5360), new DateTime(2024, 3, 19, 13, 26, 42, 443, DateTimeKind.Utc).AddTicks(5357), 100, null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 19, 13, 26, 42, 439, DateTimeKind.Utc).AddTicks(3841));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 19, 13, 26, 42, 439, DateTimeKind.Utc).AddTicks(4112));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreationDate", "ReportDate" },
                values: new object[] { new DateTime(2024, 3, 19, 13, 26, 42, 442, DateTimeKind.Utc).AddTicks(5409), new DateTime(2024, 3, 19, 13, 26, 42, 442, DateTimeKind.Utc).AddTicks(5407) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("ab671c84-7bb8-4ba1-a60f-1e4442b25683"));

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 18, 21, 7, 580, DateTimeKind.Utc).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 18, 21, 7, 577, DateTimeKind.Utc).AddTicks(3183));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("9bb64f7a-f656-40eb-9206-845cbee05ef2"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 3, 13, 18, 21, 7, 581, DateTimeKind.Utc).AddTicks(7365), new DateTime(2024, 3, 13, 18, 21, 7, 581, DateTimeKind.Utc).AddTicks(7363), 100, null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 18, 21, 7, 576, DateTimeKind.Utc).AddTicks(1009));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 18, 21, 7, 576, DateTimeKind.Utc).AddTicks(1278));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreationDate", "ReportDate" },
                values: new object[] { new DateTime(2024, 3, 13, 18, 21, 7, 580, DateTimeKind.Utc).AddTicks(8869), new DateTime(2024, 3, 13, 18, 21, 7, 580, DateTimeKind.Utc).AddTicks(8868) });
        }
    }
}
