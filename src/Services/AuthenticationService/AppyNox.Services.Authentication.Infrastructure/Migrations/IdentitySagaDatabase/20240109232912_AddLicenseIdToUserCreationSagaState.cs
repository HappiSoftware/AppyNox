﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Authentication.Infrastructure.Migrations.IdentitySagaDatabase
{
    /// <inheritdoc />
    public partial class AddLicenseIdToUserCreationSagaState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LicenseId",
                table: "UserCreationSagaState",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "UserCreationSagaState");
        }
    }
}
