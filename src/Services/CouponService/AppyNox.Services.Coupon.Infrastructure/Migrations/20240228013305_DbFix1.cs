using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponDetailTags_CouponDetails_CouponDetailEntityId",
                table: "CouponDetailTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailEntityId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_CouponDetailEntityId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CouponDetailEntityId",
                table: "Coupons");

            migrationBuilder.RenameColumn(
                name: "CouponDetailEntityId",
                table: "CouponDetailTags",
                newName: "CouponDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_CouponDetailTags_CouponDetailEntityId",
                table: "CouponDetailTags",
                newName: "IX_CouponDetailTags_CouponDetailId");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponDetailId",
                table: "Coupons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                columns: new[] { "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdateDate", "Audit_UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 28, 1, 33, 4, 992, DateTimeKind.Utc).AddTicks(2868), null, "" });

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                columns: new[] { "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdateDate", "Audit_UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 28, 1, 33, 4, 987, DateTimeKind.Utc).AddTicks(8101), null, "" });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                columns: new[] { "Amount_DiscountAmount", "Amount_MinAmount", "CouponDetailId", "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdateDate", "Audit_UpdatedBy" },
                values: new object[] { 10.65, 100, new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"), "admin", new DateTime(2024, 2, 28, 1, 33, 4, 977, DateTimeKind.Utc).AddTicks(3036), null, "" });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                columns: new[] { "Amount_DiscountAmount", "Amount_MinAmount", "CouponDetailId", "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdateDate", "Audit_UpdatedBy" },
                values: new object[] { 10.65, 100, new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"), "admin", new DateTime(2024, 2, 28, 1, 33, 4, 977, DateTimeKind.Utc).AddTicks(3294), null, "" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CouponDetailId",
                table: "Coupons",
                column: "CouponDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponDetailTags_CouponDetails_CouponDetailId",
                table: "CouponDetailTags",
                column: "CouponDetailId",
                principalTable: "CouponDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailId",
                table: "Coupons",
                column: "CouponDetailId",
                principalTable: "CouponDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponDetailTags_CouponDetails_CouponDetailId",
                table: "CouponDetailTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_CouponDetailId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CouponDetailId",
                table: "Coupons");

            migrationBuilder.RenameColumn(
                name: "CouponDetailId",
                table: "CouponDetailTags",
                newName: "CouponDetailEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_CouponDetailTags_CouponDetailId",
                table: "CouponDetailTags",
                newName: "IX_CouponDetailTags_CouponDetailEntityId");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponDetailEntityId",
                table: "Coupons",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "CouponDetailEntityId",
                value: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "CouponDetailEntityId",
                value: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"));

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CouponDetailEntityId",
                table: "Coupons",
                column: "CouponDetailEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponDetailTags_CouponDetails_CouponDetailEntityId",
                table: "CouponDetailTags",
                column: "CouponDetailEntityId",
                principalTable: "CouponDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailEntityId",
                table: "Coupons",
                column: "CouponDetailEntityId",
                principalTable: "CouponDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
