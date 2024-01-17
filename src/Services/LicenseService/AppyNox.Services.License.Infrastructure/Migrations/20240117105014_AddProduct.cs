using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Licenses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                columns: new[] { "ExpirationDate", "ProductId" },
                values: new object[] { new DateTime(2025, 1, 16, 10, 50, 13, 898, DateTimeKind.Utc).AddTicks(442), new Guid("9991492a-118c-4f20-ac8c-76410d57957c") });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("9991492a-118c-4f20-ac8c-76410d57957c"), "PROD1", "AppyNox" });

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_ProductId",
                table: "Licenses",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Products_ProductId",
                table: "Licenses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Products_ProductId",
                table: "Licenses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_ProductId",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Licenses");

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 1, 9, 0, 10, 24, 115, DateTimeKind.Utc).AddTicks(3656));
        }
    }
}
