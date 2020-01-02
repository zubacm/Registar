using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class ColumnChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Objects");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Objects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StandardPricingM",
                table: "Objects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "StandardPricingM",
                table: "Objects");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Objects",
                nullable: false,
                defaultValue: 0);
        }
    }
}
