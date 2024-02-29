using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    DiscountAmount = table.Column<double>(type: "double precision", nullable: false),
                    MinAmount = table.Column<int>(type: "integer", nullable: false),
                    Detail = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    CouponDetailEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupons_CouponDetails_CouponDetailEntityId",
                        column: x => x.CouponDetailEntityId,
                        principalTable: "CouponDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CouponDetails",
                columns: new[] { "Id", "Code", "Detail" },
                values: new object[] { new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"), "EXF50", "TestDetail" });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CouponDetailEntityId", "Description", "Detail", "DiscountAmount", "MinAmount" },
                values: new object[,]
                {
                    { new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "EXF50", new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"), "Description", "Detail1", 10.65, 100 },
                    { new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"), "EXF60", new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"), "Description2", "Detail2", 20.550000000000001, 200 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CouponDetailEntityId",
                table: "Coupons",
                column: "CouponDetailEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "CouponDetails");
        }
    }
}
