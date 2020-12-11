using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Data.Migrations
{
    public partial class SemesterAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Term = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TA = table.Column<string>(type: "text", nullable: true),
                    FullCourseDescription = table.Column<string>(type: "text", nullable: true),
                    SemesterId = table.Column<int>(type: "integer", nullable: true),
                    CourseTitle = table.Column<string>(type: "text", nullable: true),
                    College = table.Column<string>(type: "text", nullable: true),
                    Department = table.Column<string>(type: "text", nullable: true),
                    CRN = table.Column<string>(type: "text", nullable: true),
                    Building = table.Column<string>(type: "text", nullable: true),
                    ClassFormat = table.Column<string>(type: "text", nullable: true),
                    ClassMeetingTimes = table.Column<string>(type: "text", nullable: true),
                    BeginTime1 = table.Column<int>(type: "integer", nullable: false),
                    EndTime1 = table.Column<int>(type: "integer", nullable: false),
                    BeginTime2 = table.Column<int>(type: "integer", nullable: false),
                    EndTime2 = table.Column<int>(type: "integer", nullable: false),
                    BeginTime3 = table.Column<int>(type: "integer", nullable: false),
                    EndTime3 = table.Column<int>(type: "integer", nullable: false),
                    CourseStatus = table.Column<string>(type: "text", nullable: true),
                    CourseCredits = table.Column<int>(type: "integer", nullable: false),
                    WithdrawlDropDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Prerequisites = table.Column<string>(type: "text", nullable: true),
                    RequiredSeqNum = table.Column<int>(type: "integer", nullable: false),
                    Site = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppUserId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseStudents_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudents_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    CourseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_AppUserId",
                table: "CourseStudents",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudents_CourseId",
                table: "CourseStudents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_CourseId",
                table: "Instructors",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseStudents");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Semesters");
        }
    }
}
