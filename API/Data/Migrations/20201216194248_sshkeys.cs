using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class sshkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrivateKeySSH1",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivateKeySSH2",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKeySSH1",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicKeySSH2",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateKeySSH1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrivateKeySSH2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicKeySSH1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicKeySSH2",
                table: "AspNetUsers");
        }
    }
}
