using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Sso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 20, "API.Permission", "Products.View", new Guid("e24e99e7-00e4-4007-a042-565eac12d96d") },
                    { 21, "API.Permission", "Products.Create", new Guid("e24e99e7-00e4-4007-a042-565eac12d96d") },
                    { 22, "API.Permission", "Products.Edit", new Guid("e24e99e7-00e4-4007-a042-565eac12d96d") },
                    { 23, "API.Permission", "Products.Delete", new Guid("e24e99e7-00e4-4007-a042-565eac12d96d") }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "003dd3c8-a3a1-465e-b823-09c52949ccb7", "AQAAAAIAAYagAAAAEAaWsXB34zA44VfCLFN0ZDnuq0HG6rtB+19gP+qCJairxC/FkkiBoN9Vem4ouc9QnA==", "098b4d9e-371f-45d4-bc64-635cea26f318" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a464461f-35cb-4176-877d-18f550071d87", "AQAAAAIAAYagAAAAEHHcLKp0ScXN2dQJ51Wh1CPva9WmhqSoeHRpphLgixD/vysTeTexxqfn6EU3cmtVUw==", "5227a5d4-0bec-47c1-8946-8861955840a8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7e533c75-f2d5-4a55-ab78-fa897ae6f44d", "AQAAAAIAAYagAAAAEI3nZbtfb+oc+ISBiSz64tvw3qyJG7r9r0icBPGsWto1pLW5+3Peg29QjzId5MDjsw==", "7f038812-7850-4d10-a6bd-af0a84357319" });
        }
    }
}
