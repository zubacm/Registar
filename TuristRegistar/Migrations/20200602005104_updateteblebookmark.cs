using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class updateteblebookmark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_Userss_UserId",
                table: "Bookmark");

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "Bookmark",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_UsersId",
                table: "Bookmark",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_AspNetUsers_UserId",
                table: "Bookmark",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_Userss_UsersId",
                table: "Bookmark",
                column: "UsersId",
                principalTable: "Userss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_AspNetUsers_UserId",
                table: "Bookmark");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_Userss_UsersId",
                table: "Bookmark");

            migrationBuilder.DropIndex(
                name: "IX_Bookmark_UsersId",
                table: "Bookmark");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Bookmark");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_Userss_UserId",
                table: "Bookmark",
                column: "UserId",
                principalTable: "Userss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
