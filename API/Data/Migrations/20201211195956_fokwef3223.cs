using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class fokwef3223 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CLASS_LEVEL_BOAP",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnrollmentStartTerm",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentStartYear",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CLASS_LEVEL_BOAP",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EnrollmentStartTerm",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EnrollmentStartYear",
                table: "AspNetUsers");
        }
    }
}
