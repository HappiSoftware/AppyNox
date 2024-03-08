using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewAuditMechanismChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                table: "Products",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdateDate",
                table: "Products",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreationDate",
                table: "Products",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                table: "Products",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                table: "Licenses",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdateDate",
                table: "Licenses",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreationDate",
                table: "Licenses",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                table: "Licenses",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Licenses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                columns: new[] { "CreatedBy", "CreationDate", "ExpirationDate", "UpdateDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 21, 11, 15, DateTimeKind.Utc).AddTicks(2020), new DateTime(2025, 3, 8, 9, 21, 11, 15, DateTimeKind.Utc).AddTicks(1995), null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9991492a-118c-4f20-ac8c-76410d57957c"),
                columns: new[] { "CreatedBy", "CreationDate", "UpdateDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 21, 11, 13, DateTimeKind.Utc).AddTicks(1236), null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreationDate",
                table: "Products",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdateDate",
                table: "Products",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_CreationDate",
                table: "Licenses",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_UpdateDate",
                table: "Licenses",
                column: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_CreationDate",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UpdateDate",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_CreationDate",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_UpdateDate",
                table: "Licenses");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Products",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Products",
                newName: "Audit_UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Products",
                newName: "Audit_CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Products",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Licenses",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Licenses",
                newName: "Audit_UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Licenses",
                newName: "Audit_CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Licenses",
                newName: "Audit_CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Audit_UpdatedBy",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Audit_UpdatedBy",
                table: "Licenses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Licenses",
                keyColumn: "Id",
                keyValue: new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"),
                column: "ExpirationDate",
                value: new DateTime(2025, 2, 20, 2, 26, 46, 493, DateTimeKind.Utc).AddTicks(961));
        }
    }
}
