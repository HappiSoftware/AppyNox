using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLicenseClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 15, "Permission", "Licenses.View", "e24e99e7-00e4-4007-a042-565eac12d96d" },
                    { 16, "Permission", "Licenses.Create", "e24e99e7-00e4-4007-a042-565eac12d96d" },
                    { 17, "Permission", "Licenses.Edit", "e24e99e7-00e4-4007-a042-565eac12d96d" },
                    { 18, "Permission", "Licenses.Delete", "e24e99e7-00e4-4007-a042-565eac12d96d" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a8bfc75b-2ac3-47e2-b013-8b8a1efba45d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd18ed19-3270-4a56-912d-14ed510d1241", "AQAAAAIAAYagAAAAEItvSgepnZq435JezINsA1qbaO7HGt4cAGo1BGcBzFiNHkkvNBT+FZMzpNXsje89cg==", "bf6452e2-04be-4bbe-9292-4baae1cf7d36" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a8bfc75b-2ac3-47e2-b013-8b8a1efba45d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "468c8d69-45fe-4ec7-a01b-f8d45ebc95c8", "AQAAAAIAAYagAAAAEL7Gq3iAfQONsdMxy9S61TFzNLFzZrvKGpDNN0797cq/oGYclbYydXc0nHxZVvvVww==", "2422d31d-716c-4968-9108-a2b3f9602721" });
        }
    }
}
