using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 2, 20, 2, 26, 46, 493, DateTimeKind.Utc).AddTicks(961));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 2, 20, 2, 5, 18, 161, DateTimeKind.Utc).AddTicks(1799));
        }
    }
}
