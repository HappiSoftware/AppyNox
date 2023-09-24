using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CouponService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialDataToCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Coupons",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "Description", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("7383188c-a775-4b39-ad7f-6a44d38bdd63"), "EXF50", "Description", 10.65, 100 },
                    { new Guid("eb2264f1-6b31-412c-ba3b-8b4c4b607535"), "EXF60", "Description2", 20.550000000000001, 200 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("7383188c-a775-4b39-ad7f-6a44d38bdd63"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("eb2264f1-6b31-412c-ba3b-8b4c4b607535"));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);
        }
    }
}
