using System.Security.Cryptography;
using System.Text;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class StudentPersonalViewModel
{
    private readonly StudentPersonalRepository _studentPersonalRepository;

    public StudentPersonalViewModel(StudentPersonalRepository studentPersonalRepository)
    {
        _studentPersonalRepository = studentPersonalRepository;
    }

    public void ListStudentPersonals()
    {
        var studentPersonals = _studentPersonalRepository.GetAllStudentPersonalsName();
        Console.WriteLine("StudentPersonaller:");
        foreach (var studentPersonal in studentPersonals)
        {
            Console.WriteLine($"- ID: {studentPersonal.Id} | İsim: {studentPersonal.Name} | Email: {studentPersonal.Email}");
        }
    }

    public void AddStudentPersonal()
    {
        Console.WriteLine("Yeni studentPersonal ekleme:");
        Console.Write("İsim: ");
        string name = Console.ReadLine();

        Console.Write("Email: ");
        string email = Console.ReadLine();

        Console.Write("Şifre: ");
        string password = Console.ReadLine();
        
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Tüm alanlar doldurulmalıdır.");
            return;
        }
        
        // Check if email is already in use
        if (_studentPersonalRepository.GetAllStudentPersonalsName().Any(a => a.Email == email))
        {
            Console.WriteLine("Bu email zaten kullanılıyor.");
            return;
        }

        string hashedPassword = HashPasswordMD5(password);

        var studentPersonal = new StudentPersonal
        {
            Name = name,
            Email = email,
            Password = hashedPassword
        };

        try
        {
            _studentPersonalRepository.AddStudentPersonal(studentPersonal);
            Console.WriteLine("StudentPersonal başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateStudentPersonal()
    {
        Console.WriteLine("StudentPersonal güncelleme:");
        Console.Write("StudentPersonal ID: ");
        int id = int.Parse(Console.ReadLine());

        // Fetch current studentPersonal info
        var existing = _studentPersonalRepository.GetAllStudentPersonalsName().FirstOrDefault(a => a.Id == id);
        if (existing == null)
        {
            Console.WriteLine("StudentPersonal bulunamadı.");
            return;
        }

        Console.Write("Yeni isim (boş bırakılırsa mevcut kalır): ");
        string name = Console.ReadLine();

        Console.Write("Yeni email (boş bırakılırsa mevcut kalır): ");
        string email = Console.ReadLine();

        Console.Write("Yeni şifre (boş bırakılırsa mevcut kalır): ");
        string password = Console.ReadLine();

        existing.Name = string.IsNullOrEmpty(name) ? existing.Name : name;
        existing.Email = string.IsNullOrEmpty(email) ? existing.Email : email;

        if (!string.IsNullOrEmpty(password))
        {
            existing.Password = HashPasswordMD5(password);
        }

        try
        {
            _studentPersonalRepository.UpdateStudentPersonal(existing);
            Console.WriteLine("StudentPersonal başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteStudentPersonal()
    {
        Console.WriteLine("StudentPersonal silme:");
        Console.Write("StudentPersonal ID: ");
        int id = int.Parse(Console.ReadLine());

        var studentPersonal = _studentPersonalRepository.GetAllStudentPersonalsName().FirstOrDefault(a => a.Id == id);
        if (studentPersonal == null)
        {
            Console.WriteLine("StudentPersonal bulunamadı.");
            return;
        }

        try
        {
            _studentPersonalRepository.DeleteStudentPersonal(studentPersonal);
            Console.WriteLine("StudentPersonal başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    private string HashPasswordMD5(string password)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
