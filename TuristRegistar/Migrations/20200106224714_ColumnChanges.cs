using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class ColumnChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDaysOffer",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "MaxOccupancy",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "MinDaysOffer",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "MinOccupancy",
                table: "Objects");

            migrationBuilder.AddColumn<int>(
                name: "MaxDaysOffer",
                table: "StandardPricingModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinDaysOffer",
                table: "StandardPricingModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxDaysOffer",
                table: "OccupancyBasedPricings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinDaysOffer",
                table: "OccupancyBasedPricings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDaysOffer",
                table: "StandardPricingModels");

            migrationBuilder.DropColumn(
                name: "MinDaysOffer",
                table: "StandardPricingModels");

            migrationBuilder.DropColumn(
                name: "MaxDaysOffer",
                table: "OccupancyBasedPricings");

            migrationBuilder.DropColumn(
                name: "MinDaysOffer",
                table: "OccupancyBasedPricings");

            migrationBuilder.AddColumn<int>(
                name: "MaxDaysOffer",
                table: "Objects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxOccupancy",
                table: "Objects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinDaysOffer",
                table: "Objects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinOccupancy",
                table: "Objects",
                nullable: false,
                defaultValue: 0);
        }
    }
}
