using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Sso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotAdminRoleAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Code", "CompanyId", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b"), "Role3", new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"), null, "RoleDescription", "NotAdmin", "NOTADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea4e032c-53a6-4813-85e4-5b2794fb74be", "AQAAAAIAAYagAAAAEMdANce2goQO2RsMV/OuRA/Z4CXqGJOPa93FofvGgTlnpkFJVllhHUVh3gTcd3SRLQ==", "af9bb84c-7597-40b9-bc59-66a85b7735a9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "437863ec-af59-4efa-9968-361310f9e8dd", "AQAAAAIAAYagAAAAELnfbR9d48u1qTkAzBZBjFJbv7qvV021fxuwXh7o3MSlccEXsz60HfkDozO0nJsINg==", "c8592cdf-bcf2-4f3b-add4-3ec4905cb6d1" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Code", "CompanyId", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsAdmin", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"), 0, "USR03", new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"), "6d43abea-fd1a-4696-8850-77968b1add67", "test3@happisoft.com", true, false, false, null, "Name3", "TEST3@HAPPISOFT.COM", "TESTUSER3", "AQAAAAIAAYagAAAAECLYcfkXfyBQCor7YvrRzbPsvyDlRV0E4ZuwwPtUSMZNiWLIkND7fgcmU+F01dt4XQ==", null, false, "80ae1d82-5cd3-49ae-b29f-d47b4cc27d3a", "Surname3", false, "TestUser3" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 19, "Permission", "Coupons.View", new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b"), new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b"), new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4d0f77eb-2ad5-4b43-848e-826cd32d684b"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2c0e21cf-4845-4b0f-a653-6b7c414af2f9"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "caabc0be-72bd-4009-bf4e-7fd3471c5219", "AQAAAAIAAYagAAAAEAj8NuIX+Sox2GMnWk3tM04bHNRf44cFDdCOZDTHExQcKGj1qAJj52+4TbatebXcyg==", "d0d1d672-799d-4aeb-b85c-47bbf0658d82" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ebc26434-783d-4c87-8478-f49a9dd98416", "AQAAAAIAAYagAAAAEIqeG4y0QLG/FsTLaUuZodeQ0GXwBHXGkCjwofTL+7EdPEiaaT8UUU6XedtRBDfDnQ==", "ea60d710-8348-4749-bfa9-dde29bcbf6ba" });
        }
    }
}
