using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class seedtypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ObjectTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Apartman" },
                    { 2, "Hotelska soba" },
                    { 3, "Kuća" },
                    { 4, "Vikendica" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ObjectTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ObjectTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ObjectTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ObjectTypes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
