using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Infra.Data.Migrations
{
    public partial class CreateDisCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullAddress",
                table: "Addresses",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DisCountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisCountCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Percent = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    UsabaleCount = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DisCountId);
                });

            migrationBuilder.CreateTable(
                name: "UserDiscounts",
                columns: table => new
                {
                    UD_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DisCountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiscounts", x => x.UD_Id);
                    table.ForeignKey(
                        name: "FK_UserDiscounts_Discounts_DisCountId",
                        column: x => x.DisCountId,
                        principalTable: "Discounts",
                        principalColumn: "DisCountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDiscounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_DisCountId",
                table: "UserDiscounts",
                column: "DisCountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiscounts_UserId",
                table: "UserDiscounts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDiscounts");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.AlterColumn<string>(
                name: "FullAddress",
                table: "Addresses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800);
        }
    }
}
