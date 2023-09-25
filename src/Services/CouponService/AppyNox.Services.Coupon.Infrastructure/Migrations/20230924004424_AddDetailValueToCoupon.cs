using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CouponService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailValueToCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("7383188c-a775-4b39-ad7f-6a44d38bdd63"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("eb2264f1-6b31-412c-ba3b-8b4c4b607535"));

            migrationBuilder.AddColumn<int>(
                name: "DetailValue",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "Description", "DetailValue", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("18132dd6-3996-4073-b744-e59fc5105499"), "EXF50", "Description", 0, 10.65, 100 },
                    { new Guid("801a5223-ed86-48b2-8ccd-59c33c54961a"), "EXF60", "Description2", 0, 20.550000000000001, 200 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("18132dd6-3996-4073-b744-e59fc5105499"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("801a5223-ed86-48b2-8ccd-59c33c54961a"));

            migrationBuilder.DropColumn(
                name: "DetailValue",
                table: "Coupons");

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "Description", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("7383188c-a775-4b39-ad7f-6a44d38bdd63"), "EXF50", "Description", 10.65, 100 },
                    { new Guid("eb2264f1-6b31-412c-ba3b-8b4c4b607535"), "EXF60", "Description2", 20.550000000000001, 200 }
                });
        }
    }
}
