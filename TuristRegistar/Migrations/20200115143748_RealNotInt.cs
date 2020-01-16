using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class RealNotInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objects_OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_StandardPricingModelId",
                table: "Objects");

            migrationBuilder.AlterColumn<float>(
                name: "StandardPricePerNight",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_OccupancyBasedPricingId",
                table: "Objects",
                column: "OccupancyBasedPricingId",
                unique: true,
                filter: "[OccupancyBasedPricingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_StandardPricingModelId",
                table: "Objects",
                column: "StandardPricingModelId",
                unique: true,
                filter: "[StandardPricingModelId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objects_OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_StandardPricingModelId",
                table: "Objects");

            migrationBuilder.AlterColumn<int>(
                name: "StandardPricePerNight",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_OccupancyBasedPricingId",
                table: "Objects",
                column: "OccupancyBasedPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_StandardPricingModelId",
                table: "Objects",
                column: "StandardPricingModelId");
        }
    }
}
