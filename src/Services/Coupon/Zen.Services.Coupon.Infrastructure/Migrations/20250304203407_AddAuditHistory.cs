using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zen.Services.Coupon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Coupons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Coupons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditHistoryRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Snapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditHistoryRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditHistoryRecords_Coupons_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditHistoryRecords_EntityId",
                table: "AuditHistoryRecords",
                column: "EntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditHistoryRecords");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Coupons");
        }
    }
}
