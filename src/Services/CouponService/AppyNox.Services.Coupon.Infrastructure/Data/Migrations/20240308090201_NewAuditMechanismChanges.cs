using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppyNox.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewAuditMechanismChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                table: "Coupons",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdateDate",
                table: "Coupons",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreationDate",
                table: "Coupons",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                table: "Coupons",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                table: "CouponDetailTags",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdateDate",
                table: "CouponDetailTags",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreationDate",
                table: "CouponDetailTags",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                table: "CouponDetailTags",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                table: "CouponDetails",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdateDate",
                table: "CouponDetails",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreationDate",
                table: "CouponDetails",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                table: "CouponDetails",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Coupons",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "CouponDetailTags",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "CouponDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                columns: new[] { "CreatedBy", "CreationDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 2, 0, 337, DateTimeKind.Utc).AddTicks(7507), null });

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                columns: new[] { "CreatedBy", "CreationDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 2, 0, 336, DateTimeKind.Utc).AddTicks(1596), null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                columns: new[] { "CreatedBy", "CreationDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 2, 0, 335, DateTimeKind.Utc).AddTicks(1598), null });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                columns: new[] { "CreatedBy", "CreationDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 2, 0, 335, DateTimeKind.Utc).AddTicks(1813), null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreatedBy", "CreationDate", "ReportDate", "UpdatedBy" },
                values: new object[] { "System", new DateTime(2024, 3, 8, 9, 2, 0, 337, DateTimeKind.Utc).AddTicks(9278), new DateTime(2024, 3, 8, 9, 2, 0, 337, DateTimeKind.Utc).AddTicks(9278), null });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreationDate",
                table: "Tickets",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UpdateDate",
                table: "Tickets",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CreationDate",
                table: "Coupons",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_UpdateDate",
                table: "Coupons",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_CouponDetailTags_CreationDate",
                table: "CouponDetailTags",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_CouponDetailTags_UpdateDate",
                table: "CouponDetailTags",
                column: "UpdateDate");

            migrationBuilder.CreateIndex(
                name: "IX_CouponDetails_CreationDate",
                table: "CouponDetails",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_CouponDetails_UpdateDate",
                table: "CouponDetails",
                column: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_CreationDate",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_UpdateDate",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_CreationDate",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_UpdateDate",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_CouponDetailTags_CreationDate",
                table: "CouponDetailTags");

            migrationBuilder.DropIndex(
                name: "IX_CouponDetailTags_UpdateDate",
                table: "CouponDetailTags");

            migrationBuilder.DropIndex(
                name: "IX_CouponDetails_CreationDate",
                table: "CouponDetails");

            migrationBuilder.DropIndex(
                name: "IX_CouponDetails_UpdateDate",
                table: "CouponDetails");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Coupons",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Coupons",
                newName: "Audit_UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Coupons",
                newName: "Audit_CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Coupons",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "CouponDetailTags",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "CouponDetailTags",
                newName: "Audit_UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "CouponDetailTags",
                newName: "Audit_CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "CouponDetailTags",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "CouponDetails",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "CouponDetails",
                newName: "Audit_UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "CouponDetails",
                newName: "Audit_CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "CouponDetails",
                newName: "Audit_CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Audit_UpdatedBy",
                table: "Coupons",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Audit_UpdatedBy",
                table: "CouponDetailTags",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Audit_UpdatedBy",
                table: "CouponDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CouponDetailTags",
                keyColumn: "Id",
                keyValue: new Guid("b6bcfe76-83c7-4a4a-b088-13b14751fce8"),
                columns: new[] { "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 29, 9, 47, 1, 158, DateTimeKind.Utc).AddTicks(3365), "" });

            migrationBuilder.UpdateData(
                table: "CouponDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec80532f-58f0-4690-b40c-2133b067d5f2"),
                columns: new[] { "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 29, 9, 47, 1, 152, DateTimeKind.Utc).AddTicks(7121), "" });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("594cf045-3a2b-46f5-99c9-1eb59f035db2"),
                columns: new[] { "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 29, 9, 47, 1, 144, DateTimeKind.Utc).AddTicks(814), "" });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: new Guid("c386aec2-dfd2-4ea5-b878-8fe5632e2e40"),
                columns: new[] { "Audit_CreatedBy", "Audit_CreationDate", "Audit_UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 29, 9, 47, 1, 144, DateTimeKind.Utc).AddTicks(1682), "" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("69472ec0-4da6-4fdd-93cc-b0a529d7f5e0"),
                columns: new[] { "CreatedBy", "CreationDate", "ReportDate", "UpdatedBy" },
                values: new object[] { "admin", new DateTime(2024, 2, 29, 9, 47, 1, 158, DateTimeKind.Utc).AddTicks(5411), new DateTime(2024, 2, 29, 9, 47, 1, 158, DateTimeKind.Utc).AddTicks(5410), "" });
        }
    }
}