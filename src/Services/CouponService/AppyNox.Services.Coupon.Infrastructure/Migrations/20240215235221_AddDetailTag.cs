using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CouponDetailTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    CouponDetailEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponDetailTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponDetailTags_CouponDetails_CouponDetailEntityId",
                        column: x => x.CouponDetailEntityId,
                        principalTable: "CouponDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CouponDetailTags",
                columns: new[] { "Id", "CouponDetailEntityId", "Tag" },
                values: new object[] { new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"), new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"), "Tag Description" });

            migrationBuilder.CreateIndex(
                name: "IX_CouponDetailTags_CouponDetailEntityId",
                table: "CouponDetailTags",
                column: "CouponDetailEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponDetailTags");
        }
    }
}
