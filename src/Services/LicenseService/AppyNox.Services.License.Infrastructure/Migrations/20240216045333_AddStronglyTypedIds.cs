using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStronglyTypedIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 2, 15, 4, 53, 32, 725, DateTimeKind.Utc).AddTicks(8870));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 1, 16, 10, 50, 13, 898, DateTimeKind.Utc).AddTicks(442));
        }
    }
}
