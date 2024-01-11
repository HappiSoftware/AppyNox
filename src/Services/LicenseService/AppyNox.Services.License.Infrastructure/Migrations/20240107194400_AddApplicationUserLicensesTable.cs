using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserLicensesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserLicenses_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 1, 6, 19, 43, 59, 466, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLicenses_LicenseId",
                table: "ApplicationUserLicenses",
                column: "LicenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserLicenses");

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 1, 2, 21, 33, 34, 534, DateTimeKind.Utc).AddTicks(4202));
        }
    }
}
