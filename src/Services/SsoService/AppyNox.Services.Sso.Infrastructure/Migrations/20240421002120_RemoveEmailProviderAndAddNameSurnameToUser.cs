using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Sso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailProviderAndAddNameSurnameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailProviders");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "caabc0be-72bd-4009-bf4e-7fd3471c5219", "Name2", "AQAAAAIAAYagAAAAEAj8NuIX+Sox2GMnWk3tM04bHNRf44cFDdCOZDTHExQcKGj1qAJj52+4TbatebXcyg==", "d0d1d672-799d-4aeb-b85c-47bbf0658d82", "Surname2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp", "Surname" },
                values: new object[] { "ebc26434-783d-4c87-8478-f49a9dd98416", "Name1", "AQAAAAIAAYagAAAAEIqeG4y0QLG/FsTLaUuZodeQ0GXwBHXGkCjwofTL+7EdPEiaaT8UUU6XedtRBDfDnQ==", "ea60d710-8348-4749-bfa9-dde29bcbf6ba", "Surname1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "EmailProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromAddress = table.Column<string>(type: "text", nullable: false),
                    FromName = table.Column<string>(type: "text", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    UseSSL = table.Column<bool>(type: "boolean", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailProviders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31e68687-35e0-4534-b2cc-2fb863eced6a", "AQAAAAIAAYagAAAAEJjVQWt956MazyDYrfrW69EMs/tf3WJVePShA99G24/U10AAByIvJ1zAWd+i7nDDxQ==", "d48aea4c-df26-4f51-9130-48f91a715bc9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b349e81b-dcd0-4774-875d-2f82f354b851", "AQAAAAIAAYagAAAAECctZAT6KwHXJS1xZlAYsc5DxH2BsB4/YjiKxn27mZkHg06+TwX/FHFAaZOEkW2GIQ==", "9d5ace69-894a-4b66-beb8-0d187a2362a2" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailProviders_CompanyId",
                table: "EmailProviders",
                column: "CompanyId");
        }
    }
}
