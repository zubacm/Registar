using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class NewForeignKeysInObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Users_CreatorId1",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingsAndReviews_Users_UserId1",
                table: "RatingsAndReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AspNetUsers_IdentUserId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Userss");

            migrationBuilder.RenameIndex(
                name: "IX_Users_IdentUserId",
                table: "Userss",
                newName: "IX_Userss_IdentUserId");

            migrationBuilder.AddColumn<int>(
                name: "OccupancyBasedPricingId",
                table: "Objects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StandardPricingModelId",
                table: "Objects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Userss",
                table: "Userss",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_OccupancyBasedPricingId",
                table: "Objects",
                column: "OccupancyBasedPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_Objects_StandardPricingModelId",
                table: "Objects",
                column: "StandardPricingModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Userss_CreatorId1",
                table: "Objects",
                column: "CreatorId1",
                principalTable: "Userss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_RatingsAndReviews_Userss_UserId1",
                table: "RatingsAndReviews",
                column: "UserId1",
                principalTable: "Userss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Userss_AspNetUsers_IdentUserId",
                table: "Userss",
                column: "IdentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objects_Userss_CreatorId1",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_OccupancyBasedPricings_OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_Objects_StandardPricingModels_StandardPricingModelId",
                table: "Objects");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingsAndReviews_Userss_UserId1",
                table: "RatingsAndReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Userss_AspNetUsers_IdentUserId",
                table: "Userss");

            migrationBuilder.DropIndex(
                name: "IX_Objects_OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropIndex(
                name: "IX_Objects_StandardPricingModelId",
                table: "Objects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Userss",
                table: "Userss");

            migrationBuilder.DropColumn(
                name: "OccupancyBasedPricingId",
                table: "Objects");

            migrationBuilder.DropColumn(
                name: "StandardPricingModelId",
                table: "Objects");

            migrationBuilder.RenameTable(
                name: "Userss",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_Userss_IdentUserId",
                table: "Users",
                newName: "IX_Users_IdentUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objects_Users_CreatorId1",
                table: "Objects",
                column: "CreatorId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingsAndReviews_Users_UserId1",
                table: "RatingsAndReviews",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AspNetUsers_IdentUserId",
                table: "Users",
                column: "IdentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
