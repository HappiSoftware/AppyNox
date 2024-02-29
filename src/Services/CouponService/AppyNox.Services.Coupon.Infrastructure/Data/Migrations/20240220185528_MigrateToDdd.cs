using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrateToDdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Coupons",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Coupons",
                newName: "Audit_UpdateDate");

            migrationBuilder.RenameColumn(
                name: "MinAmount",
                table: "Coupons",
                newName: "Amount_MinAmount");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "Coupons",
                newName: "Amount_DiscountAmount");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Coupons",
                newName: "Audit_CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Coupons",
                newName: "Audit_CreatedBy");

            migrationBuilder.AlterColumn<Guid>(
                name: "CouponDetailEntityId",
                table: "Coupons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Audit_CreatedBy",
                table: "CouponDetailTags",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_CreationDate",
                table: "CouponDetailTags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_UpdateDate",
                table: "CouponDetailTags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Audit_UpdatedBy",
                table: "CouponDetailTags",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Audit_CreatedBy",
                table: "CouponDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_CreationDate",
                table: "CouponDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_UpdateDate",
                table: "CouponDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Audit_UpdatedBy",
                table: "CouponDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "Code",
                value: "EXD10");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audit_CreatedBy",
                table: "CouponDetailTags");

            migrationBuilder.DropColumn(
                name: "Audit_CreationDate",
                table: "CouponDetailTags");

            migrationBuilder.DropColumn(
                name: "Audit_UpdateDate",
                table: "CouponDetailTags");

            migrationBuilder.DropColumn(
                name: "Audit_UpdatedBy",
                table: "CouponDetailTags");

            migrationBuilder.DropColumn(
                name: "Audit_CreatedBy",
                table: "CouponDetails");

            migrationBuilder.DropColumn(
                name: "Audit_CreationDate",
                table: "CouponDetails");

            migrationBuilder.DropColumn(
                name: "Audit_UpdateDate",
                table: "CouponDetails");

            migrationBuilder.DropColumn(
                name: "Audit_UpdatedBy",
                table: "CouponDetails");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                table: "Coupons",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdateDate",
                table: "Coupons",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreationDate",
                table: "Coupons",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                table: "Coupons",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Amount_MinAmount",
                table: "Coupons",
                newName: "MinAmount");

            migrationBuilder.RenameColumn(
                name: "Amount_DiscountAmount",
                table: "Coupons",
                newName: "DiscountAmount");

            migrationBuilder.AlterColumn<Guid>(
                name: "CouponDetailEntityId",
                table: "Coupons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "Code",
                value: "EXF50");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                columns: new[] { "CreatedBy", "CreationDate", "DiscountAmount", "MinAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 16, 2, 20, 24, 302, DateTimeKind.Utc).AddTicks(7049), 10.65, 100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                columns: new[] { "CreatedBy", "CreationDate", "DiscountAmount", "MinAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 16, 2, 20, 24, 302, DateTimeKind.Utc).AddTicks(7084), 20.550000000000001, 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" });
        }
    }
}
