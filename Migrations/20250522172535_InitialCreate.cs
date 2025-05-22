using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace gp_unisis.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Dean = table.Column<string>(type: "TEXT", nullable: false),
                    ViceDean = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinalExamDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Head = table.Column<string>(type: "TEXT", nullable: false),
                    ViceHead = table.Column<string>(type: "TEXT", nullable: false),
                    FacultyId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveSemesterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturers_Semesters_ActiveSemesterId",
                        column: x => x.ActiveSemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentPersonals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveSemesterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPersonals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentPersonals_Semesters_ActiveSemesterId",
                        column: x => x.ActiveSemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RequiredCredits = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredCoursesCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    EntranceSemesterId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseGroups_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroups_Semesters_EntranceSemesterId",
                        column: x => x.EntranceSemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    StudentNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    NationalId = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsGraduated = table.Column<bool>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveSemesterId = table.Column<int>(type: "INTEGER", nullable: true),
                    EntranceSemesterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Semesters_ActiveSemesterId",
                        column: x => x.ActiveSemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Semesters_EntranceSemesterId",
                        column: x => x.EntranceSemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Credit = table.Column<int>(type: "INTEGER", nullable: false),
                    SemesterNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Quota = table.Column<int>(type: "INTEGER", nullable: false),
                    IsElective = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    LecturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentLecturer",
                columns: table => new
                {
                    DepartmentsId = table.Column<int>(type: "INTEGER", nullable: false),
                    LecturersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentLecturer", x => new { x.DepartmentsId, x.LecturersId });
                    table.ForeignKey(
                        name: "FK_DepartmentLecturer_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentLecturer_Lecturers_LecturersId",
                        column: x => x.LecturersId,
                        principalTable: "Lecturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cancelled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseSelections_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourseSelections_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdminId = table.Column<int>(type: "INTEGER", nullable: true),
                    StudentPersonalId = table.Column<int>(type: "INTEGER", nullable: true),
                    LecturerId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_StudentPersonals_StudentPersonalId",
                        column: x => x.StudentPersonalId,
                        principalTable: "StudentPersonals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseCourseGroup",
                columns: table => new
                {
                    CourseGroupsId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoursesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCourseGroup", x => new { x.CourseGroupsId, x.CoursesId });
                    table.ForeignKey(
                        name: "FK_CourseCourseGroup_CourseGroups_CourseGroupsId",
                        column: x => x.CourseGroupsId,
                        principalTable: "CourseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCourseGroup_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLetterGradeIntervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBellCurve = table.Column<bool>(type: "INTEGER", nullable: false),
                    AAStart = table.Column<double>(type: "REAL", nullable: false),
                    AAEnd = table.Column<double>(type: "REAL", nullable: false),
                    BAStart = table.Column<double>(type: "REAL", nullable: false),
                    BAEnd = table.Column<double>(type: "REAL", nullable: false),
                    BBStart = table.Column<double>(type: "REAL", nullable: false),
                    BBEnd = table.Column<double>(type: "REAL", nullable: false),
                    CBStart = table.Column<double>(type: "REAL", nullable: false),
                    CBEnd = table.Column<double>(type: "REAL", nullable: false),
                    CCStart = table.Column<double>(type: "REAL", nullable: false),
                    CCEnd = table.Column<double>(type: "REAL", nullable: false),
                    DCStart = table.Column<double>(type: "REAL", nullable: false),
                    DCEnd = table.Column<double>(type: "REAL", nullable: false),
                    DDStart = table.Column<double>(type: "REAL", nullable: false),
                    DDEnd = table.Column<double>(type: "REAL", nullable: false),
                    FDStart = table.Column<double>(type: "REAL", nullable: false),
                    FDEnd = table.Column<double>(type: "REAL", nullable: false),
                    Average = table.Column<double>(type: "REAL", nullable: false),
                    Stdev = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLetterGradeIntervals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseLetterGradeIntervals_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLetterGradeIntervals_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseScheduleEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Day = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseScheduleEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseScheduleEntries_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseScheduleEntries_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseSemester",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "INTEGER", nullable: false),
                    SemestersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSemester", x => new { x.CoursesId, x.SemestersId });
                    table.ForeignKey(
                        name: "FK_CourseSemester_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSemester_Semesters_SemestersId",
                        column: x => x.SemestersId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ExamCoefficient = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsExamCalculated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exams_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transcripts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseName = table.Column<string>(type: "TEXT", nullable: false),
                    CourseCode = table.Column<string>(type: "TEXT", nullable: false),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: false),
                    LetterGrade = table.Column<double>(type: "REAL", nullable: false),
                    HasFailed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transcripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transcripts_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transcripts_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transcripts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudentCourseSelection",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentCourseSelectionsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudentCourseSelection", x => new { x.CoursesId, x.StudentCourseSelectionsId });
                    table.ForeignKey(
                        name: "FK_CourseStudentCourseSelection_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudentCourseSelection_StudentCourseSelections_StudentCourseSelectionsId",
                        column: x => x.StudentCourseSelectionsId,
                        principalTable: "StudentCourseSelections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: true),
                    SemesterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Grades_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { 1, "efe@efe.com", "Efe Çetin", "827ccb0eea8a706c4c34a16891f84e7b" });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "Id", "Address", "ContactNumber", "Dean", "Name", "ViceDean" },
                values: new object[,]
                {
                    { 1, "Uludağ Üniversitesi", "(0555) 555-55-55", "Efe", "Mühendislik Fakültesi", "Celil" },
                    { 2, "Uludağ Üniversitesi", "(0555) 555-55-56", "Ali", "Tıp Fakültesi", "Ayşe" },
                    { 3, "Uludağ Üniversitesi", "(0555) 555-55-57", "Mehmet", "İktisadi ve İdari Bilimler Fakültesi", "Fatma" },
                    { 4, "Uludağ Üniversitesi", "(0555) 555-55-58", "Ahmet", "Eğitim Fakültesi", "Zeynep" }
                });

            migrationBuilder.InsertData(
                table: "Lecturers",
                columns: new[] { "Id", "ActiveSemesterId", "Email", "FullName", "Password" },
                values: new object[,]
                {
                    { 1, null, "efe@efe.com", "Efe Çetin", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 2, null, "ali@ali.com", "Ali Yılmaz", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 3, null, "mehm543et@mehet.com", "Mehmet Demir", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 4, null, "fatma@fatma.com", "Fatma Demir", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 5, null, "ayse@ayse.com", "Ayşe Demir", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 6, null, "atreli@ali.com", "Ali Demir", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 7, null, "muhittin@muhittin.com", "Muhittin Demir", "827ccb0eea8a706c4c34a16891f84e7b" },
                    { 8, null, "muhittin2s@muhittin.com", "Muhittin Demir2", "827ccb0eea8a706c4c34a16891f84e7b" }
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "Id", "EndDate", "FinalExamDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "2023-2024 Güz Dönemi", new DateTime(2023, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "2023-2024 Bahar Dönemi", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "2024-2025 Güz Dönemi", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "2024-2025 Bahar Dönemi", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "StudentPersonals",
                columns: new[] { "Id", "ActiveSemesterId", "Email", "Name", "Password" },
                values: new object[] { 1, null, "efe@efe.com", "Efe Çetin", "827ccb0eea8a706c4c34a16891f84e7b" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Address", "ContactNumber", "FacultyId", "Head", "Name", "ViceHead" },
                values: new object[,]
                {
                    { 1, "Uludağ Üniversitesi", "(0555) 555-55-55", 1, "Efe", "Bilgisayar Mühendisliği", "Celil" },
                    { 2, "Uludağ Üniversitesi", "(0555) 555-55-56", 1, "Ali", "Elektrik Mühendisliği", "Ayşe" },
                    { 3, "Uludağ Üniversitesi", "(0555) 555-55-57", 2, "Mehmet", "Tıp", "Fatma" },
                    { 4, "Uludağ Üniversitesi", "(0555) 555-55-58", 3, "Ahmet", "İşletme", "Zeynep" },
                    { 5, "Uludağ Üniversitesi", "(0555) 555-55-59", 3, "Ayşe", "İktisat", "Fatma" },
                    { 6, "Uludağ Üniversitesi", "(0555) 555-55-59", 4, "Ayşe", "Matematik Öğretmenliği", "Fatma" },
                    { 7, "Uludağ Üniversitesi", "(0555) 555-55-59", 4, "Ayşe", "Fen Bilgisi Öğretmenliği", "Fatma" },
                    { 8, "Uludağ Üniversitesi", "(0555) 555-55-59", 4, "Ayşe", "Kimya Öğretmenliği", "Fatma" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Code", "Credit", "DepartmentId", "Description", "IsConfirmed", "IsElective", "LecturerId", "Name", "Quota", "SemesterNumber" },
                values: new object[,]
                {
                    { 1, "ECON-359", 4, 4, "A harum quasi. Deleniti rerum labore illo quis odit ut. Atque qui omnis. İllum accusantium officia ad voluptas quia ab at. Autem totam assumenda in non. Mollitia aliquid aut.", true, true, 5, "Physics for Teachers", 87, 8 },
                    { 2, "PHYS-175", 4, 3, "Necessitatibus alias esse suscipit facilis vel eos commodi. İncidunt expedita velit laudantium quisquam ea quis quia. Laudantium veritatis sed repellat repellendus et nihil. Sit velit tempora dolores quia delectus in cumque. Assumenda omnis vitae ut eos quis qui pariatur assumenda et. Recusandae itaque eum et est.", true, false, 2, "Control Systems", 99, 4 },
                    { 3, "MATH-106", 5, 3, "Ut doloremque aut quia eos sapiente facilis corrupti quo. Labore velit itaque similique. Sapiente blanditiis sit sapiente quo officia eveniet qui autem tempora. Perferendis autem architecto. Voluptate sunt praesentium omnis.", true, false, 2, "History of Science", 28, 4 },
                    { 4, "MATH-EDU-436", 4, 7, "Tenetur consequatur voluptatum magnam et et. Voluptas repellat nulla tenetur perferendis illo at eveniet beatae tempora. İmpedit sunt recusandae.", true, false, 4, "Electromagnetic Theory", 93, 3 },
                    { 5, "PSY-249", 6, 6, "Possimus dolores tenetur. İtaque in corporis. Eum suscipit voluptates adipisci consectetur quia temporibus omnis. Aut molestiae qui dolor est optio a et et dolorem. Debitis sit pariatur quod odio. İd totam voluptas expedita quod.", true, true, 5, "Laboratory Techniques in Science Education", 67, 3 },
                    { 6, "EDU-343", 1, 5, "Magnam odio provident in molestias omnis quia. Exercitationem omnis quisquam est minima sint at omnis dolor est. Et suscipit minima sit inventore inventore.", true, true, 6, "Computer Networks", 25, 3 },
                    { 7, "CHEM-EDU-158", 4, 2, "Vitae consectetur autem et aut officia saepe unde architecto temporibus. Provident aliquid expedita. Dolor illo velit est. Similique qui ipsa ea eum officiis.", true, true, 2, "Signal Processing", 59, 7 },
                    { 8, "CHEM-EDU-503", 4, 3, "Rerum placeat ut rerum ut fuga et rem sed quo. Et dolores atque pariatur asperiores ut accusamus cum neque qui. Aperiam animi sed consequuntur beatae deserunt. Ut est voluptatum voluptas illo aut. Voluptatem porro accusantium.", true, false, 1, "Signal Processing", 66, 2 },
                    { 9, "PHYS-104", 6, 7, "Maiores ea omnis sequi. Accusantium asperiores voluptatum sequi enim deleniti aut at. Laborum eum sint accusantium.", true, true, 7, "General Physics II", 68, 1 },
                    { 10, "EE-301", 4, 3, "Quos quasi sit sapiente. Natus autem pariatur. Eaque porro corrupti laudantium culpa placeat aliquam consequatur cupiditate. Officiis sapiente pariatur est. Ducimus eius debitis itaque sit aut cum voluptatibus aliquam. Sunt vitae ducimus nihil vitae eius autem facilis.", true, false, 1, "Mathematics Education", 83, 2 },
                    { 11, "EE-372", 2, 2, "Laboriosam quod tenetur et. Molestiae impedit velit eligendi labore. Hic aliquid at aut vitae odit fugit. Assumenda nam sunt inventore corrupti assumenda. Rerum molestias in. İn exercitationem ullam dignissimos.", true, false, 3, "Chemistry for Teachers", 93, 6 },
                    { 12, "MATH-EDU-575", 6, 2, "Atque sed quo quia voluptatibus ea ratione beatae. Doloribus sit possimus aut excepturi rerum. Aperiam ea nisi dolorum aut omnis eum. Tempora et a. Quasi velit non qui ex. Vitae magni veritatis accusantium enim reprehenderit possimus nesciunt.", true, true, 5, "Inorganic Chemistry for Teachers", 64, 6 },
                    { 13, "SCI-EDU-105", 3, 4, "Maiores ipsam at sit aliquid sed. Fugiat rerum totam facere porro voluptatibus officia distinctio. Quibusdam ipsum quo vel nam ea eum. Dolorem excepturi sint omnis eum voluptatum qui ea ducimus atque. İpsa et quis. İtaque possimus veritatis dolores rerum unde voluptatem neque quod vitae.", true, false, 5, "Engineering Ethics", 64, 2 },
                    { 14, "PHYS-EDU-144", 1, 1, "Non nostrum esse. Possimus qui dicta et et architecto eius. Aut inventore labore consequuntur id voluptas voluptatum dolor.", false, false, 2, "Calculus I", 53, 6 },
                    { 15, "ECON-389", 2, 4, "Sed suscipit autem accusantium suscipit molestiae laborum accusamus cupiditate. Ullam deleniti aut vero quidem. Quae autem quos laboriosam fugit laboriosam inventore. Adipisci aut eum sequi est.", true, false, 3, "Technical Drawing", 21, 6 },
                    { 16, "HIST-119", 2, 3, "Non repudiandae ipsam nesciunt id facere voluptatibus vel non possimus. Nesciunt doloribus aut eos ullam nostrum eos cupiditate delectus. Sunt veniam temporibus quibusdam et qui adipisci. Cum excepturi aut voluptatum facilis iusto maiores soluta praesentium quibusdam. İpsum et corporis qui labore nemo.", true, false, 6, "Circuit Theory", 65, 8 },
                    { 17, "HIST-237", 1, 5, "Possimus et et non est officia provident quia dolor molestiae. Sunt ea incidunt sint. Quibusdam iusto porro. Et porro et.", true, false, 6, "Database Systems", 74, 8 },
                    { 18, "EE-487", 6, 2, "Voluptas ea vitae dolorum consectetur placeat et. Veritatis doloremque eveniet magnam dolor ipsa blanditiis. Est et est eligendi quo voluptatem est. Aut quae dolorem facilis voluptatem quia in. Quia minima sapiente autem odit deserunt vero deserunt. Maiores repudiandae voluptas.", false, false, 2, "General Chemistry I", 82, 7 },
                    { 19, "ART-559", 5, 5, "Quas fuga mollitia deserunt. Voluptatem sunt voluptate cum aut quis veniam molestias accusamus. Non dicta eum rerum aperiam esse. İd quisquam cum rerum. Quisquam labore eveniet sit ut quis.", true, false, 4, "Database Systems", 20, 5 },
                    { 20, "CHEM-396", 4, 3, "Exercitationem nihil non natus vel eos qui deleniti. Et repudiandae est quos totam. Harum illo enim est nihil qui. Ab corporis non. Ullam modi cupiditate quidem.", true, false, 2, "Macroeconomics", 80, 2 },
                    { 21, "MATH-EDU-519", 4, 1, "Quos odio voluptatem iure rerum et. Eligendi fugit in doloremque natus amet perspiciatis quidem dolorem. Dolorum aut aut delectus. Fugiat similique aliquam rerum ipsum quae aliquid beatae deserunt. Dolores et maiores impedit repudiandae quod ea illo repellendus. Ex sit vitae est est quis.", true, false, 4, "International Economics", 80, 4 },
                    { 22, "SCI-EDU-161", 3, 7, "İtaque expedita ea officia rem. Quia cum accusantium dolore sint doloribus quasi. Necessitatibus et aut qui repellendus. Qui voluptatem libero.", true, false, 1, "Human-Computer Interaction", 34, 8 },
                    { 23, "PHYS-EDU-456", 6, 7, "Vitae consequatur distinctio at. Quia et corrupti. Est consequatur velit eligendi id molestias ut sed laborum sequi.", true, false, 3, "Control Systems", 29, 1 },
                    { 24, "BIO-454", 2, 7, "Consequuntur voluptatem possimus id hic dolorum. Est quaerat consectetur. Quia suscipit architecto adipisci in et. Velit rerum deserunt repellat laborum eveniet aspernatur omnis qui totam. Soluta provident ea.", true, false, 2, "Teaching Chemistry in Secondary Schools", 22, 2 },
                    { 25, "PHYS-363", 6, 4, "Quidem dolores ipsam. Quia ut voluptatibus consequatur. Odio omnis adipisci et sequi placeat mollitia.", true, false, 6, "Econometrics", 71, 5 },
                    { 26, "PHYS-597", 4, 2, "A hic qui aut veniam quia. Eligendi et autem esse. Perferendis eum est quod.", true, true, 7, "Computer Graphics", 35, 1 },
                    { 27, "ENG-116", 3, 4, "Dolorum iste excepturi totam et. Veritatis sint suscipit cupiditate. Mollitia vel laudantium et repudiandae dolores labore facere nulla ut. Atque repellendus eveniet enim aut ullam.", true, false, 5, "Development Economics", 25, 3 },
                    { 28, "ELEC-506", 5, 7, "Dolorem ut laborum. Excepturi dicta autem nesciunt omnis hic. Quia porro aliquid est quam qui velit consequatur. Tempora aut pariatur optio et ducimus et cum.", true, true, 2, "Biochemistry", 71, 2 },
                    { 29, "ELEC-210", 5, 8, "Non qui qui necessitatibus aut voluptatem necessitatibus necessitatibus mollitia. Culpa sint reprehenderit deserunt eveniet est libero. Temporibus porro dignissimos fugiat quos sed autem amet.", true, false, 2, "Introduction to Programming", 47, 6 },
                    { 30, "PSY-485", 2, 8, "Possimus illum sunt. İpsa asperiores id hic accusamus consequatur. Eius explicabo cum nobis qui ut tempora molestias sed ratione. Aperiam voluptas et sit. Perspiciatis dolor qui veritatis magni dolor.", true, true, 6, "Probability and Statistics for Educators", 41, 8 },
                    { 31, "PHYS-EDU-405", 2, 7, "Consequatur tempore necessitatibus quia vitae et inventore. Aut quia consequatur voluptas expedita. Quod fugiat dolores. Sed iusto voluptatibus ducimus cupiditate consequatur possimus rem.", true, false, 5, "Calculus II", 34, 2 },
                    { 32, "PHYS-244", 3, 6, "Enim qui omnis temporibus laboriosam exercitationem alias placeat rem fugit. Ut odio est voluptatem officia repellat. Expedita tempora sint sequi consequatur esse rerum explicabo necessitatibus. İllo deserunt qui dolores rerum officiis dolores similique.", true, false, 7, "Signal Processing", 62, 5 },
                    { 33, "EDU-289", 1, 3, "Voluptate eius non et sequi voluptatem occaecati non explicabo. Rerum consequuntur consequatur velit quo soluta. Vel beatae dicta fugiat. Voluptatibus inventore fugit deleniti eum eos.", true, true, 6, "International Economics", 76, 4 },
                    { 34, "MATH-EDU-553", 5, 1, "Nihil expedita et provident id recusandae et est aut ipsam. İn perferendis nemo voluptate magni repudiandae. Consequatur et aut tempora aliquid nihil quam. Ut quis reprehenderit quam. Quasi aut minus impedit soluta.", true, true, 5, "General Chemistry I", 27, 1 },
                    { 35, "CHEM-EDU-163", 5, 7, "Et illo eveniet voluptates dolorum expedita et. Qui assumenda magnam alias sit animi. Quia omnis dolores quo id inventore omnis quos illum omnis. Maiores sapiente tempore ratione omnis harum molestias sed doloribus.", true, false, 7, "General Physics I", 67, 7 },
                    { 36, "SCI-EDU-547", 2, 7, "Porro libero qui. Et fugiat autem nemo sit nesciunt eum maxime harum. Dolor tenetur quos. Quo distinctio ipsam soluta vero aliquid ut quibusdam explicabo. Totam error sed et aut quod ea vel id.", true, true, 1, "Geometry for Teachers", 27, 8 },
                    { 37, "ENG-160", 5, 7, "Porro ut explicabo voluptas perferendis dignissimos voluptatibus necessitatibus minima dolorum. Sed et quibusdam enim mollitia sed et dolor et. Molestias odio sunt perspiciatis sunt quibusdam.", true, false, 7, "Technical Drawing", 53, 3 },
                    { 38, "CS-557", 4, 5, "Architecto quia magni. Qui maiores sequi eos. İnventore libero officia ducimus ad nobis id soluta illum. Sunt accusantium et quia sint. Deserunt quia aspernatur non. Sint qui tenetur nam.", true, false, 5, "Computer Graphics", 63, 2 },
                    { 39, "PSY-488", 1, 8, "Deleniti a eligendi eius dolor minima non earum blanditiis. Error optio id eaque recusandae perspiciatis totam. Velit quae enim quo error dolores delectus et sed. Molestiae dolorem eveniet.", true, false, 2, "Teaching Mathematics in Secondary Schools", 38, 1 },
                    { 40, "EDU-274", 1, 4, "Distinctio quia quo distinctio a quos aut minima. Ut neque ut non. Rerum consequatur accusamus totam quasi est non voluptatum. Alias alias inventore. Nemo et eum commodi et et eos et reprehenderit. Ullam vitae est est doloremque assumenda reiciendis porro tempore officiis.", true, false, 2, "Telecommunication Systems", 99, 6 },
                    { 41, "EE-431", 2, 1, "Vitae placeat qui ea totam qui. Sed voluptatem qui quis. İd rerum eum cupiditate officiis. Maxime dolore modi est dolore quisquam non sapiente quas et. Blanditiis dolore non quaerat nisi quam minima modi expedita.", false, true, 6, "Astronomy for Teachers", 30, 4 },
                    { 42, "ART-222", 4, 3, "Est quisquam voluptate. Aut aut voluptatem. Dolorum corrupti voluptas officia quis nemo qui voluptatibus accusantium. Qui aut omnis earum explicabo quia temporibus.", true, true, 4, "Laboratory Safety in Chemistry Education", 87, 3 },
                    { 43, "ART-327", 2, 5, "Est numquam dicta minus pariatur. Rem totam cupiditate laboriosam consequatur quo omnis. Natus aliquid et aut. Ad minus cupiditate quis mollitia est eveniet.", true, false, 6, "History of Science", 46, 8 },
                    { 44, "ECON-591", 1, 4, "Quo doloremque ut atque ut perspiciatis aut sit illum ab. Et cumque id commodi sit quaerat possimus dolores. Nisi sint neque quasi quas sint illo amet quos. Fuga dolorem ullam ut quo.", true, true, 1, "Introduction to Programming", 55, 8 },
                    { 45, "CS-364", 1, 6, "Sit adipisci sunt dolore labore sequi ullam. Quia quisquam nemo sed consequatur. Eligendi assumenda dolores alias.", true, false, 2, "Astronomy for Teachers", 48, 7 },
                    { 46, "ELEC-256", 6, 2, "Unde dolorem non doloremque similique est sed dolorum. Consequatur totam eligendi velit fuga aut harum. Autem corporis eos autem voluptate inventore sequi inventore eum deleniti. Distinctio possimus ipsam occaecati voluptates consectetur. Magnam hic hic quo accusantium adipisci tempora at eos. Est adipisci voluptatibus repudiandae dolores dolores sequi voluptatem iure.", true, true, 1, "Microeconomics", 55, 7 },
                    { 47, "ENG-175", 5, 2, "Facilis quo doloremque. Eum nesciunt eum molestiae quibusdam a. Expedita non vitae.", true, true, 3, "Telecommunication Systems", 92, 6 },
                    { 48, "PHYS-267", 1, 8, "Quam est repellat ipsam sit blanditiis amet animi. Exercitationem aut necessitatibus placeat aperiam debitis. Ea quis id quod rerum. Sint molestiae eligendi qui. At omnis molestiae ratione blanditiis blanditiis sint. Velit quis doloremque voluptatum rerum totam.", true, true, 5, "Inorganic Chemistry for Teachers", 77, 7 },
                    { 49, "MATH-EDU-358", 6, 5, "Perspiciatis delectus et praesentium praesentium quibusdam et reiciendis occaecati. Occaecati reprehenderit laborum qui et ex est odit. Et cum quos voluptas consequuntur sit sunt ut.", true, true, 3, "Circuit Theory", 70, 8 },
                    { 50, "ENG-257", 2, 5, "İmpedit debitis non eum sit cumque autem voluptas magni. İnventore eius pariatur similique a. Voluptatem et perspiciatis iste. İste eligendi ipsa iste quo sit ut ab eius non. Quis quod laudantium omnis.", true, false, 2, "Teaching Chemistry in Secondary Schools", 68, 5 }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ActiveSemesterId", "DateOfBirth", "DepartmentId", "Email", "EntranceSemesterId", "FirstName", "IsGraduated", "LastName", "NationalId", "Password", "StudentNumber" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1988, 1, 26, 13, 0, 25, 116, DateTimeKind.Unspecified).AddTicks(2478), 2, "joannie52@gmail.com", null, "Joannie", false, "Considine", "7446679054855", "827ccb0eea8a706c4c34a16891f84e7b", "A3TGV5RLG" },
                    { 2, null, new DateTime(1984, 12, 17, 10, 18, 43, 215, DateTimeKind.Unspecified).AddTicks(2226), 4, "edmond.beier61@hotmail.com", null, "Edmond", false, "Beier", "1288092521691", "827ccb0eea8a706c4c34a16891f84e7b", "XPHPVOQQ9" },
                    { 3, null, new DateTime(1994, 3, 29, 23, 35, 55, 420, DateTimeKind.Unspecified).AddTicks(2623), 3, "creola43@hotmail.com", null, "Creola", false, "Morar", "1272652437897", "827ccb0eea8a706c4c34a16891f84e7b", "TDİNVDLJ4" },
                    { 4, null, new DateTime(2003, 6, 26, 11, 0, 30, 740, DateTimeKind.Unspecified).AddTicks(4211), 7, "wilfredo53@yahoo.com", null, "Wilfredo", false, "Renner", "4082721686680", "827ccb0eea8a706c4c34a16891f84e7b", "A2LZ8CQNS" },
                    { 5, null, new DateTime(1986, 1, 15, 22, 53, 47, 698, DateTimeKind.Unspecified).AddTicks(2191), 8, "vella.kemmer7@yahoo.com", null, "Vella", false, "Kemmer", "4009962091932", "827ccb0eea8a706c4c34a16891f84e7b", "F62SWAEW1" },
                    { 6, null, new DateTime(1986, 6, 14, 12, 25, 44, 682, DateTimeKind.Unspecified).AddTicks(6122), 8, "lacey16@yahoo.com", null, "Lacey", false, "Bogisich", "5704882537973", "827ccb0eea8a706c4c34a16891f84e7b", "S3742D12X" },
                    { 7, null, new DateTime(1991, 8, 15, 19, 22, 25, 565, DateTimeKind.Unspecified).AddTicks(6230), 5, "alison84@yahoo.com", null, "Alison", false, "Medhurst", "9375356522391", "827ccb0eea8a706c4c34a16891f84e7b", "OBNGW2537" },
                    { 8, null, new DateTime(1999, 5, 28, 4, 44, 14, 607, DateTimeKind.Unspecified).AddTicks(4529), 8, "gay79@yahoo.com", null, "Gay", false, "Wyman", "9792248437479", "827ccb0eea8a706c4c34a16891f84e7b", "NOU7DWTLF" },
                    { 9, null, new DateTime(2001, 1, 18, 17, 51, 8, 484, DateTimeKind.Unspecified).AddTicks(296), 2, "reynold_wilkinson62@hotmail.com", null, "Reynold", false, "Wilkinson", "8692222990576", "827ccb0eea8a706c4c34a16891f84e7b", "CSCLFDZU3" },
                    { 10, null, new DateTime(1981, 9, 15, 1, 26, 55, 492, DateTimeKind.Unspecified).AddTicks(1190), 6, "dangelo_durgan@yahoo.com", null, "Dangelo", false, "Durgan", "7832694349740", "827ccb0eea8a706c4c34a16891f84e7b", "HYF1VVV30" },
                    { 11, null, new DateTime(1980, 3, 16, 21, 18, 10, 231, DateTimeKind.Unspecified).AddTicks(4632), 3, "deron54@yahoo.com", null, "Deron", false, "Spencer", "8061462358999", "827ccb0eea8a706c4c34a16891f84e7b", "JNNB3Q3AZ" },
                    { 12, null, new DateTime(1981, 4, 24, 14, 45, 46, 889, DateTimeKind.Unspecified).AddTicks(6788), 7, "keven_kris@hotmail.com", null, "Keven", false, "Kris", "2932948668704", "827ccb0eea8a706c4c34a16891f84e7b", "LRİ7E9PV1" },
                    { 13, null, new DateTime(1994, 12, 11, 8, 19, 40, 623, DateTimeKind.Unspecified).AddTicks(3854), 8, "nikolas.wehner31@hotmail.com", null, "Nikolas", false, "Wehner", "5388176644485", "827ccb0eea8a706c4c34a16891f84e7b", "MUOKJVİ49" },
                    { 14, null, new DateTime(1991, 6, 30, 0, 22, 7, 738, DateTimeKind.Unspecified).AddTicks(7211), 2, "dorcas83@hotmail.com", null, "Dorcas", false, "Torphy", "5178117068603", "827ccb0eea8a706c4c34a16891f84e7b", "J2XVH99W0" },
                    { 15, null, new DateTime(1981, 7, 14, 2, 38, 25, 199, DateTimeKind.Unspecified).AddTicks(9058), 8, "ımogene55@hotmail.com", null, "Imogene", false, "Lemke", "8225583158015", "827ccb0eea8a706c4c34a16891f84e7b", "5EEB4L0TO" },
                    { 16, null, new DateTime(2001, 2, 3, 17, 11, 50, 98, DateTimeKind.Unspecified).AddTicks(4084), 2, "alivia.koch1@yahoo.com", null, "Alivia", false, "Koch", "9607483465428", "827ccb0eea8a706c4c34a16891f84e7b", "3MM1EİGAC" },
                    { 17, null, new DateTime(1999, 7, 9, 3, 39, 29, 331, DateTimeKind.Unspecified).AddTicks(3690), 6, "amos_emard@hotmail.com", null, "Amos", false, "Emard", "6258825864449", "827ccb0eea8a706c4c34a16891f84e7b", "CC51VX00M" },
                    { 18, null, new DateTime(1993, 7, 3, 22, 28, 19, 825, DateTimeKind.Unspecified).AddTicks(3266), 8, "robb_kub@yahoo.com", null, "Robb", false, "Kub", "4669628522420", "827ccb0eea8a706c4c34a16891f84e7b", "321U8CN2Z" },
                    { 19, null, new DateTime(2001, 9, 10, 6, 58, 35, 340, DateTimeKind.Unspecified).AddTicks(1093), 1, "emil_franecki@gmail.com", null, "Emil", false, "Franecki", "3956919639118", "827ccb0eea8a706c4c34a16891f84e7b", "4KDT1ACPF" },
                    { 20, null, new DateTime(2005, 2, 21, 16, 34, 4, 569, DateTimeKind.Unspecified).AddTicks(7216), 5, "martine24@yahoo.com", null, "Martine", false, "Schimmel", "0873864422394", "827ccb0eea8a706c4c34a16891f84e7b", "LM8S4BBLB" },
                    { 21, null, new DateTime(1995, 7, 20, 15, 30, 35, 768, DateTimeKind.Unspecified).AddTicks(3598), 8, "wilson_johns80@yahoo.com", null, "Wilson", false, "Johns", "6750097379973", "827ccb0eea8a706c4c34a16891f84e7b", "FWWC6ORCO" },
                    { 22, null, new DateTime(1984, 8, 8, 19, 42, 57, 660, DateTimeKind.Unspecified).AddTicks(5723), 2, "jedediah43@hotmail.com", null, "Jedediah", false, "Koepp", "8262801366521", "827ccb0eea8a706c4c34a16891f84e7b", "COJK2XOİL" },
                    { 23, null, new DateTime(2000, 12, 19, 5, 52, 39, 682, DateTimeKind.Unspecified).AddTicks(814), 2, "paxton_zemlak@yahoo.com", null, "Paxton", false, "Zemlak", "0535956636554", "827ccb0eea8a706c4c34a16891f84e7b", "15D044QA6" },
                    { 24, null, new DateTime(1993, 1, 8, 11, 11, 53, 984, DateTimeKind.Unspecified).AddTicks(6372), 4, "johanna24@gmail.com", null, "Johanna", false, "Turcotte", "8655317661559", "827ccb0eea8a706c4c34a16891f84e7b", "MFROJDH4F" },
                    { 25, null, new DateTime(1994, 10, 3, 19, 59, 12, 187, DateTimeKind.Unspecified).AddTicks(4200), 5, "cassandra35@hotmail.com", null, "Cassandra", false, "Windler", "5764574317071", "827ccb0eea8a706c4c34a16891f84e7b", "DYBM3750Z" },
                    { 26, null, new DateTime(2005, 6, 23, 14, 49, 25, 947, DateTimeKind.Unspecified).AddTicks(606), 5, "constantin_paucek47@yahoo.com", null, "Constantin", false, "Paucek", "8035561508987", "827ccb0eea8a706c4c34a16891f84e7b", "9NBKMOVA8" },
                    { 27, null, new DateTime(1991, 10, 29, 2, 2, 52, 31, DateTimeKind.Unspecified).AddTicks(1898), 3, "jordy.nolan@gmail.com", null, "Jordy", false, "Nolan", "0923329299236", "827ccb0eea8a706c4c34a16891f84e7b", "AHY6BD53G" },
                    { 28, null, new DateTime(1991, 10, 22, 7, 45, 27, 26, DateTimeKind.Unspecified).AddTicks(4500), 8, "raymond.bauch@gmail.com", null, "Raymond", false, "Bauch", "2711259555226", "827ccb0eea8a706c4c34a16891f84e7b", "M3YJCT7CW" },
                    { 29, null, new DateTime(1992, 4, 30, 5, 52, 9, 192, DateTimeKind.Unspecified).AddTicks(6660), 1, "kenton30@hotmail.com", null, "Kenton", false, "Lakin", "0109484686929", "827ccb0eea8a706c4c34a16891f84e7b", "OMCVBKNSQ" },
                    { 30, null, new DateTime(1981, 5, 12, 3, 32, 19, 660, DateTimeKind.Unspecified).AddTicks(3952), 3, "clarissa85@hotmail.com", null, "Clarissa", false, "Hamill", "6521307624720", "827ccb0eea8a706c4c34a16891f84e7b", "CRF065DPE" },
                    { 31, null, new DateTime(1985, 8, 8, 15, 1, 38, 450, DateTimeKind.Unspecified).AddTicks(5007), 5, "garth.quigley18@hotmail.com", null, "Garth", false, "Quigley", "6409436409038", "827ccb0eea8a706c4c34a16891f84e7b", "49MNZ49ST" },
                    { 32, null, new DateTime(2004, 2, 15, 16, 49, 50, 841, DateTimeKind.Unspecified).AddTicks(2721), 3, "nova.wunsch49@yahoo.com", null, "Nova", false, "Wunsch", "8207962109378", "827ccb0eea8a706c4c34a16891f84e7b", "9WM87TİT0" },
                    { 33, null, new DateTime(1988, 5, 31, 1, 31, 1, 253, DateTimeKind.Unspecified).AddTicks(6888), 2, "cleveland_ullrich@hotmail.com", null, "Cleveland", false, "Ullrich", "0762487007347", "827ccb0eea8a706c4c34a16891f84e7b", "SW0OU2W84" },
                    { 34, null, new DateTime(1995, 11, 13, 19, 42, 34, 100, DateTimeKind.Unspecified).AddTicks(2939), 5, "emelia.harber@gmail.com", null, "Emelia", false, "Harber", "1561758354213", "827ccb0eea8a706c4c34a16891f84e7b", "176CB8ZJT" },
                    { 35, null, new DateTime(1996, 11, 28, 11, 42, 23, 841, DateTimeKind.Unspecified).AddTicks(794), 7, "emmie_hauck55@hotmail.com", null, "Emmie", false, "Hauck", "7144383100877", "827ccb0eea8a706c4c34a16891f84e7b", "B3RM960CO" },
                    { 36, null, new DateTime(2001, 10, 29, 15, 59, 19, 43, DateTimeKind.Unspecified).AddTicks(6971), 4, "nella.hoeger@hotmail.com", null, "Nella", false, "Hoeger", "7285045638278", "827ccb0eea8a706c4c34a16891f84e7b", "WN2NUA2HP" },
                    { 37, null, new DateTime(1982, 3, 27, 12, 40, 44, 579, DateTimeKind.Unspecified).AddTicks(2473), 2, "judd19@yahoo.com", null, "Judd", false, "Johnston", "0680154431073", "827ccb0eea8a706c4c34a16891f84e7b", "CTYGY0JJJ" },
                    { 38, null, new DateTime(1999, 7, 19, 21, 30, 7, 347, DateTimeKind.Unspecified).AddTicks(6560), 2, "estevan51@gmail.com", null, "Estevan", false, "Roob", "5006546317936", "827ccb0eea8a706c4c34a16891f84e7b", "610AATQ29" },
                    { 39, null, new DateTime(1985, 1, 6, 3, 15, 40, 344, DateTimeKind.Unspecified).AddTicks(1149), 5, "josue9@gmail.com", null, "Josue", false, "Huels", "0008162079692", "827ccb0eea8a706c4c34a16891f84e7b", "MSOKNWQ39" },
                    { 40, null, new DateTime(1997, 5, 25, 7, 53, 57, 689, DateTimeKind.Unspecified).AddTicks(4580), 6, "autumn.turner98@gmail.com", null, "Autumn", false, "Turner", "1057243763027", "827ccb0eea8a706c4c34a16891f84e7b", "6ZWMZK4QA" },
                    { 41, null, new DateTime(1983, 9, 21, 13, 45, 35, 972, DateTimeKind.Unspecified).AddTicks(9302), 1, "carrie_reynolds26@yahoo.com", null, "Carrie", false, "Reynolds", "7612993014390", "827ccb0eea8a706c4c34a16891f84e7b", "CY1NQİDNG" },
                    { 42, null, new DateTime(2003, 1, 17, 14, 1, 46, 672, DateTimeKind.Unspecified).AddTicks(6198), 2, "ıla98@gmail.com", null, "Ila", false, "Rodriguez", "8926203301702", "827ccb0eea8a706c4c34a16891f84e7b", "YC9XMVEQ3" },
                    { 43, null, new DateTime(1997, 6, 29, 8, 24, 13, 124, DateTimeKind.Unspecified).AddTicks(8609), 7, "sylvan_koss@gmail.com", null, "Sylvan", false, "Koss", "7315315067154", "827ccb0eea8a706c4c34a16891f84e7b", "X03K4U8JJ" },
                    { 44, null, new DateTime(1986, 6, 9, 22, 51, 42, 381, DateTimeKind.Unspecified).AddTicks(8755), 1, "jadyn11@hotmail.com", null, "Jadyn", false, "Wintheiser", "0519632090115", "827ccb0eea8a706c4c34a16891f84e7b", "CQKLSB7W6" },
                    { 45, null, new DateTime(1994, 6, 4, 7, 5, 18, 221, DateTimeKind.Unspecified).AddTicks(5698), 3, "pamela.oconnell@hotmail.com", null, "Pamela", false, "O'Connell", "0760679371226", "827ccb0eea8a706c4c34a16891f84e7b", "0DAPK99İ7" },
                    { 46, null, new DateTime(1990, 9, 24, 4, 10, 34, 254, DateTimeKind.Unspecified).AddTicks(6359), 7, "shanna_watsica@gmail.com", null, "Shanna", false, "Watsica", "6621414591020", "827ccb0eea8a706c4c34a16891f84e7b", "NF2RYT8GU" },
                    { 47, null, new DateTime(1987, 2, 15, 6, 43, 8, 605, DateTimeKind.Unspecified).AddTicks(7802), 4, "ıvy_roob51@hotmail.com", null, "Ivy", false, "Roob", "6669204722395", "827ccb0eea8a706c4c34a16891f84e7b", "LUN8Q6611" },
                    { 48, null, new DateTime(2000, 6, 4, 1, 41, 4, 779, DateTimeKind.Unspecified).AddTicks(3020), 7, "zula_konopelski32@hotmail.com", null, "Zula", false, "Konopelski", "5593763516737", "827ccb0eea8a706c4c34a16891f84e7b", "EA6PGHNA5" },
                    { 49, null, new DateTime(1991, 11, 26, 5, 21, 14, 868, DateTimeKind.Unspecified).AddTicks(8619), 1, "nedra81@yahoo.com", null, "Nedra", false, "Hills", "3985475804064", "827ccb0eea8a706c4c34a16891f84e7b", "6D7İZKQHB" },
                    { 50, null, new DateTime(1992, 7, 10, 20, 14, 5, 961, DateTimeKind.Unspecified).AddTicks(8884), 7, "major_carroll40@yahoo.com", null, "Major", false, "Carroll", "8808758033896", "827ccb0eea8a706c4c34a16891f84e7b", "İJJ1ZQ22C" },
                    { 51, null, new DateTime(1991, 8, 11, 15, 0, 5, 206, DateTimeKind.Unspecified).AddTicks(9818), 5, "hilton_schmitt@hotmail.com", null, "Hilton", false, "Schmitt", "4237499554375", "827ccb0eea8a706c4c34a16891f84e7b", "AYGPX76SS" },
                    { 52, null, new DateTime(2004, 12, 27, 9, 0, 50, 189, DateTimeKind.Unspecified).AddTicks(8948), 6, "ben.batz86@gmail.com", null, "Ben", false, "Batz", "4161823043411", "827ccb0eea8a706c4c34a16891f84e7b", "9WQ53UCAT" },
                    { 53, null, new DateTime(1981, 3, 5, 19, 3, 48, 193, DateTimeKind.Unspecified).AddTicks(4424), 2, "noble_funk@hotmail.com", null, "Noble", false, "Funk", "8673277467375", "827ccb0eea8a706c4c34a16891f84e7b", "3NY9F9DV6" },
                    { 54, null, new DateTime(1983, 2, 18, 15, 36, 32, 582, DateTimeKind.Unspecified).AddTicks(2193), 1, "myron_dickens38@gmail.com", null, "Myron", false, "Dickens", "1095350440607", "827ccb0eea8a706c4c34a16891f84e7b", "T3RZNCCAM" },
                    { 55, null, new DateTime(1999, 3, 12, 12, 59, 21, 305, DateTimeKind.Unspecified).AddTicks(2173), 3, "kathleen92@yahoo.com", null, "Kathleen", false, "Parker", "9131189029153", "827ccb0eea8a706c4c34a16891f84e7b", "N77AHHQ2A" },
                    { 56, null, new DateTime(1984, 12, 23, 9, 1, 53, 827, DateTimeKind.Unspecified).AddTicks(3280), 7, "ımmanuel55@hotmail.com", null, "Immanuel", false, "Smitham", "8890200580779", "827ccb0eea8a706c4c34a16891f84e7b", "LLRVZPFS2" },
                    { 57, null, new DateTime(1997, 1, 24, 14, 21, 57, 68, DateTimeKind.Unspecified).AddTicks(8331), 8, "jerrod61@hotmail.com", null, "Jerrod", false, "Shields", "2743949166379", "827ccb0eea8a706c4c34a16891f84e7b", "GG8RTQKVA" },
                    { 58, null, new DateTime(1997, 8, 1, 14, 42, 20, 914, DateTimeKind.Unspecified).AddTicks(7057), 1, "rigoberto.williamson45@yahoo.com", null, "Rigoberto", false, "Williamson", "9723544760670", "827ccb0eea8a706c4c34a16891f84e7b", "AWE3VTNZV" },
                    { 59, null, new DateTime(1989, 7, 22, 18, 24, 27, 89, DateTimeKind.Unspecified).AddTicks(1442), 8, "hazel93@gmail.com", null, "Hazel", false, "Marvin", "2188149126075", "827ccb0eea8a706c4c34a16891f84e7b", "ADFRYXXBA" },
                    { 60, null, new DateTime(1992, 7, 2, 21, 54, 52, 764, DateTimeKind.Unspecified).AddTicks(7366), 8, "nora.tremblay55@yahoo.com", null, "Nora", false, "Tremblay", "8853880786015", "827ccb0eea8a706c4c34a16891f84e7b", "59NF9LDWB" },
                    { 61, null, new DateTime(2000, 5, 18, 10, 52, 22, 244, DateTimeKind.Unspecified).AddTicks(3851), 2, "earlene75@gmail.com", null, "Earlene", false, "Gorczany", "4754232057083", "827ccb0eea8a706c4c34a16891f84e7b", "5VU4K4DVP" },
                    { 62, null, new DateTime(1996, 4, 3, 19, 6, 43, 930, DateTimeKind.Unspecified).AddTicks(2162), 3, "tatyana59@hotmail.com", null, "Tatyana", false, "Miller", "4443297658305", "827ccb0eea8a706c4c34a16891f84e7b", "VAEYNEON5" },
                    { 63, null, new DateTime(1992, 2, 7, 22, 44, 3, 118, DateTimeKind.Unspecified).AddTicks(1956), 5, "nadia90@yahoo.com", null, "Nadia", false, "Paucek", "9432542266516", "827ccb0eea8a706c4c34a16891f84e7b", "Z1GRHMM7X" },
                    { 64, null, new DateTime(1993, 10, 15, 19, 3, 41, 107, DateTimeKind.Unspecified).AddTicks(5886), 8, "frankie_hand60@hotmail.com", null, "Frankie", false, "Hand", "9073470432483", "827ccb0eea8a706c4c34a16891f84e7b", "UZ5NA8İ56" },
                    { 65, null, new DateTime(1987, 3, 26, 20, 19, 47, 400, DateTimeKind.Unspecified).AddTicks(2196), 5, "kallie.smitham@yahoo.com", null, "Kallie", false, "Smitham", "6832780666262", "827ccb0eea8a706c4c34a16891f84e7b", "ONUTQİ38K" },
                    { 66, null, new DateTime(1985, 7, 17, 20, 40, 51, 561, DateTimeKind.Unspecified).AddTicks(3050), 1, "verna.bruen@hotmail.com", null, "Verna", false, "Bruen", "9398612630270", "827ccb0eea8a706c4c34a16891f84e7b", "WRM5531FW" },
                    { 67, null, new DateTime(1997, 9, 2, 13, 27, 0, 720, DateTimeKind.Unspecified).AddTicks(750), 1, "floy.boehm15@yahoo.com", null, "Floy", false, "Boehm", "7960258490766", "827ccb0eea8a706c4c34a16891f84e7b", "C0NFJ7H0C" },
                    { 68, null, new DateTime(1996, 1, 7, 6, 10, 0, 379, DateTimeKind.Unspecified).AddTicks(1571), 3, "marshall73@yahoo.com", null, "Marshall", false, "Kessler", "3475733128799", "827ccb0eea8a706c4c34a16891f84e7b", "GEJEJ7AJX" },
                    { 69, null, new DateTime(1984, 2, 2, 13, 31, 13, 83, DateTimeKind.Unspecified).AddTicks(8032), 3, "marshall_botsford@gmail.com", null, "Marshall", false, "Botsford", "2142061814829", "827ccb0eea8a706c4c34a16891f84e7b", "KPROCT605" },
                    { 70, null, new DateTime(1985, 1, 31, 2, 0, 34, 687, DateTimeKind.Unspecified).AddTicks(4136), 7, "flavio47@gmail.com", null, "Flavio", false, "Hauck", "6682243024012", "827ccb0eea8a706c4c34a16891f84e7b", "BAZZİ6US7" },
                    { 71, null, new DateTime(2001, 4, 5, 12, 48, 22, 534, DateTimeKind.Unspecified).AddTicks(8838), 3, "freddy_huel71@hotmail.com", null, "Freddy", false, "Huel", "7882407484736", "827ccb0eea8a706c4c34a16891f84e7b", "D24GYSSZV" },
                    { 72, null, new DateTime(1998, 8, 27, 14, 3, 32, 284, DateTimeKind.Unspecified).AddTicks(2695), 8, "kaci.fahey25@gmail.com", null, "Kaci", false, "Fahey", "5440714537290", "827ccb0eea8a706c4c34a16891f84e7b", "2DQJYNKCO" },
                    { 73, null, new DateTime(1980, 3, 23, 23, 31, 49, 315, DateTimeKind.Unspecified).AddTicks(9265), 8, "larissa9@gmail.com", null, "Larissa", false, "Sauer", "7203139671873", "827ccb0eea8a706c4c34a16891f84e7b", "SN753MXBH" },
                    { 74, null, new DateTime(1984, 5, 8, 14, 44, 33, 890, DateTimeKind.Unspecified).AddTicks(2464), 4, "salma_braun87@hotmail.com", null, "Salma", false, "Braun", "2420886360559", "827ccb0eea8a706c4c34a16891f84e7b", "VRP5ONSX4" },
                    { 75, null, new DateTime(1999, 6, 17, 23, 11, 37, 741, DateTimeKind.Unspecified).AddTicks(8452), 3, "annabel_flatley67@gmail.com", null, "Annabel", false, "Flatley", "7906004606354", "827ccb0eea8a706c4c34a16891f84e7b", "EDVKEU1NE" },
                    { 76, null, new DateTime(1984, 11, 8, 11, 47, 33, 887, DateTimeKind.Unspecified).AddTicks(7158), 8, "carlee.deckow66@yahoo.com", null, "Carlee", false, "Deckow", "0700807668355", "827ccb0eea8a706c4c34a16891f84e7b", "CLO2U3TJP" },
                    { 77, null, new DateTime(2000, 11, 11, 23, 55, 27, 885, DateTimeKind.Unspecified).AddTicks(2415), 2, "amira_dicki@gmail.com", null, "Amira", false, "Dicki", "1716211564363", "827ccb0eea8a706c4c34a16891f84e7b", "ZRHMXREJ9" },
                    { 78, null, new DateTime(2001, 11, 21, 20, 10, 20, 423, DateTimeKind.Unspecified).AddTicks(6188), 8, "chance.stanton@yahoo.com", null, "Chance", false, "Stanton", "0253876370363", "827ccb0eea8a706c4c34a16891f84e7b", "İA1NCBM11" },
                    { 79, null, new DateTime(1988, 5, 7, 15, 20, 24, 36, DateTimeKind.Unspecified).AddTicks(492), 8, "jacinto91@gmail.com", null, "Jacinto", false, "Rogahn", "6222182347937", "827ccb0eea8a706c4c34a16891f84e7b", "H5H0MZQUİ" },
                    { 80, null, new DateTime(1982, 7, 13, 1, 26, 32, 57, DateTimeKind.Unspecified).AddTicks(802), 4, "kris39@gmail.com", null, "Kris", false, "Dooley", "6838118611006", "827ccb0eea8a706c4c34a16891f84e7b", "K4TM4YAF0" },
                    { 81, null, new DateTime(2000, 12, 24, 14, 23, 18, 728, DateTimeKind.Unspecified).AddTicks(934), 2, "susan1@yahoo.com", null, "Susan", false, "Kessler", "5765665546831", "827ccb0eea8a706c4c34a16891f84e7b", "QW7T3PEP3" },
                    { 82, null, new DateTime(1981, 3, 9, 20, 31, 4, 853, DateTimeKind.Unspecified).AddTicks(2669), 2, "bryce6@yahoo.com", null, "Bryce", false, "Murazik", "5546308126291", "827ccb0eea8a706c4c34a16891f84e7b", "ZLOZYTUT6" },
                    { 83, null, new DateTime(1981, 7, 2, 2, 50, 25, 802, DateTimeKind.Unspecified).AddTicks(3281), 5, "craig.schimmel83@hotmail.com", null, "Craig", false, "Schimmel", "9495914241538", "827ccb0eea8a706c4c34a16891f84e7b", "FJHU1B5OX" },
                    { 84, null, new DateTime(1983, 3, 23, 4, 24, 59, 554, DateTimeKind.Unspecified).AddTicks(3060), 5, "arnulfo78@yahoo.com", null, "Arnulfo", false, "Gusikowski", "2801116479572", "827ccb0eea8a706c4c34a16891f84e7b", "LPTGCBBHH" },
                    { 85, null, new DateTime(1987, 12, 2, 13, 56, 11, 643, DateTimeKind.Unspecified).AddTicks(9159), 1, "jack.prosacco@gmail.com", null, "Jack", false, "Prosacco", "1797913381478", "827ccb0eea8a706c4c34a16891f84e7b", "6S7DYYMVO" },
                    { 86, null, new DateTime(1989, 3, 21, 6, 29, 0, 669, DateTimeKind.Unspecified).AddTicks(6621), 7, "tomasa_schinner@hotmail.com", null, "Tomasa", false, "Schinner", "4432088122420", "827ccb0eea8a706c4c34a16891f84e7b", "YBPS9XMGG" },
                    { 87, null, new DateTime(1996, 2, 24, 6, 33, 34, 46, DateTimeKind.Unspecified).AddTicks(9585), 3, "abdiel.mcglynn@yahoo.com", null, "Abdiel", false, "McGlynn", "3289509506756", "827ccb0eea8a706c4c34a16891f84e7b", "OXT8X3PE3" },
                    { 88, null, new DateTime(1983, 12, 16, 15, 8, 51, 842, DateTimeKind.Unspecified).AddTicks(184), 6, "savion91@yahoo.com", null, "Savion", false, "O'Conner", "3521167951950", "827ccb0eea8a706c4c34a16891f84e7b", "7WNJLASCV" },
                    { 89, null, new DateTime(1991, 1, 6, 2, 37, 44, 806, DateTimeKind.Unspecified).AddTicks(960), 3, "emmalee.gorczany@yahoo.com", null, "Emmalee", false, "Gorczany", "6647643579359", "827ccb0eea8a706c4c34a16891f84e7b", "8P5LUFRX9" },
                    { 90, null, new DateTime(1996, 8, 24, 22, 0, 10, 149, DateTimeKind.Unspecified).AddTicks(2785), 6, "karson56@gmail.com", null, "Karson", false, "McClure", "5277286325709", "827ccb0eea8a706c4c34a16891f84e7b", "CD8BNRA1L" },
                    { 91, null, new DateTime(2005, 7, 21, 15, 46, 40, 197, DateTimeKind.Unspecified).AddTicks(8075), 8, "fernando7@hotmail.com", null, "Fernando", false, "Heller", "5631285987249", "827ccb0eea8a706c4c34a16891f84e7b", "1G11Q3B32" },
                    { 92, null, new DateTime(1991, 4, 17, 23, 12, 45, 688, DateTimeKind.Unspecified).AddTicks(6918), 6, "ottis_mcglynn91@gmail.com", null, "Ottis", false, "McGlynn", "6393516384697", "827ccb0eea8a706c4c34a16891f84e7b", "CTJ1CXVO8" },
                    { 93, null, new DateTime(1999, 7, 1, 14, 44, 50, 529, DateTimeKind.Unspecified).AddTicks(8527), 6, "darrion77@gmail.com", null, "Darrion", false, "Kerluke", "9545757035408", "827ccb0eea8a706c4c34a16891f84e7b", "Z932MBUİ0" },
                    { 94, null, new DateTime(1981, 3, 19, 3, 45, 54, 233, DateTimeKind.Unspecified).AddTicks(450), 6, "junius.okon@yahoo.com", null, "Junius", false, "O'Kon", "0287524436968", "827ccb0eea8a706c4c34a16891f84e7b", "982XS8NQM" },
                    { 95, null, new DateTime(1992, 6, 29, 2, 41, 7, 512, DateTimeKind.Unspecified).AddTicks(2584), 5, "gilda50@gmail.com", null, "Gilda", false, "Deckow", "7433189801468", "827ccb0eea8a706c4c34a16891f84e7b", "İ2XJTET0W" },
                    { 96, null, new DateTime(1987, 7, 21, 16, 22, 30, 750, DateTimeKind.Unspecified).AddTicks(3906), 3, "bobby.mcclure@hotmail.com", null, "Bobby", false, "McClure", "5770280280989", "827ccb0eea8a706c4c34a16891f84e7b", "W1CVJNY6P" },
                    { 97, null, new DateTime(1984, 7, 19, 18, 42, 25, 76, DateTimeKind.Unspecified).AddTicks(2488), 1, "brody_kautzer@gmail.com", null, "Brody", false, "Kautzer", "1508239151619", "827ccb0eea8a706c4c34a16891f84e7b", "2EJQ57FC6" },
                    { 98, null, new DateTime(1994, 9, 16, 6, 56, 3, 146, DateTimeKind.Unspecified).AddTicks(2178), 4, "kade.hegmann59@gmail.com", null, "Kade", false, "Hegmann", "1811117777709", "827ccb0eea8a706c4c34a16891f84e7b", "GZ36R882A" },
                    { 99, null, new DateTime(1999, 8, 29, 23, 28, 1, 511, DateTimeKind.Unspecified).AddTicks(5988), 7, "creola_veum49@hotmail.com", null, "Creola", false, "Veum", "7831527687920", "827ccb0eea8a706c4c34a16891f84e7b", "XOSHJR3YH" },
                    { 100, null, new DateTime(1995, 4, 13, 7, 35, 32, 52, DateTimeKind.Unspecified).AddTicks(2882), 2, "jameson99@gmail.com", null, "Jameson", false, "Feest", "4711348836816", "827ccb0eea8a706c4c34a16891f84e7b", "GH0Z9KFXJ" },
                    { 101, null, new DateTime(2002, 5, 17, 3, 27, 50, 456, DateTimeKind.Unspecified).AddTicks(3713), 8, "palma.stokes23@gmail.com", null, "Palma", false, "Stokes", "7831239449711", "827ccb0eea8a706c4c34a16891f84e7b", "44ZQPİ8LK" },
                    { 102, null, new DateTime(1994, 8, 21, 4, 5, 20, 823, DateTimeKind.Unspecified).AddTicks(1678), 8, "curt88@hotmail.com", null, "Curt", false, "Wilkinson", "7472807580192", "827ccb0eea8a706c4c34a16891f84e7b", "RPF9R6ERT" },
                    { 103, null, new DateTime(1982, 11, 30, 23, 47, 5, 76, DateTimeKind.Unspecified).AddTicks(3681), 3, "dagmar.gerhold91@gmail.com", null, "Dagmar", false, "Gerhold", "6865586923849", "827ccb0eea8a706c4c34a16891f84e7b", "NTXSJQ1OF" },
                    { 104, null, new DateTime(2002, 4, 24, 6, 23, 51, 649, DateTimeKind.Unspecified).AddTicks(2330), 1, "hilton26@yahoo.com", null, "Hilton", false, "Erdman", "0115993109874", "827ccb0eea8a706c4c34a16891f84e7b", "5PGX5FOGR" },
                    { 105, null, new DateTime(2003, 4, 10, 9, 32, 39, 426, DateTimeKind.Unspecified).AddTicks(3848), 7, "felipa.kuhic58@gmail.com", null, "Felipa", false, "Kuhic", "1559153751874", "827ccb0eea8a706c4c34a16891f84e7b", "YH5HEQ08S" },
                    { 106, null, new DateTime(1997, 1, 3, 12, 14, 30, 816, DateTimeKind.Unspecified).AddTicks(5188), 8, "clement_schultz@hotmail.com", null, "Clement", false, "Schultz", "0704291298666", "827ccb0eea8a706c4c34a16891f84e7b", "ZV7UGQ13E" },
                    { 107, null, new DateTime(1996, 1, 30, 19, 59, 12, 972, DateTimeKind.Unspecified).AddTicks(2360), 1, "krista16@hotmail.com", null, "Krista", false, "Denesik", "2480810330377", "827ccb0eea8a706c4c34a16891f84e7b", "9VUTWKCDS" },
                    { 108, null, new DateTime(2005, 9, 10, 5, 31, 45, 762, DateTimeKind.Unspecified).AddTicks(9981), 1, "felicity.pagac@hotmail.com", null, "Felicity", false, "Pagac", "4876818034155", "827ccb0eea8a706c4c34a16891f84e7b", "9X0J2EHAQ" },
                    { 109, null, new DateTime(2001, 12, 4, 11, 16, 4, 921, DateTimeKind.Unspecified).AddTicks(6190), 1, "earlene51@yahoo.com", null, "Earlene", false, "Schmeler", "4045873515269", "827ccb0eea8a706c4c34a16891f84e7b", "PQBE91P5H" },
                    { 110, null, new DateTime(1983, 5, 22, 18, 43, 39, 269, DateTimeKind.Unspecified).AddTicks(8664), 8, "brad68@gmail.com", null, "Brad", false, "Feil", "4794970694952", "827ccb0eea8a706c4c34a16891f84e7b", "RLLOD2UX3" },
                    { 111, null, new DateTime(1990, 1, 20, 3, 34, 19, 173, DateTimeKind.Unspecified).AddTicks(7956), 2, "alison_ledner@gmail.com", null, "Alison", false, "Ledner", "8631951420321", "827ccb0eea8a706c4c34a16891f84e7b", "2J69RZ8F7" },
                    { 112, null, new DateTime(1986, 6, 6, 13, 53, 32, 846, DateTimeKind.Unspecified).AddTicks(6238), 6, "devin18@hotmail.com", null, "Devin", false, "Schroeder", "5543686669891", "827ccb0eea8a706c4c34a16891f84e7b", "QRSX4HNV9" },
                    { 113, null, new DateTime(2003, 1, 10, 4, 35, 25, 893, DateTimeKind.Unspecified).AddTicks(9206), 2, "sage_mills45@gmail.com", null, "Sage", false, "Mills", "9414613517658", "827ccb0eea8a706c4c34a16891f84e7b", "XTA0HG7FS" },
                    { 114, null, new DateTime(1992, 2, 8, 10, 51, 27, 380, DateTimeKind.Unspecified).AddTicks(876), 1, "jaunita88@hotmail.com", null, "Jaunita", false, "Murphy", "1332368032479", "827ccb0eea8a706c4c34a16891f84e7b", "PSSVXZ5AR" },
                    { 115, null, new DateTime(2000, 2, 13, 10, 14, 19, 718, DateTimeKind.Unspecified).AddTicks(1388), 2, "dulce.hoeger12@hotmail.com", null, "Dulce", false, "Hoeger", "7801586964922", "827ccb0eea8a706c4c34a16891f84e7b", "KUDJH0GQF" },
                    { 116, null, new DateTime(1982, 9, 30, 21, 14, 27, 799, DateTimeKind.Unspecified).AddTicks(1993), 2, "kaelyn_marquardt@yahoo.com", null, "Kaelyn", false, "Marquardt", "2582745462717", "827ccb0eea8a706c4c34a16891f84e7b", "Z5EWWRMG7" },
                    { 117, null, new DateTime(2000, 5, 26, 0, 5, 44, 129, DateTimeKind.Unspecified).AddTicks(2905), 3, "raheem9@hotmail.com", null, "Raheem", false, "Franecki", "6121259473855", "827ccb0eea8a706c4c34a16891f84e7b", "HPBVUBU0N" },
                    { 118, null, new DateTime(1988, 12, 6, 13, 42, 51, 893, DateTimeKind.Unspecified).AddTicks(7598), 3, "rose_johnston@yahoo.com", null, "Rose", false, "Johnston", "7502379812209", "827ccb0eea8a706c4c34a16891f84e7b", "QYD9R8MZM" },
                    { 119, null, new DateTime(1992, 6, 3, 19, 23, 38, 266, DateTimeKind.Unspecified).AddTicks(3264), 4, "elda69@yahoo.com", null, "Elda", false, "Koch", "1913161769168", "827ccb0eea8a706c4c34a16891f84e7b", "1H1ZBD32İ" },
                    { 120, null, new DateTime(1997, 2, 7, 8, 41, 32, 717, DateTimeKind.Unspecified).AddTicks(9735), 4, "margarete57@yahoo.com", null, "Margarete", false, "O'Connell", "9638081921580", "827ccb0eea8a706c4c34a16891f84e7b", "B94CPXRİF" },
                    { 121, null, new DateTime(2005, 5, 11, 3, 42, 20, 139, DateTimeKind.Unspecified).AddTicks(4281), 5, "jude_davis@gmail.com", null, "Jude", false, "Davis", "9993223143146", "827ccb0eea8a706c4c34a16891f84e7b", "GS1QYPP2P" },
                    { 122, null, new DateTime(1980, 9, 20, 2, 53, 30, 151, DateTimeKind.Unspecified).AddTicks(1425), 3, "dexter54@yahoo.com", null, "Dexter", false, "Gottlieb", "9709506463137", "827ccb0eea8a706c4c34a16891f84e7b", "9OANT55FY" },
                    { 123, null, new DateTime(2005, 8, 4, 2, 40, 27, 27, DateTimeKind.Unspecified).AddTicks(7118), 1, "eldred_oconnell@yahoo.com", null, "Eldred", false, "O'Connell", "4030085596125", "827ccb0eea8a706c4c34a16891f84e7b", "08SOSQ3J4" },
                    { 124, null, new DateTime(1999, 2, 22, 17, 36, 52, 396, DateTimeKind.Unspecified).AddTicks(4305), 1, "alva_okuneva69@yahoo.com", null, "Alva", false, "Okuneva", "7386866128173", "827ccb0eea8a706c4c34a16891f84e7b", "UV4AXHBKE" },
                    { 125, null, new DateTime(1989, 11, 15, 11, 4, 7, 113, DateTimeKind.Unspecified).AddTicks(6173), 8, "gisselle.gutkowski41@gmail.com", null, "Gisselle", false, "Gutkowski", "8818475705461", "827ccb0eea8a706c4c34a16891f84e7b", "İİ7KQLKXQ" },
                    { 126, null, new DateTime(1993, 9, 24, 23, 58, 38, 878, DateTimeKind.Unspecified).AddTicks(2292), 5, "delaney.gulgowski@yahoo.com", null, "Delaney", false, "Gulgowski", "7933389922383", "827ccb0eea8a706c4c34a16891f84e7b", "Y1BT7KBWV" },
                    { 127, null, new DateTime(1981, 10, 16, 9, 31, 59, 182, DateTimeKind.Unspecified).AddTicks(5737), 4, "baron_gusikowski@gmail.com", null, "Baron", false, "Gusikowski", "6611779704396", "827ccb0eea8a706c4c34a16891f84e7b", "BLPMKG3JZ" },
                    { 128, null, new DateTime(1985, 8, 6, 4, 59, 3, 730, DateTimeKind.Unspecified).AddTicks(2120), 2, "beryl.kerluke34@hotmail.com", null, "Beryl", false, "Kerluke", "9470960721254", "827ccb0eea8a706c4c34a16891f84e7b", "M7JEXVVF8" },
                    { 129, null, new DateTime(1990, 10, 29, 1, 55, 28, 212, DateTimeKind.Unspecified).AddTicks(692), 1, "marcella_stiedemann@yahoo.com", null, "Marcella", false, "Stiedemann", "5276059681902", "827ccb0eea8a706c4c34a16891f84e7b", "SM6K6GJ7J" },
                    { 130, null, new DateTime(2002, 12, 19, 2, 56, 50, 904, DateTimeKind.Unspecified).AddTicks(354), 6, "talon.lang69@hotmail.com", null, "Talon", false, "Lang", "4814378751176", "827ccb0eea8a706c4c34a16891f84e7b", "MCYM9RM96" },
                    { 131, null, new DateTime(1994, 12, 31, 20, 29, 50, 711, DateTimeKind.Unspecified).AddTicks(1424), 1, "tyrel.mohr48@hotmail.com", null, "Tyrel", false, "Mohr", "9468457885585", "827ccb0eea8a706c4c34a16891f84e7b", "Y1Z7OOL4A" },
                    { 132, null, new DateTime(2003, 6, 11, 6, 20, 8, 205, DateTimeKind.Unspecified).AddTicks(40), 8, "merl_nolan@hotmail.com", null, "Merl", false, "Nolan", "0168933695504", "827ccb0eea8a706c4c34a16891f84e7b", "5F92Q38KH" },
                    { 133, null, new DateTime(1995, 2, 16, 20, 8, 40, 692, DateTimeKind.Unspecified).AddTicks(7628), 3, "marcus40@hotmail.com", null, "Marcus", false, "Smith", "4999092177041", "827ccb0eea8a706c4c34a16891f84e7b", "VU2CZKG5S" },
                    { 134, null, new DateTime(1990, 1, 19, 6, 52, 6, 762, DateTimeKind.Unspecified).AddTicks(9448), 5, "sarai_zemlak37@hotmail.com", null, "Sarai", false, "Zemlak", "4302213881493", "827ccb0eea8a706c4c34a16891f84e7b", "L2VJ53L8O" },
                    { 135, null, new DateTime(1994, 7, 19, 17, 1, 38, 800, DateTimeKind.Unspecified).AddTicks(7888), 4, "scarlett_fisher16@hotmail.com", null, "Scarlett", false, "Fisher", "6948603742248", "827ccb0eea8a706c4c34a16891f84e7b", "92TLSRDJY" },
                    { 136, null, new DateTime(1994, 7, 10, 3, 14, 20, 500, DateTimeKind.Unspecified).AddTicks(1256), 6, "dessie_harvey@yahoo.com", null, "Dessie", false, "Harvey", "1224017611243", "827ccb0eea8a706c4c34a16891f84e7b", "269187W4G" },
                    { 137, null, new DateTime(1980, 3, 2, 5, 54, 44, 782, DateTimeKind.Unspecified).AddTicks(3136), 5, "ali90@yahoo.com", null, "Ali", false, "Boehm", "8713323972490", "827ccb0eea8a706c4c34a16891f84e7b", "ADN923SGQ" },
                    { 138, null, new DateTime(1999, 5, 21, 13, 11, 24, 848, DateTimeKind.Unspecified).AddTicks(1983), 3, "andy_smitham29@hotmail.com", null, "Andy", false, "Smitham", "1269810782968", "827ccb0eea8a706c4c34a16891f84e7b", "İB302OW57" },
                    { 139, null, new DateTime(1981, 10, 24, 12, 10, 4, 960, DateTimeKind.Unspecified).AddTicks(134), 3, "asha_pollich@gmail.com", null, "Asha", false, "Pollich", "1037817614558", "827ccb0eea8a706c4c34a16891f84e7b", "SRXS9X4MQ" },
                    { 140, null, new DateTime(1993, 6, 13, 23, 29, 55, 932, DateTimeKind.Unspecified).AddTicks(154), 7, "lavina36@gmail.com", null, "Lavina", false, "Cummerata", "7685141048485", "827ccb0eea8a706c4c34a16891f84e7b", "NEG2EMM76" },
                    { 141, null, new DateTime(1993, 6, 26, 4, 52, 43, 549, DateTimeKind.Unspecified).AddTicks(9862), 3, "ulices19@hotmail.com", null, "Ulices", false, "Zemlak", "2176511665468", "827ccb0eea8a706c4c34a16891f84e7b", "3L8D2LHSM" },
                    { 142, null, new DateTime(1998, 6, 5, 13, 31, 28, 249, DateTimeKind.Unspecified).AddTicks(7350), 7, "daphnee.kerluke87@hotmail.com", null, "Daphnee", false, "Kerluke", "1824181526351", "827ccb0eea8a706c4c34a16891f84e7b", "EOVLRUUR3" },
                    { 143, null, new DateTime(1981, 2, 21, 2, 45, 37, 281, DateTimeKind.Unspecified).AddTicks(4651), 4, "jordane.monahan@hotmail.com", null, "Jordane", false, "Monahan", "9271775589099", "827ccb0eea8a706c4c34a16891f84e7b", "1S391JKSA" },
                    { 144, null, new DateTime(1981, 4, 7, 5, 5, 57, 908, DateTimeKind.Unspecified).AddTicks(3703), 1, "daren3@yahoo.com", null, "Daren", false, "Orn", "8285906458951", "827ccb0eea8a706c4c34a16891f84e7b", "O3GJYZ9BK" },
                    { 145, null, new DateTime(1984, 9, 13, 10, 1, 21, 677, DateTimeKind.Unspecified).AddTicks(3428), 5, "leif_connelly@hotmail.com", null, "Leif", false, "Connelly", "4593312474973", "827ccb0eea8a706c4c34a16891f84e7b", "SMG2DHBTH" },
                    { 146, null, new DateTime(1984, 12, 6, 16, 48, 39, 255, DateTimeKind.Unspecified).AddTicks(9038), 8, "jeanie.schaefer80@hotmail.com", null, "Jeanie", false, "Schaefer", "8923323510751", "827ccb0eea8a706c4c34a16891f84e7b", "9FS0P5DJ4" },
                    { 147, null, new DateTime(2002, 1, 7, 15, 42, 41, 62, DateTimeKind.Unspecified).AddTicks(7672), 4, "adell56@gmail.com", null, "Adell", false, "Schiller", "9605373019445", "827ccb0eea8a706c4c34a16891f84e7b", "8YAOPD2VL" },
                    { 148, null, new DateTime(1990, 3, 30, 6, 52, 43, 284, DateTimeKind.Unspecified).AddTicks(1700), 3, "nia34@gmail.com", null, "Nia", false, "Hirthe", "0900375485559", "827ccb0eea8a706c4c34a16891f84e7b", "PPF5Z74DQ" },
                    { 149, null, new DateTime(1983, 8, 2, 3, 3, 14, 145, DateTimeKind.Unspecified).AddTicks(8157), 4, "thurman.jaskolski@yahoo.com", null, "Thurman", false, "Jaskolski", "5447448038530", "827ccb0eea8a706c4c34a16891f84e7b", "LZGY3RVİT" },
                    { 150, null, new DateTime(1984, 12, 30, 5, 51, 26, 603, DateTimeKind.Unspecified).AddTicks(5585), 8, "keeley_runte@hotmail.com", null, "Keeley", false, "Runte", "4561940052187", "827ccb0eea8a706c4c34a16891f84e7b", "8UNU8ABAC" },
                    { 151, null, new DateTime(2002, 4, 7, 11, 4, 29, 491, DateTimeKind.Unspecified).AddTicks(8195), 4, "chadrick_schowalter@hotmail.com", null, "Chadrick", false, "Schowalter", "9423807632475", "827ccb0eea8a706c4c34a16891f84e7b", "İQTGJFİ6M" },
                    { 152, null, new DateTime(1985, 1, 9, 14, 58, 45, 76, DateTimeKind.Unspecified).AddTicks(5138), 3, "else63@gmail.com", null, "Else", false, "Nolan", "8724107694736", "827ccb0eea8a706c4c34a16891f84e7b", "4B2EA30BR" },
                    { 153, null, new DateTime(1994, 1, 28, 19, 10, 34, 767, DateTimeKind.Unspecified).AddTicks(2984), 8, "delores.howe34@hotmail.com", null, "Delores", false, "Howe", "3846635520763", "827ccb0eea8a706c4c34a16891f84e7b", "BJFGDW5YJ" },
                    { 154, null, new DateTime(1986, 12, 7, 2, 54, 52, 735, DateTimeKind.Unspecified).AddTicks(4776), 7, "magnus43@gmail.com", null, "Magnus", false, "Hintz", "8368991586481", "827ccb0eea8a706c4c34a16891f84e7b", "19KW3HEBD" },
                    { 155, null, new DateTime(1990, 12, 12, 23, 42, 26, 55, DateTimeKind.Unspecified).AddTicks(9671), 8, "kristina.beatty13@gmail.com", null, "Kristina", false, "Beatty", "4172185411272", "827ccb0eea8a706c4c34a16891f84e7b", "E5BR7RS09" },
                    { 156, null, new DateTime(1999, 4, 26, 16, 49, 23, 913, DateTimeKind.Unspecified).AddTicks(3191), 6, "vita14@gmail.com", null, "Vita", false, "Doyle", "8343217167494", "827ccb0eea8a706c4c34a16891f84e7b", "VLRPT2KD5" },
                    { 157, null, new DateTime(1994, 7, 7, 14, 54, 26, 192, DateTimeKind.Unspecified).AddTicks(7951), 8, "sandra_kuhlman@hotmail.com", null, "Sandra", false, "Kuhlman", "0380743814979", "827ccb0eea8a706c4c34a16891f84e7b", "Zİ4RDM3YO" },
                    { 158, null, new DateTime(2001, 5, 16, 12, 40, 18, 326, DateTimeKind.Unspecified).AddTicks(7071), 7, "lee.hudson@hotmail.com", null, "Lee", false, "Hudson", "0635135756231", "827ccb0eea8a706c4c34a16891f84e7b", "QSFJ0NVT2" },
                    { 159, null, new DateTime(1980, 8, 31, 13, 16, 3, 575, DateTimeKind.Unspecified).AddTicks(1034), 7, "clinton_doyle@yahoo.com", null, "Clinton", false, "Doyle", "8368532890233", "827ccb0eea8a706c4c34a16891f84e7b", "DY12ZJMPQ" },
                    { 160, null, new DateTime(2002, 1, 20, 14, 2, 14, 917, DateTimeKind.Unspecified).AddTicks(1178), 4, "jolie_swaniawski@yahoo.com", null, "Jolie", false, "Swaniawski", "7154165470835", "827ccb0eea8a706c4c34a16891f84e7b", "M9UERE168" },
                    { 161, null, new DateTime(2000, 12, 28, 4, 1, 8, 206, DateTimeKind.Unspecified).AddTicks(4592), 8, "oda_lueilwitz32@gmail.com", null, "Oda", false, "Lueilwitz", "8742248435768", "827ccb0eea8a706c4c34a16891f84e7b", "YİCF1NTAC" },
                    { 162, null, new DateTime(1987, 11, 2, 15, 26, 30, 792, DateTimeKind.Unspecified).AddTicks(9646), 7, "roosevelt.yost@hotmail.com", null, "Roosevelt", false, "Yost", "5652544294934", "827ccb0eea8a706c4c34a16891f84e7b", "2O0EYH6UY" },
                    { 163, null, new DateTime(1985, 7, 18, 17, 42, 11, 768, DateTimeKind.Unspecified).AddTicks(7136), 6, "georgiana_daniel@yahoo.com", null, "Georgiana", false, "Daniel", "3777388488463", "827ccb0eea8a706c4c34a16891f84e7b", "Q5ZXRQVAU" },
                    { 164, null, new DateTime(1993, 12, 12, 22, 34, 12, 742, DateTimeKind.Unspecified).AddTicks(8911), 5, "rory.streich@yahoo.com", null, "Rory", false, "Streich", "0850162008940", "827ccb0eea8a706c4c34a16891f84e7b", "1AT0DEB9H" },
                    { 165, null, new DateTime(1991, 8, 30, 12, 9, 48, 950, DateTimeKind.Unspecified).AddTicks(3838), 3, "johnathon.feest33@gmail.com", null, "Johnathon", false, "Feest", "6948314517583", "827ccb0eea8a706c4c34a16891f84e7b", "X79VEPN57" },
                    { 166, null, new DateTime(2002, 5, 22, 18, 31, 24, 999, DateTimeKind.Unspecified).AddTicks(2718), 8, "trystan36@yahoo.com", null, "Trystan", false, "Dooley", "8362674656362", "827ccb0eea8a706c4c34a16891f84e7b", "KSBİH9M4N" },
                    { 167, null, new DateTime(2000, 11, 21, 7, 55, 45, 588, DateTimeKind.Unspecified).AddTicks(8976), 3, "ısabella47@yahoo.com", null, "Isabella", false, "Schmitt", "9614887950802", "827ccb0eea8a706c4c34a16891f84e7b", "MFRQUFCO1" },
                    { 168, null, new DateTime(1983, 2, 1, 6, 21, 57, 622, DateTimeKind.Unspecified).AddTicks(6682), 7, "lulu.fisher71@hotmail.com", null, "Lulu", false, "Fisher", "3830438778404", "827ccb0eea8a706c4c34a16891f84e7b", "MTRUMFARX" },
                    { 169, null, new DateTime(1982, 2, 8, 17, 10, 18, 338, DateTimeKind.Unspecified).AddTicks(8824), 4, "graciela.bergstrom54@hotmail.com", null, "Graciela", false, "Bergstrom", "7682445389977", "827ccb0eea8a706c4c34a16891f84e7b", "GB0OMY0R5" },
                    { 170, null, new DateTime(1992, 3, 19, 21, 11, 57, 224, DateTimeKind.Unspecified).AddTicks(4628), 4, "paxton_stamm92@hotmail.com", null, "Paxton", false, "Stamm", "8536028201557", "827ccb0eea8a706c4c34a16891f84e7b", "26959NGQY" },
                    { 171, null, new DateTime(1992, 7, 6, 8, 44, 25, 772, DateTimeKind.Unspecified).AddTicks(324), 8, "willis_ratke@hotmail.com", null, "Willis", false, "Ratke", "6042124419551", "827ccb0eea8a706c4c34a16891f84e7b", "TLJ299CGP" },
                    { 172, null, new DateTime(1994, 9, 10, 8, 37, 35, 522, DateTimeKind.Unspecified).AddTicks(5642), 7, "gilberto_gaylord@hotmail.com", null, "Gilberto", false, "Gaylord", "1778708462640", "827ccb0eea8a706c4c34a16891f84e7b", "99K3UFUN5" },
                    { 173, null, new DateTime(1998, 7, 31, 5, 40, 46, 516, DateTimeKind.Unspecified).AddTicks(7569), 7, "autumn72@hotmail.com", null, "Autumn", false, "Rowe", "8419836822670", "827ccb0eea8a706c4c34a16891f84e7b", "BZPXO0XDV" },
                    { 174, null, new DateTime(1984, 11, 4, 4, 29, 57, 327, DateTimeKind.Unspecified).AddTicks(7472), 7, "dora34@gmail.com", null, "Dora", false, "Beatty", "6406525500177", "827ccb0eea8a706c4c34a16891f84e7b", "7MPJA1VHT" },
                    { 175, null, new DateTime(2001, 1, 30, 13, 1, 52, 144, DateTimeKind.Unspecified).AddTicks(1361), 3, "elvie_kuhlman71@yahoo.com", null, "Elvie", false, "Kuhlman", "7573169052262", "827ccb0eea8a706c4c34a16891f84e7b", "Z7A79AE4R" },
                    { 176, null, new DateTime(1985, 1, 10, 15, 1, 46, 3, DateTimeKind.Unspecified).AddTicks(7932), 5, "ruby51@yahoo.com", null, "Ruby", false, "Rippin", "2989827945667", "827ccb0eea8a706c4c34a16891f84e7b", "4W4Jİ7TU5" },
                    { 177, null, new DateTime(1984, 8, 9, 22, 44, 42, 297, DateTimeKind.Unspecified).AddTicks(1240), 4, "damion_stokes83@hotmail.com", null, "Damion", false, "Stokes", "7516209559953", "827ccb0eea8a706c4c34a16891f84e7b", "QBH3XBVO9" },
                    { 178, null, new DateTime(1989, 8, 26, 3, 16, 49, 678, DateTimeKind.Unspecified).AddTicks(7782), 5, "clair64@yahoo.com", null, "Clair", false, "Ortiz", "6266371724776", "827ccb0eea8a706c4c34a16891f84e7b", "UL5L23ZRC" },
                    { 179, null, new DateTime(1993, 12, 31, 7, 57, 57, 99, DateTimeKind.Unspecified).AddTicks(1286), 5, "zachariah17@yahoo.com", null, "Zachariah", false, "Bauch", "8618954207822", "827ccb0eea8a706c4c34a16891f84e7b", "SVBİB98QY" },
                    { 180, null, new DateTime(1997, 5, 15, 2, 28, 47, 710, DateTimeKind.Unspecified).AddTicks(2052), 7, "dimitri10@gmail.com", null, "Dimitri", false, "Hudson", "5657486181308", "827ccb0eea8a706c4c34a16891f84e7b", "G6VBMUG29" },
                    { 181, null, new DateTime(2001, 4, 22, 7, 48, 59, 644, DateTimeKind.Unspecified).AddTicks(3951), 2, "curt20@yahoo.com", null, "Curt", false, "Dibbert", "9038453926010", "827ccb0eea8a706c4c34a16891f84e7b", "ZAYAMS4FZ" },
                    { 182, null, new DateTime(1989, 7, 29, 11, 44, 30, 985, DateTimeKind.Unspecified).AddTicks(8298), 5, "donna.jenkins58@yahoo.com", null, "Donna", false, "Jenkins", "6976808118548", "827ccb0eea8a706c4c34a16891f84e7b", "W7VİHVJ0F" },
                    { 183, null, new DateTime(1991, 9, 5, 1, 28, 20, 94, DateTimeKind.Unspecified).AddTicks(3528), 8, "jolie_bins@hotmail.com", null, "Jolie", false, "Bins", "4855689443394", "827ccb0eea8a706c4c34a16891f84e7b", "W8JBİRHN1" },
                    { 184, null, new DateTime(1983, 12, 19, 16, 37, 38, 956, DateTimeKind.Unspecified).AddTicks(1943), 7, "agustin_walsh@gmail.com", null, "Agustin", false, "Walsh", "1546503870355", "827ccb0eea8a706c4c34a16891f84e7b", "DEHNK3101" },
                    { 185, null, new DateTime(1996, 1, 25, 14, 39, 52, 365, DateTimeKind.Unspecified).AddTicks(5590), 8, "mellie.rodriguez@hotmail.com", null, "Mellie", false, "Rodriguez", "7933892167126", "827ccb0eea8a706c4c34a16891f84e7b", "13YPJBOKB" },
                    { 186, null, new DateTime(1989, 4, 12, 12, 2, 10, 542, DateTimeKind.Unspecified).AddTicks(9916), 6, "estelle_harris8@hotmail.com", null, "Estelle", false, "Harris", "7105770448333", "827ccb0eea8a706c4c34a16891f84e7b", "G2TOTAYFY" },
                    { 187, null, new DateTime(1991, 9, 6, 6, 37, 38, 29, DateTimeKind.Unspecified).AddTicks(2534), 1, "mekhi25@hotmail.com", null, "Mekhi", false, "Russel", "4782964161187", "827ccb0eea8a706c4c34a16891f84e7b", "X8BİNİH5H" },
                    { 188, null, new DateTime(1999, 5, 10, 1, 7, 42, 608, DateTimeKind.Unspecified).AddTicks(7891), 8, "eda_ohara97@gmail.com", null, "Eda", false, "O'Hara", "6539741043929", "827ccb0eea8a706c4c34a16891f84e7b", "K33CQGFEZ" },
                    { 189, null, new DateTime(1991, 7, 1, 8, 19, 31, 261, DateTimeKind.Unspecified).AddTicks(5855), 1, "perry.murphy@hotmail.com", null, "Perry", false, "Murphy", "1452591376702", "827ccb0eea8a706c4c34a16891f84e7b", "23HPCXZHX" },
                    { 190, null, new DateTime(1982, 12, 23, 4, 29, 46, 979, DateTimeKind.Unspecified).AddTicks(1845), 3, "pasquale_kub75@yahoo.com", null, "Pasquale", false, "Kub", "3098857447351", "827ccb0eea8a706c4c34a16891f84e7b", "XPBMB8ZDV" },
                    { 191, null, new DateTime(2000, 11, 7, 6, 23, 58, 468, DateTimeKind.Unspecified).AddTicks(1234), 8, "berneice56@hotmail.com", null, "Berneice", false, "Beer", "1937017177792", "827ccb0eea8a706c4c34a16891f84e7b", "MQZWQUHFM" },
                    { 192, null, new DateTime(1991, 5, 8, 13, 43, 12, 612, DateTimeKind.Unspecified).AddTicks(8990), 3, "heloise48@yahoo.com", null, "Heloise", false, "Cummerata", "9805523960720", "827ccb0eea8a706c4c34a16891f84e7b", "ZMKC8NQVN" },
                    { 193, null, new DateTime(1990, 4, 25, 16, 6, 23, 522, DateTimeKind.Unspecified).AddTicks(1301), 8, "joe_king92@yahoo.com", null, "Joe", false, "King", "7469488378144", "827ccb0eea8a706c4c34a16891f84e7b", "LLB1P67W6" },
                    { 194, null, new DateTime(1990, 7, 25, 4, 15, 36, 572, DateTimeKind.Unspecified).AddTicks(7011), 5, "dayana_williamson2@yahoo.com", null, "Dayana", false, "Williamson", "1460529769820", "827ccb0eea8a706c4c34a16891f84e7b", "Z1DJWR29İ" },
                    { 195, null, new DateTime(2000, 11, 21, 5, 14, 38, 551, DateTimeKind.Unspecified).AddTicks(3600), 1, "gabriella23@hotmail.com", null, "Gabriella", false, "Murazik", "7467542634123", "827ccb0eea8a706c4c34a16891f84e7b", "MDJ63EFRW" },
                    { 196, null, new DateTime(1980, 4, 13, 10, 29, 39, 194, DateTimeKind.Unspecified).AddTicks(1177), 6, "baby1@gmail.com", null, "Baby", false, "Conn", "9166866670784", "827ccb0eea8a706c4c34a16891f84e7b", "72İGYGR89" },
                    { 197, null, new DateTime(2000, 10, 20, 8, 11, 57, 952, DateTimeKind.Unspecified).AddTicks(5910), 3, "jaydon.herzog82@gmail.com", null, "Jaydon", false, "Herzog", "2508444862283", "827ccb0eea8a706c4c34a16891f84e7b", "O9DV20C89" },
                    { 198, null, new DateTime(2002, 6, 1, 20, 2, 51, 468, DateTimeKind.Unspecified).AddTicks(9449), 6, "joan19@hotmail.com", null, "Joan", false, "Wilderman", "3400123085577", "827ccb0eea8a706c4c34a16891f84e7b", "1KBEC7CGE" },
                    { 199, null, new DateTime(1984, 8, 9, 5, 52, 55, 382, DateTimeKind.Unspecified).AddTicks(6540), 1, "buster.grady@yahoo.com", null, "Buster", false, "Grady", "6265855603916", "827ccb0eea8a706c4c34a16891f84e7b", "RAWKB4A48" },
                    { 200, null, new DateTime(1990, 2, 4, 0, 10, 26, 473, DateTimeKind.Unspecified).AddTicks(5959), 8, "lamar5@gmail.com", null, "Lamar", false, "Fritsch", "6848728967105", "827ccb0eea8a706c4c34a16891f84e7b", "QXUQ8GMİ5" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AdminId",
                table: "Announcements",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourseId",
                table: "Announcements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_LecturerId",
                table: "Announcements",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_StudentPersonalId",
                table: "Announcements",
                column: "StudentPersonalId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCourseGroup_CoursesId",
                table: "CourseCourseGroup",
                column: "CoursesId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroups_DepartmentId",
                table: "CourseGroups",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroups_EntranceSemesterId",
                table: "CourseGroups",
                column: "EntranceSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLetterGradeIntervals_CourseId",
                table: "CourseLetterGradeIntervals",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLetterGradeIntervals_SemesterId",
                table: "CourseLetterGradeIntervals",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LecturerId",
                table: "Courses",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleEntries_CourseId",
                table: "CourseScheduleEntries",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleEntries_SemesterId",
                table: "CourseScheduleEntries",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSemester_SemestersId",
                table: "CourseSemester",
                column: "SemestersId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudentCourseSelection_StudentCourseSelectionsId",
                table: "CourseStudentCourseSelection",
                column: "StudentCourseSelectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentLecturer_LecturersId",
                table: "DepartmentLecturer",
                column: "LecturersId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SemesterId",
                table: "Exams",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CourseId",
                table: "Grades",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ExamId",
                table: "Grades",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SemesterId",
                table: "Grades",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_ActiveSemesterId",
                table: "Lecturers",
                column: "ActiveSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_Email",
                table: "Lecturers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseSelections_SemesterId",
                table: "StudentCourseSelections",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseSelections_StudentId",
                table: "StudentCourseSelections",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonals_ActiveSemesterId",
                table: "StudentPersonals",
                column: "ActiveSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPersonals_Email",
                table: "StudentPersonals",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ActiveSemesterId",
                table: "Students",
                column: "ActiveSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_EntranceSemesterId",
                table: "Students",
                column: "EntranceSemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_CourseId",
                table: "Transcripts",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_SemesterId",
                table: "Transcripts",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_StudentId",
                table: "Transcripts",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "CourseCourseGroup");

            migrationBuilder.DropTable(
                name: "CourseLetterGradeIntervals");

            migrationBuilder.DropTable(
                name: "CourseScheduleEntries");

            migrationBuilder.DropTable(
                name: "CourseSemester");

            migrationBuilder.DropTable(
                name: "CourseStudentCourseSelection");

            migrationBuilder.DropTable(
                name: "DepartmentLecturer");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Transcripts");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "StudentPersonals");

            migrationBuilder.DropTable(
                name: "CourseGroups");

            migrationBuilder.DropTable(
                name: "StudentCourseSelections");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Lecturers");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Semesters");
        }
    }
}
