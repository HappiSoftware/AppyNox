using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRetryToOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("f5a9014c-88c9-44ab-983c-81191d73b271"));

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "OutboxMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CouponHistories",
                keyColumn: "Id",
                keyValue: new Guid("9bb64f7a-f656-40eb-9206-845cbee05ef2"));

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "OutboxMessages");

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 17, 5, 24, 43, DateTimeKind.Utc).AddTicks(5123));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 17, 5, 24, 41, DateTimeKind.Utc).AddTicks(5964));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("f5a9014c-88c9-44ab-983c-81191d73b271"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 3, 13, 17, 5, 24, 44, DateTimeKind.Utc).AddTicks(3304), new DateTime(2024, 3, 13, 17, 5, 24, 44, DateTimeKind.Utc).AddTicks(3303), 100, null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 17, 5, 24, 40, DateTimeKind.Utc).AddTicks(2624));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 17, 5, 24, 40, DateTimeKind.Utc).AddTicks(3113));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreationDate", "ReportDate" },
                values: new object[] { new DateTime(2024, 3, 13, 17, 5, 24, 43, DateTimeKind.Utc).AddTicks(7408), new DateTime(2024, 3, 13, 17, 5, 24, 43, DateTimeKind.Utc).AddTicks(7407) });
        }
    }
}
