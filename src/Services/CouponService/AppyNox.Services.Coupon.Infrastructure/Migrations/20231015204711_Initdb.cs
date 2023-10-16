using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    DiscountAmount = table.Column<double>(type: "double precision", nullable: false),
                    MinAmount = table.Column<int>(type: "integer", nullable: false),
                    DetailValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "Description", "DetailValue", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("1260115d-98bc-45a2-ad5d-4e18f20309c6"), "EXF60", "Description2", 0, 20.550000000000001, 200 },
                    { new Guid("a2e73b34-000c-4fd1-809b-fd4a02c7fdba"), "EXF50", "Description", 0, 10.65, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
