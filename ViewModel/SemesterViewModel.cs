using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;
using System.Globalization;

namespace gp_unisis.ViewModel;

public class SemesterViewModel
{
    private readonly SemesterRepository _semesterRepository;

    public SemesterViewModel(SemesterRepository semesterRepository)
    {
        _semesterRepository = semesterRepository;
    }

    private DateTime ReadDate(string label)
    {
        Console.Write($"{label} (dd/MM/yyyy): ");
        string input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine($"{label} boş olamaz. Güncellenemedi.");
            return DateTime.MinValue;
        }
        return DateTime.ParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    public void ListSemesters()
    {
        var semesters = _semesterRepository.GetAllSemesters();
        Console.WriteLine("Dönemler:");
        foreach (var semester in semesters)
        {
            Console.WriteLine($"- ID: {semester.Id} İsim: {semester.Name} Başlangıç: {semester.StartDate:dd/MM/yyyy} " +
                              $"Bitiş: {semester.EndDate:dd/MM/yyyy} Final Sınavı: {semester.FinalExamDate:dd/MM/yyyy}");
        }
    }

    public void AddSemester()
    {
        Console.WriteLine("Yeni dönem ekleme:");
        
        Console.Write("Dönem adı: ");
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Dönem adı boş olamaz.");
            return;
        }
        
        DateTime startDate = ReadDate("Başlangıç tarihi");
        DateTime endDate = ReadDate("Bitiş tarihi");
        DateTime finalExamDate = ReadDate("Final sınav tarihi");

        var semester = new Semester
        {
            Name = name,
            StartDate = startDate,
            EndDate = endDate,
            FinalExamDate = finalExamDate
        };

        try
        {
            _semesterRepository.AddSemester(semester);
            Console.WriteLine("Dönem başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void UpdateSemester()
    {
        Console.WriteLine("Dönem güncelleme:");
        
        Console.Write("Dönem ID: ");
        var rl = Console.ReadLine();
        if (string.IsNullOrEmpty(rl))
        {
            Console.WriteLine("Dönem ID boş olamaz.");
            return;
        }
        
        int semesterId = int.Parse(rl);
        var semester = _semesterRepository.GetSemesterById(semesterId);
        if (semester == null)
        {
            Console.WriteLine("Dönem bulunamadı.");
            return;
        }
        
        Console.Write("Yeni dönem adı: ");
        var name = Console.ReadLine();

        DateTime startDate = ReadDate("Yeni başlangıç tarihi");
        DateTime endDate = ReadDate("Yeni bitiş tarihi");
        DateTime finalExamDate = ReadDate("Yeni final sınav tarihi");
        
        // Use old values if new ones are not provided
        semester.Name = string.IsNullOrEmpty(name) ? semester.Name : name;
        semester.StartDate = startDate == DateTime.MinValue ? semester.StartDate : startDate;
        semester.EndDate = endDate == DateTime.MinValue ? semester.EndDate : endDate;
        semester.FinalExamDate = finalExamDate == DateTime.MinValue ? semester.FinalExamDate : finalExamDate;
        
        try
        {
            _semesterRepository.UpdateSemester(semester);
            Console.WriteLine("Dönem başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteSemester()
    {
        Console.WriteLine("Dönem silme:");
        Console.Write("Dönem ID: ");
        var rl = Console.ReadLine();
        if (string.IsNullOrEmpty(rl))
        {
            Console.WriteLine("Dönem ID boş olamaz.");
            return;
        }
        int semesterId = int.Parse(rl);
        
        try
        {
            _semesterRepository.DeleteSemester(semesterId);
            Console.WriteLine("Dönem başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}