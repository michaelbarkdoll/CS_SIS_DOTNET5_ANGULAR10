using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class dockerpaged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContainerHost",
                table: "UserContainers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContainerStatus",
                table: "UserContainers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerHost",
                table: "UserContainers");

            migrationBuilder.DropColumn(
                name: "ContainerStatus",
                table: "UserContainers");
        }
    }
}
