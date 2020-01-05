using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class SeedSomeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "ObjectAttributes",
                columns: new[] { "Id", "Name", "ObjectsId" },
                values: new object[,]
                {
                    { 1, "Wi-Fi", null },
                    { 2, "Grijanje", null },
                    { 3, "Doručak", null },
                    { 4, "Mini-bar", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ObjectAttributes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ObjectAttributes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ObjectAttributes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ObjectAttributes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountriesId", "Name" },
                values: new object[] { 13, 5, "Skoplje" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountriesId", "Name" },
                values: new object[] { 14, 5, "Ohrid" });
        }
    }
}
