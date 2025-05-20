using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class CourseGroupsViewModel
{
    private readonly CourseGroupsRepository _courseGroupsRepository;
    private readonly DepartmentRepository _departmentRepository;
    private readonly SemesterRepository _semesterRepository;

    public CourseGroupsViewModel(CourseGroupsRepository courseGroupsRepository, DepartmentRepository departmentRepository, SemesterRepository semesterRepository)
    {
        _courseGroupsRepository = courseGroupsRepository;
        _departmentRepository = departmentRepository;
        _semesterRepository = semesterRepository;
    }

    public void AddCourseGroup()
    {
        Console.Write("Ders grubu adı: ");
        var name = Console.ReadLine();

        Console.Write("Departman ID: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }
        
        // Check if the department exists
        var department = _departmentRepository.GetDepartmentById(departmentId);
        if (department == null)
        {
            Console.WriteLine("Departman bulunamadı.");
            return;
        }
        
        Console.WriteLine("Minimum alınması gereken ders sayısı: ");
        if (!int.TryParse(Console.ReadLine(), out var minCourses))
        {
            Console.WriteLine("Geçersiz sayı.");
            return;
        }
        
        Console.WriteLine("Minimum alınması gereken kredi sayısı: ");
        if (!int.TryParse(Console.ReadLine(), out var minCredits))
        {
            Console.WriteLine("Geçersiz sayı.");
            return;
        }
        
        Console.WriteLine("Hangi dönem girişliler için geçerli ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }
        
        // Check if the semester exists
        var semester = _semesterRepository.GetSemesterById(semesterId);
        if (semester == null)
        {
            Console.WriteLine("Dönem bulunamadı.");
            return;
        }
        
        // Get course list to be added to the course group
        var courses = department.Courses;
        if (courses == null || courses.Count == 0)
        {
            Console.WriteLine("Bu departmanda ders bulunmamaktadır.");
            return;
        }
        
        Console.WriteLine("Dersler: ");
        foreach (var course in courses)
        {
            Console.WriteLine($" - {course.Name} (ID: {course.Id})");
        }
        
        Console.WriteLine("Ders grubu için ders ID'si girin (birden fazla ders için virgül ile ayırın): ");
        var courseIdsInput = Console.ReadLine();
        if (string.IsNullOrEmpty(courseIdsInput))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }
        
        var courseIds = courseIdsInput.Split(',').Select(id => id.Trim()).ToList();
        
        var selectedCourses = new List<Course>();
        foreach (var courseId in courseIds)
        {
            if (!int.TryParse(courseId, out var id))
            {
                Console.WriteLine($"Geçersiz ders ID'si: {courseId}");
                continue;
            }

            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                selectedCourses.Add(course);
            }
            else
            {
                Console.WriteLine($"Ders bulunamadı: {courseId}");
            }
        }

        try
        {
            var courseGroup = new CourseGroup
            {
                Name = name,
                DepartmentId = departmentId,
                RequiredCoursesCount = minCourses,
                RequiredCredits = minCredits,
                EntranceSemesterId = semesterId,
                Courses = selectedCourses
            };

            _courseGroupsRepository.AddCourseGroups(courseGroup);
            Console.WriteLine("Ders grubu başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
    
    public void DeleteCourseGroup()
    {
        Console.Write("Ders grubu ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }

        try
        {
            _courseGroupsRepository.DeleteCourseGroup(id);
            Console.WriteLine("Ders grubu başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void ListCourseGroupsByDepartment()
    {
        Console.Write("Ders gruplarınıı listelemek için departman ID: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Geçersiz departman ID.");
            return;
        }

        var groups = _courseGroupsRepository.GetCourseGroupsByDepartmentId(departmentId);
        if (groups.Count == 0)
        {
            Console.WriteLine("Ders grubu bulunamadı.");
            return;
        }

        foreach (var group in groups)
        {
            Console.WriteLine($"ID: {group.Id}, Name: {group.Name}, Department ID: {group.DepartmentId}, Min Kredi: {group.RequiredCredits}, Min Ders Sayısı: {group.RequiredCoursesCount}");
            Console.WriteLine("Dersler:");
            foreach (var course in group.Courses)
            {
                Console.WriteLine($" - {course.Name} (ID: {course.Id})");
            }
        }
    }
}
