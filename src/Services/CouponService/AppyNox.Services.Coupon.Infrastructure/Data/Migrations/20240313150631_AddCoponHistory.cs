using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoponHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CouponHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MinimumAmount = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponHistories_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 15, 6, 30, 215, DateTimeKind.Utc).AddTicks(2309));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 15, 6, 30, 209, DateTimeKind.Utc).AddTicks(7471));

            migrationBuilder.InsertData(
                table: "CouponHistories",
                columns: new[] { "Id", "CouponId", "CreatedBy", "CreationDate", "Date", "MinimumAmount", "UpdateDate", "UpdatedBy" },
                values: new object[] { new Guid("7a00866d-8240-4278-a1d0-10a59c7c67f6"), new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"), "System", new DateTime(2024, 3, 13, 15, 6, 30, 217, DateTimeKind.Utc).AddTicks(414), new DateTime(2024, 3, 13, 15, 6, 30, 217, DateTimeKind.Utc).AddTicks(412), 100, null, null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 15, 6, 30, 204, DateTimeKind.Utc).AddTicks(6095));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 13, 15, 6, 30, 204, DateTimeKind.Utc).AddTicks(6576));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreatedBy", "CreationDate", "ReportDate" },
                values: new object[] { "System", new DateTime(2024, 3, 13, 15, 6, 30, 215, DateTimeKind.Utc).AddTicks(8228), new DateTime(2024, 3, 13, 15, 6, 30, 215, DateTimeKind.Utc).AddTicks(8227) });

            migrationBuilder.CreateIndex(
                name: "IX_CouponHistories_CouponId",
                table: "CouponHistories",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponHistories_CreationDate",
                table: "CouponHistories",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_CouponHistories_UpdateDate",
                table: "CouponHistories",
                column: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponHistories");

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 8, 9, 2, 0, 337, DateTimeKind.Utc).AddTicks(7507));

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 8, 9, 2, 0, 336, DateTimeKind.Utc).AddTicks(1596));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 8, 9, 2, 0, 335, DateTimeKind.Utc).AddTicks(1598));

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                column: "CreationDate",
                value: new DateTime(2024, 3, 8, 9, 2, 0, 335, DateTimeKind.Utc).AddTicks(1813));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreatedBy", "CreationDate", "ReportDate" },
                values: new object[] { "system", new DateTime(2024, 3, 8, 9, 2, 0, 337, DateTimeKind.Utc).AddTicks(9278), new DateTime(2024, 3, 8, 9, 2, 0, 337, DateTimeKind.Utc).AddTicks(9278) });
        }
    }
}
