using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Infra.Data.Migrations
{
    public partial class migEditEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IaActive",
                table: "Transportations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "minBuy",
                table: "Transportations",
                type: "int",
                maxLength: 200,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DicPercent",
                table: "Products",
                type: "int",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IaActive",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "minBuy",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "DicPercent",
                table: "Products");
        }
    }
}
