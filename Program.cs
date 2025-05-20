using gp_unisis.Database;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;
using gp_unisis.ViewModel;

namespace Bruh;

public class Bruh
{
    public static void Main()
    {
        var db = new ApplicationDbContext();
        db.Database.EnsureCreated();

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

        var facultyViewModel = new FacultyViewModel(facultyRepository);
        var departmentViewModel = new DepartmentViewModel(departmentRepository, facultyRepository);
        var semesterViewModel = new SemesterViewModel(semesterRepository);
        var adminViewModel = new AdminViewModel(adminRepository);
        var lecturerViewModel = new LecturerViewModel(lecturerRepository, departmentRepository);
        var courseGroupViewModel = new CourseGroupsViewModel(courseGroupRepository, departmentRepository);
        var courseApproveViewModel = new CourseApproveViewModel(courseRepository);
        var studentViewModel = new StudentViewModel(studentRepository, departmentRepository);
        var courseViewModel = new CourseViewModel(departmentRepository, courseRepository, semesterRepository);
        var courseScheduleViewModel = new CourseScheduleViewModel(courseRepository, semesterRepository);
        var examScheduleViewModel = new ExamScheduleViewModel(examRepository, semesterRepository, courseRepository);
        
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
                case 23:
                    courseGroupViewModel.UpdateCourseGroup();
                    break;
                case 24:
                    courseGroupViewModel.ListCourseGroupsByDepartment();
                    break;
                case 25:
                    courseGroupViewModel.ShowCourseGroupDetails();
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
                default:
                    Console.WriteLine("sayfa yok");
                    break;
            }
        }
    }
}