using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Sso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    UseSSL = table.Column<bool>(type: "boolean", nullable: false),
                    FromAddress = table.Column<string>(type: "text", nullable: false),
                    FromName = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailProviders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d22d41ec-6dcd-4407-aaa1-dcdee94dc7a4", "AQAAAAIAAYagAAAAENtL76hfjxVVqMzyYs2TzMHGGwZ7LvhoE2SRci3uk7AZSyQLFpA8is9VkaUqq2kR7g==", "6bc7b617-b645-45a8-a168-73eceeb18af4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e1e78e12-f634-4f3e-8973-675452023739", "AQAAAAIAAYagAAAAEFVj9ae9qPDDKSly+xKcHHzLOwncZc2VKk6w1R/+xn6qzfD3zG0jDNiEk/y7OCm/uw==", "6c411693-c21c-4321-845b-d56b7babcf1a" });
        }
    }
}
