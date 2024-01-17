using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeAndDescToIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Companies",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AspNetUsers",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AspNetRoles",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetRoles",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e24e99e7-00e4-4007-a042-565eac12d96d"),
                columns: new[] { "Code", "Description" },
                values: new object[] { "Role1", "RoleDescription" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f51a5d58-ff38-4563-9d32-f658ef2b40d0"),
                columns: new[] { "Code", "Description" },
                values: new object[] { "Role2", "RoleDescription" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "Code", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "USR02", "d22d41ec-6dcd-4407-aaa1-dcdee94dc7a4", "AQAAAAIAAYagAAAAENtL76hfjxVVqMzyYs2TzMHGGwZ7LvhoE2SRci3uk7AZSyQLFpA8is9VkaUqq2kR7g==", "6bc7b617-b645-45a8-a168-73eceeb18af4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "Code", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "USR01", "e1e78e12-f634-4f3e-8973-675452023739", "AQAAAAIAAYagAAAAEFVj9ae9qPDDKSly+xKcHHzLOwncZc2VKk6w1R/+xn6qzfD3zG0jDNiEk/y7OCm/uw==", "6c411693-c21c-4321-845b-d56b7babcf1a" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("0ebae1bf-6610-4967-a8ed-b149219caf68"),
                column: "Code",
                value: "");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("221e8b2c-59d5-4e5b-b010-86c239b66738"),
                column: "Code",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6e54d3e3-90d0-4604-91b4-77009cedd760"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "30488dce-e77f-46ae-a8ae-d4de6b8e9c50", "AQAAAAIAAYagAAAAEJPe7ZGdLThqWaVJMx/9oHusPokw64Qy7AKCuYexqhyZlQkuIfzPZmDKsd30Xe+Tyw==", "164e422d-2819-41bf-b1ab-6df551a643e0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8bfc75b-2ac3-47e2-b013-8b8a1efba45d"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c87ae64d-8a58-44ec-9664-07a27b5036a6", "AQAAAAIAAYagAAAAEFR0Zq+vs0ppRv182KVwHJA8fQ1RBPHQphbXOTlIMdLXZaXB5H6RGNTtw9SsBJUaqQ==", "46fa1f43-03fb-4855-93dc-3e533308a56c" });
        }
    }
}
