using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Coupons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Coupons",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Coupons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                columns: new[] { "DeletedBy", "DeletedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                columns: new[] { "DeletedBy", "DeletedDate" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_DeletedDate",
                table: "Coupons",
                column: "DeletedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_IsDeleted",
                table: "Coupons",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coupons_DeletedDate",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_IsDeleted",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Coupons");
        }
    }
}
