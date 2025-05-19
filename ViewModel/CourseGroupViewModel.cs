using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class CourseGroupsViewModel
{
    private readonly CourseGroupsRepository _courseGroupsRepository;
    private readonly DepartmentRepository _departmentRepository;

    public CourseGroupsViewModel(CourseGroupsRepository courseGroupsRepository, DepartmentRepository departmentRepository)
    {
        _courseGroupsRepository = courseGroupsRepository;
        _departmentRepository = departmentRepository;
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

        try
        {
            var courseGroup = new CourseGroup
            {
                Name = name,
                DepartmentId = departmentId
            };

            _courseGroupsRepository.AddCourseGroups(courseGroup);
            Console.WriteLine("Ders grubu başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateCourseGroup()
    {
        Console.Write("Ders grubu ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }

        Console.Write("Ders grubu adı: ");
        var name = Console.ReadLine();

        Console.Write("Departman ID: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }

        try
        {
            var courseGroup = new CourseGroup
            {
                Id = id,
                Name = name,
                DepartmentId = departmentId
            };

            _courseGroupsRepository.UpdateCourseGroups(courseGroup);
            Console.WriteLine("Ders grubu başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
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
            Console.WriteLine($"ID: {group.Id}, Name: {group.Name}, Department ID: {group.DepartmentId}");
        }
    }

    public void ShowCourseGroupDetails()
    {
        Console.Write("Ders grubu ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }

        try
        {
            var group = _courseGroupsRepository.GetCourseGroupById(id);
            Console.WriteLine($"ID: {group.Id}");
            Console.WriteLine($"Adı: {group.Name}");
            Console.WriteLine($"DEpartman ID: {group.DepartmentId}");
            Console.WriteLine("Dersler:");
            foreach (var course in group.Courses)
            {
                Console.WriteLine($" - {course.Name} (ID: {course.Id})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
