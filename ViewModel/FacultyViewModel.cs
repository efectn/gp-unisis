using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

// Admins can add, edit, members
public class FacultyViewModel
{
    private readonly FacultyRepository _facultyRepository;

    public FacultyViewModel(FacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    public void ListFaculties()
    {
        var faculties = _facultyRepository.GetAllFaculties();
        Console.WriteLine("Fakülteler:");
        foreach (var faculty in faculties)
        {
            Console.WriteLine($"- ID: {faculty.Id} İsim: {faculty.Name}");
        }
    }

    public void AddFaculty()
    {
        Console.WriteLine("Fakülte ekleme:");
        Console.Write("Fakülte adı: ");
        string facultyName = Console.ReadLine();
        Console.Write("Fakülte adresi: ");
        string facultyAdress = Console.ReadLine();
        Console.Write("Fakülte iletişim numarası: ");
        string facultyContactNumber = Console.ReadLine();
        Console.Write("Fakülte dekanı: ");
        string facultyDean = Console.ReadLine();
        Console.Write("Fakülte yardımcı dekanı: ");
        string facultyViceDean = Console.ReadLine();

        // Validate inputs
        if (string.IsNullOrEmpty(facultyName) || string.IsNullOrEmpty(facultyAdress) ||
            string.IsNullOrEmpty(facultyContactNumber))
        {
            Console.WriteLine("Lütfen gerekli alanları doldurun!");
            return;
        }

        var faculty = new Faculty
        {
            Name = facultyName,
            Address = facultyAdress,
            ContactNumber = facultyContactNumber,
            Dean = facultyDean,
            ViceDean = facultyViceDean
        };

        try
        {
            _facultyRepository.AddFaculty(faculty);
            Console.WriteLine("Fakülte başarıyla eklendi.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateFaculty()
    {
        Console.WriteLine("Fakülte güncelleme:");
        Console.Write("Fakülte ID: ");
        int facultyId = int.Parse(Console.ReadLine());
        var faculty = _facultyRepository.GetFacultyById(facultyId);
        if (faculty == null)
        {
            Console.WriteLine("Fakülte bulunamadı.");
            return;
        }

        Console.Write("Yeni fakülte adı: ");
        string facultyName = Console.ReadLine();
        Console.Write("Yeni fakülte adresi: ");
        string facultyAdress = Console.ReadLine();
        Console.Write("Yeni fakülte iletişim numarası: ");
        string facultyContactNumber = Console.ReadLine();
        Console.Write("Yeni fakülte dekanı: ");
        string facultyDean = Console.ReadLine();
        Console.Write("Yeni fakülte yardımcı dekanı: ");
        string facultyViceDean = Console.ReadLine();

        // Use old value if new value is empty
        facultyName = string.IsNullOrEmpty(facultyName) ? faculty.Name : facultyName;
        facultyAdress = string.IsNullOrEmpty(facultyAdress) ? faculty.Address : facultyAdress;
        facultyContactNumber = string.IsNullOrEmpty(facultyContactNumber)
            ? faculty.ContactNumber
            : facultyContactNumber;
        facultyDean = string.IsNullOrEmpty(facultyDean) ? faculty.Dean : facultyDean;
        facultyViceDean = string.IsNullOrEmpty(facultyViceDean) ? faculty.ViceDean : facultyViceDean;

        faculty.Name = facultyName;
        faculty.Address = facultyAdress;
        faculty.ContactNumber = facultyContactNumber;
        faculty.Dean = facultyDean;
        faculty.ViceDean = facultyViceDean;

        try
        {
            _facultyRepository.UpdateFaculty(faculty);
            Console.WriteLine("Fakülte başarıyla güncellendi.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteFaculty()
    {
        Console.WriteLine("Fakülte silme:");
        Console.Write("Fakülte ID: ");
        int facultyId = int.Parse(Console.ReadLine());
        var faculty = _facultyRepository.GetFacultyById(facultyId);
        if (faculty == null)
        {
            Console.WriteLine("Fakülte bulunamadı.");
            return;
        }

        _facultyRepository.DeleteFaculty(facultyId);
        Console.WriteLine("Fakülte başarıyla silindi.");
    }
}