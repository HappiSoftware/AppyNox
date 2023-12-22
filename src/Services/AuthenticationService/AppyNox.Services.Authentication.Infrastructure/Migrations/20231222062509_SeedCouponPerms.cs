using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppyNox.Services.Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCouponPerms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "57bc3893-07a8-4043-a460-4da563f8624f", "019b4122-60ba-4fd4-a82c-627c421d8679" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019b4122-60ba-4fd4-a82c-627c421d8679");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleId",
                value: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81b3d8ea-626e-457c-9e90-a2f2392fc811", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3152a4bf-e990-44af-87d7-daba6f065834", 0, "ad440c35-b635-47c6-b17e-be7728cc903a", "admin@email.com", true, false, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEHDxelvqUn2XgUwPsTWYh2MvjmqAI5YOp71HVvSxtIZguKEPqxTHDm6zVCoGmcwY9Q==", null, false, "5b8d5e3e-62a6-43e9-8aa2-8e333bb90a5b", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 11, "Permission", "Coupons.View", "81b3d8ea-626e-457c-9e90-a2f2392fc811" },
                    { 12, "Permission", "Coupons.Create", "81b3d8ea-626e-457c-9e90-a2f2392fc811" },
                    { 13, "Permission", "Coupons.Edit", "81b3d8ea-626e-457c-9e90-a2f2392fc811" },
                    { 14, "Permission", "Coupons.Delete", "81b3d8ea-626e-457c-9e90-a2f2392fc811" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "81b3d8ea-626e-457c-9e90-a2f2392fc811", "3152a4bf-e990-44af-87d7-daba6f065834" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "81b3d8ea-626e-457c-9e90-a2f2392fc811", "3152a4bf-e990-44af-87d7-daba6f065834" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81b3d8ea-626e-457c-9e90-a2f2392fc811");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3152a4bf-e990-44af-87d7-daba6f065834");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleId",
                value: "57bc3893-07a8-4043-a460-4da563f8624f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "57bc3893-07a8-4043-a460-4da563f8624f", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019b4122-60ba-4fd4-a82c-627c421d8679", 0, "e1257daa-06d1-49ae-a4c8-f66a6b413d62", "admin@email.com", true, false, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEMw822zMGK4mqYy63blq3q2ek1mBN+QbTuXj040G32G4/CvETHA9IgKHbkkqOdrPfw==", null, false, "1a84dd4c-8130-45b6-b284-c99b176aae7f", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "57bc3893-07a8-4043-a460-4da563f8624f", "019b4122-60ba-4fd4-a82c-627c421d8679" });
        }
    }
}
