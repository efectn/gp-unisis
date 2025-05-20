using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_unisis.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "CourseScheduleEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleEntries_SemesterId",
                table: "CourseScheduleEntries",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseScheduleEntries_Semesters_SemesterId",
                table: "CourseScheduleEntries",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseScheduleEntries_Semesters_SemesterId",
                table: "CourseScheduleEntries");

            migrationBuilder.DropIndex(
                name: "IX_CourseScheduleEntries_SemesterId",
                table: "CourseScheduleEntries");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "CourseScheduleEntries");
        }
    }
}
