using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class Columnscolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StandardPricingM",
                table: "Objects",
                newName: "OccupancyPricing");

            migrationBuilder.AlterColumn<int>(
                name: "StandardPricePerNight",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "StandardOccupancy",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "OffsetPercentage",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "MinOccupancy",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MinDaysOffer",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MaxOccupancy",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MaxDaysOffer",
                table: "StandardPricingModels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MinOccupancy",
                table: "OccupancyBasedPricings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MinDaysOffer",
                table: "OccupancyBasedPricings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MaxOccupancy",
                table: "OccupancyBasedPricings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MaxDaysOffer",
                table: "OccupancyBasedPricings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "Surface",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<string>(
                name: "IdentUserId",
                table: "Objects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Objects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objects_IdentUserId",
                table: "Objects",
                column: "IdentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_AspNetUsers_IdentUserId",
                table: "Objects",
                column: "IdentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_AspNetUsers_IdentUserId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_IdentUserId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "IdentUserId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Objects");

            migrationBuilder.RenameColumn(
                name: "OccupancyPricing",
                table: "Objects",
                newName: "StandardPricingM");

            migrationBuilder.AlterColumn<float>(
                name: "StandardPricePerNight",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StandardOccupancy",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "OffsetPercentage",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinOccupancy",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinDaysOffer",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxOccupancy",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxDaysOffer",
                table: "StandardPricingModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinOccupancy",
                table: "OccupancyBasedPricings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinDaysOffer",
                table: "OccupancyBasedPricings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxOccupancy",
                table: "OccupancyBasedPricings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxDaysOffer",
                table: "OccupancyBasedPricings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Surface",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
