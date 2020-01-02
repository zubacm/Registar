using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class NewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "IdentUserId");

            migrationBuilder.AlterColumn<string>(
                name: "IdentUserId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactAddress",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccupancyBasedPricings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MinOccupancy = table.Column<int>(nullable: false),
                    MaxOccupancy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccupancyBasedPricings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardPricingModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StandardPricePerNight = table.Column<float>(nullable: false),
                    StandardOccupancy = table.Column<int>(nullable: false),
                    MinOccupancy = table.Column<int>(nullable: false),
                    MaxOccupancy = table.Column<int>(nullable: false),
                    OffsetPercentage = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardPricingModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Lat = table.Column<double>(nullable: false),
                    Lng = table.Column<double>(nullable: false),
                    EmailContact = table.Column<string>(maxLength: 256, nullable: true),
                    PhoneNumberContact = table.Column<string>(nullable: true),
                    WebContact = table.Column<string>(nullable: true),
                    NumberOfRooms = table.Column<int>(nullable: false),
                    Surface = table.Column<float>(nullable: false),
                    MinOccupancy = table.Column<int>(nullable: false),
                    MaxOccupancy = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    MinDaysOffer = table.Column<int>(nullable: false),
                    MaxDaysOffer = table.Column<int>(nullable: false),
                    ObejectTypeId = table.Column<int>(nullable: false),
                    ObjectTypeId = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: false),
                    CreatorId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objects_Users_CreatorId1",
                        column: x => x.CreatorId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Objects_ObjectTypes_ObjectTypeId",
                        column: x => x.ObjectTypeId,
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OccupancyBasedPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Occupancy = table.Column<int>(nullable: false),
                    PricePerNight = table.Column<float>(nullable: false),
                    OccupancyBasedPricingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccupancyBasedPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OccupancyBasedPrices_OccupancyBasedPricings_OccupancyBasedPricingId",
                        column: x => x.OccupancyBasedPricingId,
                        principalTable: "OccupancyBasedPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AvailablePeriods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableFrom = table.Column<DateTime>(nullable: false),
                    AvailableTo = table.Column<DateTime>(nullable: false),
                    ObjectsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailablePeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailablePeriods_Objects_ObjectsId",
                        column: x => x.ObjectsId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CountableObjectAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    ObjectsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountableObjectAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountableObjectAttributes_Objects_ObjectsId",
                        column: x => x.ObjectsId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObjectAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ObjectsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectAttributes_Objects_ObjectsId",
                        column: x => x.ObjectsId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObjectImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(nullable: true),
                    IsCover = table.Column<bool>(nullable: false),
                    ObjectsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectImages_Objects_ObjectsId",
                        column: x => x.ObjectsId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RatingsAndReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Rating = table.Column<int>(nullable: false),
                    Review = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true),
                    ObjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingsAndReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingsAndReviews_Objects_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "Objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingsAndReviews_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "ObjectOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SpecialOfferId = table.Column<int>(nullable: false),
                    IncludedInOriginalPrice = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectOffers_SpecialOffers_SpecialOfferId",
                        column: x => x.SpecialOfferId,
                        principalTable: "SpecialOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentUserId",
                table: "Users",
                column: "IdentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailablePeriods_ObjectsId",
                table: "AvailablePeriods",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_CountableObjectAttributes_ObjectsId",
                table: "CountableObjectAttributes",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectAttributes_ObjectsId",
                table: "ObjectAttributes",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectImages_ObjectsId",
                table: "ObjectImages",
                column: "ObjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectOffers_SpecialOfferId",
                table: "ObjectOffers",
                column: "SpecialOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_CreatorId1",
                table: "Objects",
                column: "CreatorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_ObjectTypeId",
                table: "Objects",
                column: "ObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OccupancyBasedPrices_OccupancyBasedPricingId",
                table: "OccupancyBasedPrices",
                column: "OccupancyBasedPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingsAndReviews_ObjectId",
                table: "RatingsAndReviews",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingsAndReviews_UserId1",
                table: "RatingsAndReviews",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOffers_ObjectsId",
                table: "SpecialOffers",
                column: "ObjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_IdentUserId",
                table: "Users",
                column: "IdentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_IdentUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AvailablePeriods");

            migrationBuilder.DropTable(
                name: "CountableObjectAttributes");

            migrationBuilder.DropTable(
                name: "ObjectAttributes");

            migrationBuilder.DropTable(
                name: "ObjectImages");

            migrationBuilder.DropTable(
                name: "ObjectOffers");

            migrationBuilder.DropTable(
                name: "OccupancyBasedPrices");

            migrationBuilder.DropTable(
                name: "RatingsAndReviews");

            migrationBuilder.DropTable(
                name: "StandardPricingModels");

            migrationBuilder.DropTable(
                name: "SpecialOffers");

            migrationBuilder.DropTable(
                name: "OccupancyBasedPricings");

            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.DropTable(
                name: "ObjectTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdentUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContactAddress",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IdentUserId",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 450, nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId",
                unique: true);
        }
    }
}
