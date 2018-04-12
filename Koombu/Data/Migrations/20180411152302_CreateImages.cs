using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Koombu.Data.Migrations
{
    public partial class CreateImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "Images",
                 columns: table => new
                 {
                     Id = table.Column<string>(maxLength: 256, nullable: false),
                     Url = table.Column<string>(nullable: false),
                     PostId = table.Column<string>(maxLength: 256, nullable: false),
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_Images", c => c.Id);
                     table.ForeignKey(
                         name: "FK_Images_Posts_PostId",
                         column: x => x.PostId,
                         principalTable: "Posts",
                         principalColumn: "Id",
                         onDelete: ReferentialAction.Cascade);
                 }
             );

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_PostId",
                table: "UserLikes",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikes");
        }
    }
}
