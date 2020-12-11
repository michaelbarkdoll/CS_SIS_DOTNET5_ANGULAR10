using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class woepfk33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFile_AspNetUsers_AppUserId",
                table: "UserFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFile",
                table: "UserFile");

            migrationBuilder.RenameTable(
                name: "UserFile",
                newName: "UserFiles");

            migrationBuilder.RenameIndex(
                name: "IX_UserFile_AppUserId",
                table: "UserFiles",
                newName: "IX_UserFiles_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFiles",
                table: "UserFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_AspNetUsers_AppUserId",
                table: "UserFiles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_AspNetUsers_AppUserId",
                table: "UserFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFiles",
                table: "UserFiles");

            migrationBuilder.RenameTable(
                name: "UserFiles",
                newName: "UserFile");

            migrationBuilder.RenameIndex(
                name: "IX_UserFiles_AppUserId",
                table: "UserFile",
                newName: "IX_UserFile_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFile",
                table: "UserFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFile_AspNetUsers_AppUserId",
                table: "UserFile",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
