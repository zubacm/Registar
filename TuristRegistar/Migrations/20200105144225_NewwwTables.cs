using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class NewwwTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CountableObjectAttributes_Objects_ObjectsId",
                table: "CountableObjectAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectOffers_SpecialOffers_SpecialOfferId",
                table: "ObjectOffers");

            migrationBuilder.DropTable(
                name: "SpecialOffers");

            migrationBuilder.DropIndex(
                name: "IX_ObjectOffers_SpecialOfferId",
                table: "ObjectOffers");

            migrationBuilder.DropIndex(
                name: "IX_CountableObjectAttributes_ObjectsId",
                table: "CountableObjectAttributes");

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

            migrationBuilder.DropColumn(
                name: "Count",
                table: "CountableObjectAttributes");

            migrationBuilder.DropColumn(
                name: "ObjectsId",
                table: "CountableObjectAttributes");

            migrationBuilder.AddColumn<int>(
                name: "SpecialOffersPricesObjectId",
                table: "ObjectOffers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialOffersPricesSpecialOfferId",
                table: "ObjectOffers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CntObjAttributesCount",
                columns: table => new
                {
                    CountableObjAttrId = table.Column<int>(nullable: false),
                    ObjectId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CntObjAttributesCount", x => new { x.CountableObjAttrId, x.ObjectId });
                    table.ForeignKey(
                        name: "FK_CntObjAttributesCount_CountableObjectAttributes_CountableObjAttrId",
                        column: x => x.CountableObjAttrId,
                        principalTable: "CountableObjectAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CntObjAttributesCount_Objects_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialOffersPrices",
                columns: table => new
                {
                    SpecialOfferId = table.Column<int>(nullable: false),
                    ObjectId = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialOffersPrices", x => new { x.SpecialOfferId, x.ObjectId });
                    table.UniqueConstraint("AK_SpecialOffersPrices_ObjectId_SpecialOfferId", x => new { x.ObjectId, x.SpecialOfferId });
                    table.ForeignKey(
                        name: "FK_SpecialOffersPrices_Objects_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialOffersPrices_ObjectAttributes_SpecialOfferId",
                        column: x => x.SpecialOfferId,
                        principalTable: "ObjectAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjectOffers_SpecialOffersPricesSpecialOfferId_SpecialOffersPricesObjectId",
                table: "ObjectOffers",
                columns: new[] { "SpecialOffersPricesSpecialOfferId", "SpecialOffersPricesObjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_CntObjAttributesCount_ObjectId",
                table: "CntObjAttributesCount",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectOffers_SpecialOffersPrices_SpecialOffersPricesSpecialOfferId_SpecialOffersPricesObjectId",
                table: "ObjectOffers",
                columns: new[] { "SpecialOffersPricesSpecialOfferId", "SpecialOffersPricesObjectId" },
                principalTable: "SpecialOffersPrices",
                principalColumns: new[] { "SpecialOfferId", "ObjectId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectOffers_SpecialOffersPrices_SpecialOffersPricesSpecialOfferId_SpecialOffersPricesObjectId",
                table: "ObjectOffers");

            migrationBuilder.DropTable(
                name: "CntObjAttributesCount");

            migrationBuilder.DropTable(
                name: "SpecialOffersPrices");

            migrationBuilder.DropIndex(
                name: "IX_ObjectOffers_SpecialOffersPricesSpecialOfferId_SpecialOffersPricesObjectId",
                table: "ObjectOffers");

            migrationBuilder.DropColumn(
                name: "SpecialOffersPricesObjectId",
                table: "ObjectOffers");

            migrationBuilder.DropColumn(
                name: "SpecialOffersPricesSpecialOfferId",
                table: "ObjectOffers");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "CountableObjectAttributes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ObjectsId",
                table: "CountableObjectAttributes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpecialOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ObjectsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialOffers_Objects_ObjectsId",
                        column: x => x.ObjectsId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ObjectOffers_SpecialOfferId",
                table: "ObjectOffers",
                column: "SpecialOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_CountableObjectAttributes_ObjectsId",
                table: "CountableObjectAttributes",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOffers_ObjectsId",
                table: "SpecialOffers",
                column: "ObjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CountableObjectAttributes_Objects_ObjectsId",
                table: "CountableObjectAttributes",
                column: "ObjectsId",
                principalTable: "Objects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectOffers_SpecialOffers_SpecialOfferId",
                table: "ObjectOffers",
                column: "SpecialOfferId",
                principalTable: "SpecialOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
