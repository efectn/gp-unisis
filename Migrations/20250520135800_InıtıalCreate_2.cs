using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_unisis.Migrations
{
    /// <inheritdoc />
    public partial class InıtıalCreate_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Semesters_SemesterId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ExamName",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Grades");

            migrationBuilder.AlterColumn<int>(
                name: "SemesterId",
                table: "Grades",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Grades",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "Grades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Exam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ExamType = table.Column<bool>(type: "INTEGER", nullable: false),
                    examCoefficient = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exam_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exam_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ExamId",
                table: "Grades",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_CourseId",
                table: "Exam",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_SemesterId",
                table: "Exam",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Exam_ExamId",
                table: "Grades",
                column: "ExamId",
                principalTable: "Exam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Semesters_SemesterId",
                table: "Grades",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Exam_ExamId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Semesters_SemesterId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Grades_ExamId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Grades");

            migrationBuilder.AlterColumn<int>(
                name: "SemesterId",
                table: "Grades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Grades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExamName",
                table: "Grades",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                table: "Grades",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Semesters_SemesterId",
                table: "Grades",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
