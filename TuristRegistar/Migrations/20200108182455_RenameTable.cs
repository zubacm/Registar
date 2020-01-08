using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableTo",
                table: "AvailablePeriods",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "AvailableFrom",
                table: "AvailablePeriods",
                newName: "From");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "To",
                table: "AvailablePeriods",
                newName: "AvailableTo");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "AvailablePeriods",
                newName: "AvailableFrom");
        }
    }
}
