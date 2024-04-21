using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Sso.Infrastructure.Migrations.IdentitySagaDatabase
{
    /// <inheritdoc />
    public partial class AddNameSurnameCodeCompanyIdToUserCreateSaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "UserCreationSagaState",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "UserCreationSagaState",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserCreationSagaState",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "UserCreationSagaState",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "UserCreationSagaState");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "UserCreationSagaState");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserCreationSagaState");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "UserCreationSagaState");
        }
    }
}
