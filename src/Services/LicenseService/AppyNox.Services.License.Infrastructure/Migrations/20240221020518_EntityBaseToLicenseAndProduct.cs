using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntityBaseToLicenseAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Audit_CreatedBy",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_CreationDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_UpdateDate",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Audit_UpdatedBy",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Audit_CreatedBy",
                table: "Licenses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_CreationDate",
                table: "Licenses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_UpdateDate",
                table: "Licenses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Audit_UpdatedBy",
                table: "Licenses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 2, 20, 2, 5, 18, 161, DateTimeKind.Utc).AddTicks(1799));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audit_CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Audit_CreationDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Audit_UpdateDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Audit_UpdatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Audit_CreatedBy",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Audit_CreationDate",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Audit_UpdateDate",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Audit_UpdatedBy",
                table: "Licenses");

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 2, 15, 4, 53, 32, 725, DateTimeKind.Utc).AddTicks(8870));
        }
    }
}
