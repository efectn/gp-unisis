using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class CourseViewModel
{
    private readonly DepartmentRepository _departmentRepository;
    private readonly CourseRepository _courseRepository;
    private readonly SemesterRepository _semesterRepository;

    public CourseViewModel(DepartmentRepository departmentRepository, CourseRepository courseRepository, SemesterRepository semesterRepository)
    {
        _semesterRepository = semesterRepository;
        _departmentRepository = departmentRepository;
        _courseRepository = courseRepository;
    }
    
    public void ListCourses()
    {
        Console.WriteLine("Mevcut Bölümler: ");
        foreach (var department in _departmentRepository.GetAllDepartments())
        {
            Console.WriteLine($"- ID: {department.Id}, İsim: {department.Name}");
        }
        
        Console.Write("Bölüm ID girin: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Geçersiz bölüm ID'si.");
            return;
        }
        
        var courses = _courseRepository.GetCoursesByDepartment(departmentId);
        if (courses.Length == 0)
        {
            Console.WriteLine("Bu bölümde ders bulunamadı.");
            return;
        }
        
        Console.WriteLine("Bölüme ait dersler:");
        foreach (var course in courses)
        {
            Console.WriteLine($"- ID: {course.Id}, İsim: {course.Name}, Yarıyıl: {course.SemesterNumber} Akademisyen: {course.Lecturer?.FullName}");
        }
    }
    
    public void AddCourse()
    {
        Console.WriteLine("Yeni ders ekleme:");
        Console.Write("Ders adı: ");
        string name = Console.ReadLine();
        Console.Write("Ders kodu: ");
        string code = Console.ReadLine();
        Console.Write("Kredi: ");
        if (!int.TryParse(Console.ReadLine(), out int credit))
        {
            Console.WriteLine("Geçersiz kredi.");
            return;
        }
        
        Console.Write("Yarıyıl numarası: ");
        if (!int.TryParse(Console.ReadLine(), out int semesterNumber))
        {
            Console.WriteLine("Geçersiz yarıyıl numarası.");
            return;
        }
        
        Console.Write("Kontenjan: ");
        if (!int.TryParse(Console.ReadLine(), out int quota))
        {
            Console.WriteLine("Geçersiz kontenjan.");
            return;
        }
        
        Console.Write("Seçmeli mi? (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool isElective))
        {
            Console.WriteLine("Geçersiz seçim.");
            return;
        }
        
        Console.WriteLine("Mevcut bölümler:");
        var departments = _departmentRepository.GetAllDepartments();
        foreach (var department in departments)
        {
            Console.WriteLine($"- ID: {department.Id}, İsim: {department.Name}");
        }
        
        Console.Write("Bölüm ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Geçersiz bölüm ID'si.");
            return;
        }
        
        // Check if the same coded course already exists in the department
        var existingCourses = _courseRepository.GetCoursesByDepartment(departmentId).Where(c => c.Code == code);
        if (existingCourses.Any())
        {
            Console.WriteLine("Bu bölümde aynı kodlu bir ders zaten var. Yeni ders eklemek yerine var olan derse yeni dönem ekleyebilirsiniz");
            return;
        }
        
        Console.WriteLine("Mevcut dönemler:");
        var semesters = _semesterRepository.GetAllSemesters();
        foreach (var semester in semesters)
        {
            Console.WriteLine($"- ID: {semester.Id}, İsim: {semester.Name}");
        }
        
        Console.Write("Dönem ID'leri (virgülle ayırarak girin): ");
        string semesterIdsInput = Console.ReadLine();
        var semesterIds = semesterIdsInput.Split(',').Select(id => int.Parse(id.Trim())).ToArray();
        foreach (var semesterId in semesterIds)
        {
            if (!semesters.Any(s => s.Id == semesterId))
            {
                Console.WriteLine($"Dönem ID {semesterId} geçersiz.");
                return;
            }
        }
        
        Console.WriteLine("Akademisyenler:");
        var lecturers = _departmentRepository.GetLecturersByDepartmentId(departmentId);
        foreach (var lecturerS in lecturers)
        {
            Console.WriteLine($"- ID: {lecturerS.Id}, İsim: {lecturerS.FullName}");
        }
        
        Console.Write("Akademisyen ID: ");
        if (!int.TryParse(Console.ReadLine(), out int lecturerId))
        {
            Console.WriteLine("Geçersiz akademisyen ID'si.");
            return;
        }
        
        // Check if the lecturer exists
        var lecturer = lecturers.FirstOrDefault(l => l.Id == lecturerId);
        if (lecturer == null)
        {
            Console.WriteLine("Geçersiz akademisyen ID'si.");
            return;
        }
        
        Console.Write("Açıklama: ");
        string description = Console.ReadLine();
        
        var course = new Course
        {
            Name = name,
            Code = code,
            Credit = credit,
            SemesterNumber = semesterNumber,
            Quota = quota,
            IsElective = isElective,
            Description = description,
            LecturerId = lecturerId,
            DepartmentId = departmentId,
            IsConfirmed = false
        };
        
        // Add the course to the selected semesters
        course.Semesters = new List<Semester>();
        foreach (var semesterId in semesterIds)
        {
            var semester = semesters.First(s => s.Id == semesterId);
            course.Semesters.Add(semester);
        }
        
        try 
        {
            _courseRepository.AddCourse(course);
            Console.WriteLine("Ders başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException.Message);
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateCourse()
    {
        Console.WriteLine("Ders güncelleme:");
        Console.Write("Ders ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var course = _courseRepository.GetCourseById(id);
        if (course == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }

        Console.Write("Yeni ders adı (boş bırakmak için Enter'a basın): ");
        string name = Console.ReadLine();
        if (!string.IsNullOrEmpty(name))
        {
            course.Name = name;
        }

        Console.Write("Yeni ders kodu (boş bırakmak için Enter'a basın): ");
        string code = Console.ReadLine();
        if (!string.IsNullOrEmpty(code))
        {
            course.Code = code;
        }

        Console.Write("Yeni kredi (boş bırakmak için Enter'a basın): ");
        string creditInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(creditInput) && int.TryParse(creditInput, out int credit))
        {
            course.Credit = credit;
        }

        Console.Write("Yeni yarıyıl numarası (boş bırakmak için Enter'a basın): ");
        string semesterNumberInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(semesterNumberInput) && int.TryParse(semesterNumberInput, out int semesterNumber))
        {
            course.SemesterNumber = semesterNumber;
        }

        Console.Write("Yeni kontenjan (boş bırakmak için Enter'a basın): ");
        string quotaInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(quotaInput) && int.TryParse(quotaInput, out int quota))
        {
            course.Quota = quota;
        }

        Console.Write("Yeni seçmeli mi? (true/false, boş bırakmak için Enter'a basın): ");
        string isElectiveInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(isElectiveInput) && bool.TryParse(isElectiveInput, out bool isElective))
        {
            course.IsElective = isElective;
        }

        // Update the department and lecturer
        Console.WriteLine("Mevcut bölümler:");
        var departments = _departmentRepository.GetAllDepartments();
        foreach (var department in departments)
        {
            Console.WriteLine($"- ID: {department.Id}, İsim: {department.Name}");
        }

        Console.Write("Yeni bölüm ID (boş bırakmak için Enter'a basın): ");
        string departmentIdInput = Console.ReadLine();

        if (!string.IsNullOrEmpty(departmentIdInput) && int.TryParse(departmentIdInput, out int departmentId))
        {
            // Check if the same coded course already exists in the department
            var existingCourses = _courseRepository.GetCoursesByDepartment(departmentId).Where(c => c.Code == code && c.Id != id);
            if (existingCourses.Any())
            {
                Console.WriteLine(
                    "Bu bölümde aynı kodlu bir ders zaten var. Yeni ders eklemek yerine var olan derse yeni dönem ekleyebilirsiniz");
                return;
            }

            course.DepartmentId = departmentId;
        }

        Console.WriteLine("Mevcut dönemler:");
        var semesters = _semesterRepository.GetAllSemesters();
        foreach (var semester in semesters)
        {
            Console.WriteLine($"- ID: {semester.Id}, İsim: {semester.Name}");
        }

        Console.Write("Yeni dönem ID'leri (virgülle ayırarak girin, boş bırakmak için Enter'a basın): ");
        string semesterIdsInput = Console.ReadLine();

        if (!string.IsNullOrEmpty(semesterIdsInput))
        {
            var semesterIds = semesterIdsInput.Split(',').Select(id => int.Parse(id.Trim())).ToArray();
            foreach (var semesterId in semesterIds)
            {
                if (!semesters.Any(s => s.Id == semesterId))
                {
                    Console.WriteLine($"Dönem ID {semesterId} geçersiz.");
                    return;
                }
            }

            // Clear existing semesters and add new ones
            course.Semesters.Clear();
            foreach (var semesterId in semesterIds)
            {
                var semester = semesters.First(s => s.Id == semesterId);
                course.Semesters.Add(semester);
            }
        }

        Console.WriteLine("Akademisyenler:");
        var lecturers = _departmentRepository.GetLecturersByDepartmentId(course.DepartmentId);
        foreach (var lecturerS in lecturers)
        {
            Console.WriteLine($"- ID: {lecturerS.Id}, İsim: {lecturerS.FullName}");
        }

        Console.Write("Yeni akademisyen ID (boş bırakmak için Enter'a basın): ");
        string lecturerIdInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(lecturerIdInput) && int.TryParse(lecturerIdInput, out int lecturerId))
        {
            // Check if the lecturer exists
            var lecturer = lecturers.FirstOrDefault(l => l.Id == lecturerId);
            if (lecturer == null)
            {
                Console.WriteLine("Geçersiz akademisyen ID'si.");
                return;
            }

            course.LecturerId = lecturerId;
        }

        Console.Write("Yeni açıklama (boş bırakmak için Enter'a basın): ");
        string description = Console.ReadLine();
        if (!string.IsNullOrEmpty(description))
        {
            course.Description = description;
        }

        try
        {
            _courseRepository.UpdateCourse(course);
            Console.WriteLine("Ders başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
    
    public void DeleteCourse()
    {
        Console.WriteLine("Ders silme:");
        Console.Write("Ders ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var course = _courseRepository.GetCourseById(id);
        if (course == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }

        try
        {
            _courseRepository.DeleteCourse(id);
            Console.WriteLine("Ders başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}