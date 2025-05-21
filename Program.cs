using gp_unisis.Database;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;
using gp_unisis.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Bruh;

public class Bruh
{
    public static void Main()
    {
        var db = new ApplicationDbContext();
        if (db.Database.EnsureCreated())
        {
            // Add courses to semesters randomly
            for (int i = 1; i <= 50; i++)
            {
                // select a random semester
                var semesters = new List<Semester>();
                for (int j = 1; j < 25; j++)
                {
                    var rnd = new Random();
                    var semesterId = rnd.Next(1, 5);

                    if (!semesters.Any(s => s.Id == semesterId))
                    {
                        semesters.Add(db.Semesters.FirstOrDefault(s => s.Id == semesterId));
                    }

                    var course = db.Courses.FirstOrDefault(c => c.Id == i);
                    if (course != null)
                    {
                        course.Semesters = semesters;
                        db.SaveChanges();
                    }
                }
            }
            
            string[] days = ["Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"];
            // Add vize and final exams to courses for each semester
            for (int i = 1; i <= 50; i++)
            {
                var course = db.Courses.Include(c => c.Semesters).FirstOrDefault(c => c.Id == i);
                if (course != null)
                {
                    var semesters = course.Semesters.ToList();
                    if (semesters.Count == 0)
                    {
                        continue;
                    }
                    
                    foreach (var semester in semesters)
                    {
                        // Add exams for course
                        var exam = new Exam
                        {
                            Name = "Vize",
                            ExamType = true,
                            examCoefficient = 40,
                            DurationMinutes = 60,
                            CourseId = course.Id,
                            SemesterId = semester.Id,
                            ExamDate = DateTime.Now.AddDays(30),
                            IsExamCalculated = false
                        };
                        db.Exams.Add(exam);
                        
                        var finalExam = new Exam
                        {
                            Name = "Final",
                            ExamType = false,
                            examCoefficient = 60,
                            DurationMinutes = 120,
                            CourseId = course.Id,
                            SemesterId = semester.Id,
                            ExamDate = DateTime.Now.AddDays(60),
                            IsExamCalculated = false
                        };
                        db.Exams.Add(finalExam);
                        
                        // Also add course schedule entries
                        var courseScheduleEntry = new CourseScheduleEntry
                        {
                            CourseId = course.Id,
                            SemesterId = semester.Id,
                            Day = days[new Random().Next(0, 7)],
                            StartTime = new TimeSpan(new Random().Next(8, 18), new Random().Next(0, 60), 0),
                            EndTime = new TimeSpan(new Random().Next(18, 24), new Random().Next(0, 60), 0),
                        };
                        db.CourseScheduleEntries.Add(courseScheduleEntry);
                        
                        db.SaveChanges();
                    }
                }
            }
            
            // Add lecturers to departments according to courses
            for (int i = 1; i <= 50; i++)
            {
                var course = db.Courses.Include(c => c.Semesters).FirstOrDefault(c => c.Id == i);
                if (course != null)
                {
                    var lecturerId = course.LecturerId;
                    var lecturer = db.Lecturers.FirstOrDefault(l => l.Id == lecturerId);
                    
                    if (lecturer != null)
                    {
                        var departmentId = course.DepartmentId;
                        var department = db.Departments.FirstOrDefault(d => d.Id == departmentId);
                        if (department != null)
                        {
                            if (department.Lecturers == null)
                            {
                                department.Lecturers = new List<Lecturer>();
                            }
                            department.Lecturers.Add(lecturer);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        var facultyRepository = new FacultyRepository(db);
        var departmentRepository = new DepartmentRepository(db);
        var semesterRepository = new SemesterRepository(db);
        var adminRepository = new AdminRepository(db);
        var lecturerRepository = new LecturerRepository(db);
        var courseGroupRepository = new CourseGroupsRepository(db);
        var courseRepository = new CourseRepository(db);
        var studentRepository = new StudentRepository(db);
        var courseScheduleRepository = new CourseScheduleEntryRepository(db);
        var examRepository = new ExamRepository(db);
        var gradeRepository = new GradeRepository(db);
        var studentCourseSelectionRepository = new StudentCourseSelectionRepository(db);

        var facultyViewModel = new FacultyViewModel(facultyRepository);
        var departmentViewModel = new DepartmentViewModel(departmentRepository, facultyRepository);
        var semesterViewModel = new SemesterViewModel(semesterRepository);
        var adminViewModel = new AdminViewModel(adminRepository);
        var lecturerViewModel = new LecturerViewModel(lecturerRepository, departmentRepository);
        var courseGroupViewModel = new CourseGroupsViewModel(courseGroupRepository, departmentRepository, semesterRepository);
        var courseApproveViewModel = new CourseApproveViewModel(courseRepository);
        var studentViewModel = new StudentViewModel(studentRepository, departmentRepository);
        var courseViewModel = new CourseViewModel(departmentRepository, courseRepository, semesterRepository);
        var courseScheduleViewModel = new CourseScheduleViewModel(courseRepository, semesterRepository);
        var examScheduleViewModel = new ExamScheduleViewModel(examRepository, semesterRepository, courseRepository);
        var gradeViewModel = new GradeViewModel(gradeRepository, studentRepository, examRepository, semesterRepository, departmentRepository);
        var calculateLetterGradeViewModel = new CalculateLetterGradeViewModel(examRepository, gradeRepository, studentRepository, courseRepository);
        var courseSelectionViewModel = new StudentCourseSelectionViewModel(semesterRepository, courseRepository, studentRepository, studentCourseSelectionRepository);
        var studentCourseScheduleViewModel = new StudentCourseScheduleViewModel(semesterRepository, studentCourseSelectionRepository, courseRepository, studentRepository);
        var studentCourseExamScheduleViewModel = new StudentCourseExamScheduleViewModel(semesterRepository, studentCourseSelectionRepository, courseRepository, studentRepository);
        
        /*
        Console.WriteLine("GP Unisis Yönetim Sistemi");
        Console.WriteLine("Giriş yapın: (1 = Admin, 2 = Öğrenci, 3 = Çıkış)");
        var role = Console.ReadLine();
        if (string.IsNullOrEmpty(role))
        {
            Console.WriteLine("Geçersiz giriş. Çıkılıyor...");
            return;
        }

        switch (role)
        {
            case "1":
                Console.WriteLine("Admin girişi");
                break;
            case "2":
                Console.WriteLine("Öğrenci girişi");
                break;
            case "3":
                Console.WriteLine("Çıkılıyor...");
                return;
            default:
                Console.WriteLine("Geçersiz giriş. Çıkılıyor...");
                return;
        }
*/

        var courseId = 28;
        var students = db.StudentCourseSelections
            .Where(scs => scs.Confirmed == true && scs.SemesterId == 1)
            .Where(scs => scs.Courses.Any(c => c.Id == courseId)).Select(scs => scs.StudentId).ToList();
        Console.WriteLine("Öğrenci Listesi {0}", string.Join(", ", students));
        
        while (true)
        {
            Console.WriteLine("Sayfa numarası girin: ");
            var rl = Console.ReadLine();
            if (string.IsNullOrEmpty(rl))
            {
                Console.WriteLine("Geçersiz giriş. Çıkılıyor...");
                continue;
            }

            var pageNumber = int.Parse(rl);
            switch (pageNumber)
            {
                case 0:
                    Console.WriteLine("Çıkılıyor...");
                    return;
                case 1:
                    facultyViewModel.ListFaculties();
                    break;
                case 2:
                    facultyViewModel.AddFaculty();
                    break;
                case 3:
                    facultyViewModel.UpdateFaculty();
                    break;
                case 4:
                    facultyViewModel.DeleteFaculty();
                    break;
                case 5:
                    departmentViewModel.ListDepartments();
                    break;
                case 6:
                    departmentViewModel.AddDepartment();
                    break;
                case 7:
                    departmentViewModel.UpdateDepartment();
                    break;
                case 8:
                    departmentViewModel.DeleteDepartment();
                    break;
                case 9:
                    semesterViewModel.ListSemesters();
                    break;
                case 10:
                    semesterViewModel.AddSemester();
                    break;
                case 11:
                    semesterViewModel.UpdateSemester();
                    break;
                case 12:
                    semesterViewModel.DeleteSemester();
                    break;
                case 13:
                    adminViewModel.ListAdmins();
                    break;
                case 14:
                    adminViewModel.AddAdmin();
                    break;
                case 15:
                    adminViewModel.UpdateAdmin();
                    break;
                case 16:
                    adminViewModel.DeleteAdmin();
                    break;
                case 17:
                    lecturerViewModel.ListLecturers();
                    break;
                case 18:
                    lecturerViewModel.AddLecturer();
                    break;
                case 19:
                    lecturerViewModel.UpdateLecturer();
                    break;
                case 20:
                    lecturerViewModel.DeleteLecturer();
                    break;
                case 21:
                    courseGroupViewModel.AddCourseGroup();
                    break;
                case 22:
                    courseGroupViewModel.DeleteCourseGroup();
                    break;
                case 24:
                    courseGroupViewModel.ListCourseGroupsByDepartment();
                    break;
                case 25:
                    courseGroupViewModel.DeleteCourseGroup();
                    break;
                case 26:
                    courseApproveViewModel.AwaitingApprovalCourses();
                    break;
                case 27:
                    courseApproveViewModel.ApproveCourse();
                    break;
                case 28:
                    courseApproveViewModel.RejectCourse();
                    break;
                case 29:
                    studentViewModel.ListStudents();
                    break;
                case 30:
                    studentViewModel.AddStudent();
                    break;
                case 31:
                    studentViewModel.UpdateStudent();
                    break;
                case 32:
                    studentViewModel.DeleteStudent();
                    break;
                case 33:
                    courseViewModel.ListCourses();
                    break;
                case 34:
                    courseViewModel.AddCourse();
                    break;
                case 35:
                    courseViewModel.UpdateCourse();
                    break;
                case 36:
                    courseViewModel.DeleteCourse();
                    break;
                case 37:
                    courseScheduleViewModel.ListCourseSchedule();
                    break;
                case 38:
                    courseScheduleViewModel.AddCourseScheduleEntry();
                    break;
                case 39:
                    courseScheduleViewModel.RemoveCourseScheduleEntry();
                    break;
                case 40:
                    examScheduleViewModel.AddExamSchedule();
                    break;
                case 41:
                    examScheduleViewModel.RemoveExamSchedule();
                    break;
                case 42:
                    examScheduleViewModel.ListExamSchedule();
                    break;
                case 43:
                    calculateLetterGradeViewModel.CalculateStudentCourseGrade();
                    break;
                case 44:
                    gradeViewModel.ListExamGrades();
                    break;
                case 45:
                    gradeViewModel.AddGrade();
                    break;
                case 46:
                    gradeViewModel.DeleteGrade();
                    break;
                case 47:
                    courseSelectionViewModel.ListCourseSelections();
                    break;
                case 48:
                    courseSelectionViewModel.AddCourses();
                    break;
                case 49:
                    courseSelectionViewModel.UpdateCourses();
                    break;
                case 50:
                    courseSelectionViewModel.DeleteCourseSelection();
                    break;
                case 51:
                    courseSelectionViewModel.ListCourseSelectionsByStudentId();
                    break;
                case 52:
                    courseSelectionViewModel.ConfirmOrCancelCourseSelection();
                    break;
                case 53:
                    studentCourseScheduleViewModel.ListStudentCourseSchedule();
                    break;
                case 54:
                    studentCourseExamScheduleViewModel.ListStudentCourseSchedule();
                    break;
                default:
                    Console.WriteLine("sayfa yok");
                    break;
            }
        }
    }
}