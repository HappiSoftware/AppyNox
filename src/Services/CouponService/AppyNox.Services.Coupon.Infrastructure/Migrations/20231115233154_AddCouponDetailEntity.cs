using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCouponDetailEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("1260115d-98bc-45a2-ad5d-4e18f20309c6"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("a2e73b34-000c-4fd1-809b-fd4a02c7fdba"));

            migrationBuilder.AddColumn<Guid>(
                name: "CouponDetailEntityId",
                table: "Coupons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CouponDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponDetails", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CouponDetails",
                columns: new[] { "Id", "Code", "Detail" },
                values: new object[] { new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "EXF50", "TestDetail" });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CouponDetailEntityId", "Description", "DetailValue", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("5ee097cb-66b2-4bb2-b425-a5c3898f87c0"), "EXF50", new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "Description", 0, 10.65, 100 },
                    { new Guid("b7b687cc-a0c7-4553-b7e4-d627fcd9b40e"), "EXF60", new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "Description2", 0, 20.550000000000001, 200 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CouponDetailEntityId",
                table: "Coupons",
                column: "CouponDetailEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailEntityId",
                table: "Coupons",
                column: "CouponDetailEntityId",
                principalTable: "CouponDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_CouponDetails_CouponDetailEntityId",
                table: "Coupons");

            migrationBuilder.DropTable(
                name: "CouponDetails");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_CouponDetailEntityId",
                table: "Coupons");

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("5ee097cb-66b2-4bb2-b425-a5c3898f87c0"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("b7b687cc-a0c7-4553-b7e4-d627fcd9b40e"));

            migrationBuilder.DropColumn(
                name: "CouponDetailEntityId",
                table: "Coupons");

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "Description", "DetailValue", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("1260115d-98bc-45a2-ad5d-4e18f20309c6"), "EXF60", "Description2", 0, 20.550000000000001, 200 },
                    { new Guid("a2e73b34-000c-4fd1-809b-fd4a02c7fdba"), "EXF50", "Description", 0, 10.65, 100 }
                });
        }
    }
}
