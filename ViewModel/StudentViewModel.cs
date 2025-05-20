using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class StudentViewModel
{
    private readonly StudentRepository _studentRepository;
    private readonly DepartmentRepository _departmentRepository;

    public StudentViewModel(StudentRepository studentRepository, DepartmentRepository departmentRepository)
    {
        _studentRepository = studentRepository;
        _departmentRepository = departmentRepository;
    }

    public void ListStudents()
    {
        var students = _studentRepository.GetAllStudents();
        Console.WriteLine("Öğrenciler:");
        foreach (var student in students)
        {
            Console.WriteLine($"- ID: {student.Id}, Ad Soyad: {student.FirstName} {student.LastName}, Email: {student.Email}, Mezun Mu: {(student.IsGraduated ? "Evet" : "Hayır")}");
            Console.WriteLine($"  - Bölüm: {student.Department?.Name}");
        }
    }

    public void AddStudent()
    {
        Console.WriteLine("Yeni öğrenci ekleme:");
        Console.Write("Ad: ");
        string firstName = Console.ReadLine();
        Console.Write("Soyad: ");
        string lastName = Console.ReadLine();
        Console.Write("Öğrenci No: ");
        string studentNumber = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Şifre: ");
        string password = Console.ReadLine();
        Console.Write("TC Kimlik No: ");
        string nationalId = Console.ReadLine();
        Console.Write("Doğum Tarihi (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
        {
            Console.WriteLine("Geçersiz tarih formatı.");
            return;
        }

        Console.WriteLine("Bölümler: ");
        var departments = _departmentRepository.GetAllDepartments();
        foreach (var department in departments)
        {
            Console.WriteLine($"  - ID: {department.Id} İsim: {department.Name}");
        }

        Console.Write("Bölüm ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Geçersiz bölüm ID'si.");
            return;
        }
        
        // Validate inputs
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(studentNumber) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(nationalId))
        {
            Console.WriteLine("Lütfen gerekli alanları doldurun!");
            return;
        }

        var student = new Student
        {
            FirstName = firstName,
            LastName = lastName,
            StudentNumber = studentNumber,
            Email = email,
            Password = password,
            NationalId = nationalId,
            DateOfBirth = dateOfBirth,
            DepartmentId = departmentId,
            IsGraduated = false
        };

        try
        {
            _studentRepository.AddStudent(student);
            Console.WriteLine("Öğrenci başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateStudent()
    {
        Console.WriteLine("Öğrenci güncelleme:");
        Console.Write("Öğrenci ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var student = _studentRepository.GetStudentById(id);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        Console.Write("Yeni ad: ");
        string firstName = Console.ReadLine();
        Console.Write("Yeni soyad: ");
        string lastName = Console.ReadLine();
        Console.Write("Yeni email: ");
        string email = Console.ReadLine();
        Console.Write("Yeni şifre: ");
        string password = Console.ReadLine();
        Console.Write("Yeni TC Kimlik No: ");
        string nationalId = Console.ReadLine();
        Console.Write("Yeni doğum tarihi (yyyy-MM-dd): ");
        string dobInput = Console.ReadLine();
        Console.Write("Mezun mu? (e/h): ");
        bool isGraduated = Console.ReadLine()?.Trim().ToLower() == "e";
        // TODO: check if graduation is valid

        student.FirstName = string.IsNullOrWhiteSpace(firstName) ? student.FirstName : firstName;
        student.LastName = string.IsNullOrWhiteSpace(lastName) ? student.LastName : lastName;
        student.Email = string.IsNullOrWhiteSpace(email) ? student.Email : email;
        student.Password = string.IsNullOrWhiteSpace(password) ? student.Password : password;
        student.NationalId = string.IsNullOrWhiteSpace(nationalId) ? student.NationalId : nationalId;
        student.DateOfBirth = DateTime.TryParse(dobInput, out var dob) ? dob : student.DateOfBirth;
        student.IsGraduated = isGraduated;

        try
        {
            _studentRepository.UpdateStudent(student);
            Console.WriteLine("Öğrenci başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteStudent()
    {
        Console.WriteLine("Öğrenci silme:");
        Console.Write("Öğrenci ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        try
        {
            _studentRepository.DeleteStudent(id);
            Console.WriteLine("Öğrenci başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}
