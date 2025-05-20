using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class LecturerViewModel
{
    private readonly LecturerRepository _lecturerRepository;
    private readonly DepartmentRepository _departmentRepository;

    public LecturerViewModel(LecturerRepository lecturerRepository, DepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
        _lecturerRepository = lecturerRepository;
    }

    public void ListLecturers()
    {
        var lecturers = _lecturerRepository.GetAllLecturers();
        Console.WriteLine("Akademisyenler:");
        foreach (var lecturer in lecturers)
        {
            Console.WriteLine($"- ID: {lecturer.Id}, İsim: {lecturer.FullName}, Email: {lecturer.Email}");
            Console.WriteLine($"  - Bölümler: {string.Join(", ", lecturer.Departments.Select(d => d.Name))}");
        }
    }

    public void AddLecturer()
    {
        Console.WriteLine("Yeni akademisyen ekleme:");
        Console.Write("Ad Soyad: ");
        string fullName = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        
        Console.WriteLine("Bölümler: ");
        var departments = _departmentRepository.GetAllDepartments();
        foreach (var department in departments)
        {
            Console.WriteLine($"  - ID: {department.Id} İsim: {department.Name}");
        }
        
        Console.Write("Bölüm ID'leri: ");
        var departmentIdsInput = Console.ReadLine();
        var departmentIds = departmentIdsInput?.Split(',').Select(id => int.TryParse(id.Trim(), out var parsedId) ? parsedId : (int?)null).Where(id => id.HasValue).Select(id => id.Value).ToList();
        if (departmentIds == null || departmentIds.Count == 0)
        {
            Console.WriteLine("Geçersiz bölüm ID'leri.");
            return;
        }
        
        Console.Write("Şifre: ");
        string password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Tüm alanlar doldurulmalıdır!");
            return;
        }

        var lecturer = new Lecturer
        {
            FullName = fullName,
            Email = email,
            Password = password
        };
        
        // Add departments to the lecturer
        lecturer.Departments = new List<Department>();
        foreach (var departmentId in departmentIds)
        {
            var department = _departmentRepository.GetDepartmentById(departmentId);
            if (department != null)
            {
                lecturer.Departments.Add(department);
            }
            else
            {
                Console.WriteLine($"Bölüm ID {departmentId} bulunamadı.");
            }
        }

        try
        {
            _lecturerRepository.AddLecturer(lecturer);
            Console.WriteLine("Akademisyen başarıyla eklendi.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateLecturer()
    {
        Console.WriteLine("Akademisyen güncelleme:");
        Console.Write("Akademisyen ID: ");
        if (!int.TryParse(Console.ReadLine(), out int lecturerId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var lecturer = _lecturerRepository.GetLecturerById(lecturerId);
        if (lecturer == null)
        {
            Console.WriteLine("Akademisyen bulunamadı.");
            return;
        }

        Console.Write("Yeni ad soyad : ");
        string fullName = Console.ReadLine();
        Console.Write("Yeni email : ");
        string email = Console.ReadLine();
        
        Console.WriteLine("Bölümler: ");
        var departments = _departmentRepository.GetAllDepartments();
        foreach (var department in departments)
        {
            Console.WriteLine($"  - ID: {department.Id} İsim: {department.Name}");
        }
        
        Console.Write("Yeni bölüm ID'leri (virgülle ayırarak): ");
        var departmentIdsInput = Console.ReadLine();
        var departmentIds = departmentIdsInput?.Split(',').Select(id => int.TryParse(id.Trim(), out var parsedId) ? parsedId : (int?)null).Where(id => id.HasValue).Select(id => id.Value).ToList();
        
        Console.Write("Yeni şifre : ");
        string password = Console.ReadLine();

        lecturer.FullName = string.IsNullOrWhiteSpace(fullName) ? lecturer.FullName : fullName;
        lecturer.Email = string.IsNullOrWhiteSpace(email) ? lecturer.Email : email;
        lecturer.Password = string.IsNullOrWhiteSpace(password) ? lecturer.Password : password;

        // Clear existing departments if any new ones are provided
        if (lecturer.Departments == null) lecturer.Departments = new List<Department>();
        lecturer.Departments.Clear();
        // Add new departments to the lecturer
        if (departmentIds != null && departmentIds.Count > 0)
        {
            foreach (var departmentId in departmentIds)
            {
                var department = _departmentRepository.GetDepartmentById(departmentId);
                if (department != null)
                {
                    lecturer.Departments.Add(department);
                }
                else
                {
                    Console.WriteLine($"Bölüm ID {departmentId} bulunamadı.");
                }
            }
        }
        
        try
        {
            _lecturerRepository.UpdateLecturer(lecturer);
            Console.WriteLine("Akademisyen başarıyla güncellendi.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteLecturer()
    {
        Console.WriteLine("Akademisyen silme:");
        Console.Write("Akademisyen ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var lecturer = _lecturerRepository.GetLecturerById(id);
        if (lecturer == null)
        {
            Console.WriteLine("Akademisyen bulunamadı.");
            return;
        }

        _lecturerRepository.DeleteLecturer(id);
        Console.WriteLine("Akademisyen başarıyla silindi.");
    }
}
