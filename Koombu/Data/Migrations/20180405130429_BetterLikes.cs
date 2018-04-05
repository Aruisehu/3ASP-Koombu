using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Koombu.Data.Migrations
{
    public partial class BetterLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows",
                column: "FollowerId");
            

            migrationBuilder.CreateTable(
                name: "UserLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 256, nullable: false),
                    PostId = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikes", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_UserLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikes_Users_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
