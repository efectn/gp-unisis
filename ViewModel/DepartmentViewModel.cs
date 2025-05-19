using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class DepartmentViewModel
{
    private readonly DepartmentRepository _departmentRepository;
    private readonly FacultyRepository _facultyRepository;

    public DepartmentViewModel(DepartmentRepository departmentRepository, FacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
        _departmentRepository = departmentRepository;
    }

    public void ListDepartments()
    {
        Console.WriteLine("Bölümler:");
        foreach (var department in _departmentRepository.GetAllDepartments())
        {
            var facultyName = department.Faculty != null ? department.Faculty.Name : "Fakülte bulunamadı";
            Console.WriteLine($"- ID: {department.Id} İsim: {department.Name}, Fakülte: {facultyName}");
        }
    }

    public void AddDepartment()
    {
        Console.WriteLine("Bölüm ekleme:");
        Console.Write("Bölüm adı: ");
        string departmentName = Console.ReadLine();
        Console.Write("Bölüm adresi: ");
        string departmentAdress = Console.ReadLine();
        Console.Write("Bölüm başkanı: ");
        string departmentHead = Console.ReadLine();
        Console.Write("Bölüm başkan yardımcısı: ");
        string departmentViceHead = Console.ReadLine();
        Console.Write("Bölüm iletişim numarası: ");
        string departmentContactNumber = Console.ReadLine();

        Console.WriteLine("Fakülteler:");
        var faculties = _facultyRepository.GetAllFaculties();
        foreach (var faculty in faculties)
        {
            Console.WriteLine($"  - ID: {faculty.Id} İsim: {faculty.Name}");
        }

        Console.Write("Fakülte ID'si: ");
        var rl = Console.ReadLine();
        if (string.IsNullOrEmpty(rl))
        {
            Console.WriteLine("Geçersiz fakülte ID'si...");
            return;
        }

        int facultyId = int.Parse(rl);

        // Validate inputs
        if (string.IsNullOrEmpty(departmentName) || string.IsNullOrEmpty(departmentAdress) ||
            string.IsNullOrEmpty(departmentContactNumber) || string.IsNullOrEmpty(departmentHead) ||
            string.IsNullOrEmpty(departmentViceHead))
        {
            Console.WriteLine("Lütfen gerekli alanları doldurun!");
            return;
        }

        var department = new Department
        {
            Name = departmentName,
            Address = departmentAdress,
            ContactNumber = departmentContactNumber,
            Head = departmentHead,
            ViceHead = departmentViceHead,
            FacultyId = facultyId
        };

        try
        {
            _departmentRepository.AddDepartment(department);
            Console.WriteLine("Bölüm başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateDepartment()
    {
        Console.WriteLine("Bölüm güncelleme:");
        Console.Write("Bölüm ID: ");
        int departmentId = int.Parse(Console.ReadLine() ?? "0");

        var department = _departmentRepository.GetDepartmentById(departmentId);
        if (department == null)
        {
            Console.WriteLine("Bölüm bulunamadı.");
            return;
        }

        Console.Write("Yeni bölüm adı (boş bırakmak için Enter'a basın): ");
        string departmentName = Console.ReadLine();
        Console.Write("Yeni bölüm adresi (boş bırakmak için Enter'a basın): ");
        string departmentAdress = Console.ReadLine();
        Console.Write("Yeni bölüm iletişim numarası (boş bırakmak için Enter'a basın): ");
        string departmentContactNumber = Console.ReadLine();

        // Validate inputs
        if (!string.IsNullOrEmpty(departmentName))
            department.Name = departmentName;

        if (!string.IsNullOrEmpty(departmentAdress))
            department.Address = departmentAdress;

        if (!string.IsNullOrEmpty(departmentContactNumber))
            department.ContactNumber = departmentContactNumber;

        try
        {
            _departmentRepository.UpdateDepartment(department);
            Console.WriteLine("Bölüm başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteDepartment()
    {
        Console.WriteLine("Bölüm silme:");
        Console.Write("Bölüm ID: ");
        int departmentId = int.Parse(Console.ReadLine() ?? "0");

        var department = _departmentRepository.GetDepartmentById(departmentId);
        if (department == null)
        {
            Console.WriteLine("Bölüm bulunamadı.");
            return;
        }

        try
        {
            _departmentRepository.DeleteDepartment(departmentId);
            Console.WriteLine("Bölüm başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}