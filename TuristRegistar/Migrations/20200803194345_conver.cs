using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TuristRegistar.Migrations
{
    public partial class conver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Counries_CountriesId",
                table: "Cities");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "3", null });

            migrationBuilder.AlterColumn<int>(
                name: "CountriesId",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentUser1Id = table.Column<string>(nullable: true),
                    IdentUser2Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_IdentUser1Id",
                        column: x => x.IdentUser1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_IdentUser2Id",
                        column: x => x.IdentUser2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConversationId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    SendingIdentUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SendingIdentUserId",
                        column: x => x.SendingIdentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_IdentUser1Id",
                table: "Conversations",
                column: "IdentUser1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_IdentUser2Id",
                table: "Conversations",
                column: "IdentUser2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendingIdentUserId",
                table: "Messages",
                column: "SendingIdentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Counries_CountriesId",
                table: "Cities",
                column: "CountriesId",
                principalTable: "Counries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Counries_CountriesId",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.AlterColumn<int>(
                name: "CountriesId",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3", null, "BANNED", "BANNED" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Counries_CountriesId",
                table: "Cities",
                column: "CountriesId",
                principalTable: "Counries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
