using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailId",
                table: "Coupons");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailId",
                table: "Coupons",
                column: "CouponDetailId",
                principalTable: "CouponDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailId",
                table: "Coupons");

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 1, 33, 4, 992, DateTimeKind.Utc).AddTicks(2868));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 1, 33, 4, 987, DateTimeKind.Utc).AddTicks(8101));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 1, 33, 4, 977, DateTimeKind.Utc).AddTicks(3036));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "Audit_CreationDate",
                value: new DateTime(2024, 2, 28, 1, 33, 4, 977, DateTimeKind.Utc).AddTicks(3294));

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailId",
                table: "Coupons",
                column: "CouponDetailId",
                principalTable: "CouponDetails",
                principalColumn: "Id");
        }
    }
}
