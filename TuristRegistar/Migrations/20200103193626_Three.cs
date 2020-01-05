using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class Three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountriesId", "Name" },
                values: new object[] { 13, 5, "Skoplje" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountriesId", "Name" },
                values: new object[] { 14, 5, "Ohrid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountriesId", "Name" },
                values: new object[] { 10, 5, "Skoplje" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountriesId", "Name" },
                values: new object[] { 11, 5, "Ohrid" });
        }
    }
}
