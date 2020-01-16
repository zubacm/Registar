using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class floattodc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "StandardPricePerNight",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OffsetPercentage",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SpecialOffersPrices",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerNight",
                table: "OccupancyBasedPrices",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "StandardPricePerNight",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "OffsetPercentage",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "SpecialOffersPrices",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "PricePerNight",
                table: "OccupancyBasedPrices",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
