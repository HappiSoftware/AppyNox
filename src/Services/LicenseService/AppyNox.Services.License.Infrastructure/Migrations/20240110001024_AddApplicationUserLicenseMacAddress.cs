using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserLicenseMacAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "ApplicationUserLicenses");

            migrationBuilder.AddColumn<int>(
                name: "MaxMacAddresses",
                table: "Licenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationUserLicenseMacAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    ApplicationUserLicenseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLicenseMacAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserLicenseMacAddresses_ApplicationUserLicenses_~",
                        column: x => x.ApplicationUserLicenseId,
                        principalTable: "ApplicationUserLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                columns: new[] { "ExpirationDate", "MaxMacAddresses" },
                values: new object[] { new DateTime(2025, 1, 9, 0, 10, 24, 115, DateTimeKind.Utc).AddTicks(3656), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLicenses_UserId_LicenseId",
                table: "ApplicationUserLicenses",
                columns: new[] { "UserId", "LicenseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLicenseMacAddresses_ApplicationUserLicenseId",
                table: "ApplicationUserLicenseMacAddresses",
                column: "ApplicationUserLicenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserLicenseMacAddresses");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserLicenses_UserId_LicenseId",
                table: "ApplicationUserLicenses");

            migrationBuilder.DropColumn(
                name: "MaxMacAddresses",
                table: "Licenses");

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "ApplicationUserLicenses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 1, 6, 19, 43, 59, 466, DateTimeKind.Utc).AddTicks(7483));
        }
    }
}
