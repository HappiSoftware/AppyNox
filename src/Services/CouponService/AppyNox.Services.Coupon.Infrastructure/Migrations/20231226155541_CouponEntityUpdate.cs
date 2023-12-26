using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CouponEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("5ee097cb-66b2-4bb2-b425-a5c3898f87c0"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("b7b687cc-a0c7-4553-b7e4-d627fcd9b40e"));

            migrationBuilder.DropColumn(
                name: "DetailValue",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Coupons",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "Coupons",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CouponDetailEntityId", "Description", "Detail", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("19c4ff11-f4fd-4eb3-9332-517018c735d1"), "EXF60", new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "Description2", "Detail2", 20.550000000000001, 200 },
                    { new Guid("bc31782e-a509-4745-a4ec-dc400f9208ac"), "EXF50", new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "Description", "Detail1", 10.65, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("19c4ff11-f4fd-4eb3-9332-517018c735d1"));

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("bc31782e-a509-4745-a4ec-dc400f9208ac"));

            migrationBuilder.DropColumn(
                name: "Detail",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Coupons",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AddColumn<int>(
                name: "DetailValue",
                table: "Coupons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CouponDetailEntityId", "Description", "DetailValue", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("5ee097cb-66b2-4bb2-b425-a5c3898f87c0"), "EXF50", new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "Description", 0, 10.65, 100 },
                    { new Guid("b7b687cc-a0c7-4553-b7e4-d627fcd9b40e"), "EXF60", new Guid("c2feaca4-d82a-4d2e-ba5a-667b685212b4"), "Description2", 0, 20.550000000000001, 200 }
                });
        }
    }
}
