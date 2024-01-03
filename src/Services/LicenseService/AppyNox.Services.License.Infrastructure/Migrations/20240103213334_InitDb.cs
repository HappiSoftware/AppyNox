using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.License.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Description = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    LicenseKey = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxUsers = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Licenses",
                columns: new[] { "Id", "Code", "CompanyId", "Description", "ExpirationDate", "LicenseKey", "MaxUsers" },
                values: new object[] { new Guid("00455e60-8524-48df-955c-cc9b1f2e7476"), "LK001", new Guid("221e8b2c-59d5-4e5b-b010-86c239b66738"), "License Description", new DateTime(2025, 1, 2, 21, 33, 34, 534, DateTimeKind.Utc).AddTicks(4202), "7f033381-fbf7-4929-b5f7-c64261b20bf3", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses");
        }
    }
}
