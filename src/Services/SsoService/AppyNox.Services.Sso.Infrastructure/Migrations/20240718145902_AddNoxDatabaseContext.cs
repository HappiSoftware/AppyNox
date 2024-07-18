using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Sso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoxDatabaseContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "19fc9c45-32a0-48d0-a19c-0b08768ec978", "AQAAAAIAAYagAAAAEHbKmIx0fweVhJDmO9lTk+WPvKWe7pt+M1874wzDY+0QTDpzjSYq0sPAVeismVRO4w==", "3154469d-ba03-41b8-95cf-3747937207f7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9bd19525-febf-4725-8e99-2eba92716419", "AQAAAAIAAYagAAAAEPvvCUzqKxZ4ZQYJM09h0aVFTHGcGPwwFjfCyB+h60kPKri7qZXHBS3pGZLauzsC8Q==", "54ea0f3c-cf92-4e04-95db-7a33231bf0ea" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1749ef3f-95cd-4b6f-9f1f-cf2edbdaa9a0", "AQAAAAIAAYagAAAAEOcRsq0d8C2ytaHY2fESpChkiEeSQsArzPGJjHgyH1ePY3/ztIjWMSFaBNXU9z5OzA==", "c028e3e6-1e6c-4a99-8d76-283e548299c3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e373f72-2fc9-4a34-86d0-fde55a9da142", "AQAAAAIAAYagAAAAENuCjXeryRGU+1fBWsWdIj7+T5zuOyWbxsZULjz89iIxl4V8NEKq8xa+12X3v718DA==", "4a2fcd07-8517-43c7-b4e2-23c81c408fe6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "497c55b0-62da-4921-9345-b58b82bd029c", "AQAAAAIAAYagAAAAELIkY6pXxeKuXd7ew84/YcKeqb/o5iJB8wsQz+QB6BpCHfHC0oPlqZgan3FEsyml5A==", "75362c3d-0a90-4699-9ad0-40e165470f4c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22983e7e-7b0c-4f6a-9f2d-15b29bea4c0b", "AQAAAAIAAYagAAAAEDWddlD4GknyLTTEs3lk/gv7traAFYlK6AGy7r3VNVjjEeX+6kLq1n3wmwFfcGrFkw==", "6507632d-22d1-47e4-bd52-d913ea933fd8" });
        }
    }
}
