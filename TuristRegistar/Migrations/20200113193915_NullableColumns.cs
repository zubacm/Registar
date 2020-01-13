using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class NullableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Cities_CityId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Counries_CountryId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_StandardPricingModels_StandardPricingModelId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Objects");

            migrationBuilder.AlterColumn<int>(
                name: "StandardPricingModelId",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OccupancyBasedPricingId",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ObejectTypeId",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Objects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Cities_CityId",
                table: "Objects",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Counries_CountryId",
                table: "Objects",
                column: "CountryId",
                principalTable: "Counries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "Objects",
                column: "OccupancyBasedPricingId",
                principalTable: "OccupancyBasedPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_StandardPricingModels_StandardPricingModelId",
                table: "Objects",
                column: "StandardPricingModelId",
                principalTable: "StandardPricingModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Cities_CityId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Counries_CountryId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_StandardPricingModels_StandardPricingModelId",
                table: "Objects");

            migrationBuilder.AlterColumn<int>(
                name: "StandardPricingModelId",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OccupancyBasedPricingId",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ObejectTypeId",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Objects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Objects",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Cities_CityId",
                table: "Objects",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Counries_CountryId",
                table: "Objects",
                column: "CountryId",
                principalTable: "Counries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "Objects",
                column: "OccupancyBasedPricingId",
                principalTable: "OccupancyBasedPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_StandardPricingModels_StandardPricingModelId",
                table: "Objects",
                column: "StandardPricingModelId",
                principalTable: "StandardPricingModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
