using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class ObjectHasAttrTableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectAttributes_Objects_ObjectsId",
                table: "ObjectAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_OccupancyBasedPrices_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "OccupancyBasedPrices");

            migrationBuilder.DropIndex(
                name: "IX_OccupancyBasedPrices_OccupancyBasedPricingId",
                table: "OccupancyBasedPrices");

            migrationBuilder.DropIndex(
                name: "IX_ObjectAttributes_ObjectsId",
                table: "ObjectAttributes");

            migrationBuilder.DropColumn(
                name: "OccupancyBasedPricingId",
                table: "OccupancyBasedPrices");

            migrationBuilder.DropColumn(
                name: "ObjectsId",
                table: "ObjectAttributes");

            migrationBuilder.AddColumn<int>(
                name: "OccunapncyBasedPricingId",
                table: "OccupancyBasedPrices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ObjectHasAttributes",
                columns: table => new
                {
                    ObjectId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectHasAttributes", x => new { x.ObjectId, x.AttributeId });
                    table.UniqueConstraint("AK_ObjectHasAttributes_AttributeId_ObjectId", x => new { x.AttributeId, x.ObjectId });
                    table.ForeignKey(
                        name: "FK_ObjectHasAttributes_ObjectAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "ObjectAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectHasAttributes_Objects_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OccupancyBasedPrices_OccunapncyBasedPricingId",
                table: "OccupancyBasedPrices",
                column: "OccunapncyBasedPricingId");

            migrationBuilder.AddForeignKey(
                name: "FK_OccupancyBasedPrices_OccupancyBasedPricings_OccunapncyBasedPricingId",
                table: "OccupancyBasedPrices",
                column: "OccunapncyBasedPricingId",
                principalTable: "OccupancyBasedPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OccupancyBasedPrices_OccupancyBasedPricings_OccunapncyBasedPricingId",
                table: "OccupancyBasedPrices");

            migrationBuilder.DropTable(
                name: "ObjectHasAttributes");

            migrationBuilder.DropIndex(
                name: "IX_OccupancyBasedPrices_OccunapncyBasedPricingId",
                table: "OccupancyBasedPrices");

            migrationBuilder.DropColumn(
                name: "OccunapncyBasedPricingId",
                table: "OccupancyBasedPrices");

            migrationBuilder.AddColumn<int>(
                name: "OccupancyBasedPricingId",
                table: "OccupancyBasedPrices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObjectsId",
                table: "ObjectAttributes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OccupancyBasedPrices_OccupancyBasedPricingId",
                table: "OccupancyBasedPrices",
                column: "OccupancyBasedPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectAttributes_ObjectsId",
                table: "ObjectAttributes",
                column: "ObjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectAttributes_Objects_ObjectsId",
                table: "ObjectAttributes",
                column: "ObjectsId",
                principalTable: "Objects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OccupancyBasedPrices_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "OccupancyBasedPrices",
                column: "OccupancyBasedPricingId",
                principalTable: "OccupancyBasedPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
