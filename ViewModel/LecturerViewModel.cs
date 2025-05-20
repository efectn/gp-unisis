using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class LecturerViewModel
{
    private readonly LecturerRepository _lecturerRepository;

    public LecturerViewModel(LecturerRepository lecturerRepository)
    {
        _lecturerRepository = lecturerRepository;
    }

    public void ListLecturers()
    {
        var lecturers = _lecturerRepository.GetAllLecturers();
        Console.WriteLine("Akademisyenler:");
        foreach (var lecturer in lecturers)
        {
            Console.WriteLine($"- ID: {lecturer.Id}, İsim: {lecturer.FullName}, Email: {lecturer.Email}");
        }
    }

    public void AddLecturer()
    {
        Console.WriteLine("Yeni akademisyen ekleme:");
        Console.Write("Ad Soyad: ");
        string fullName = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
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
        Console.Write("Yeni şifre : ");
        string password = Console.ReadLine();

        lecturer.FullName = string.IsNullOrWhiteSpace(fullName) ? lecturer.FullName : fullName;
        lecturer.Email = string.IsNullOrWhiteSpace(email) ? lecturer.Email : email;
        lecturer.Password = string.IsNullOrWhiteSpace(password) ? lecturer.Password : password;

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
