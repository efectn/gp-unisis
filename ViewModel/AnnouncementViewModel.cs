using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class AnnouncementViewModel
{
    private readonly AnnouncementRepository _announcementRepository;
    private readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;
    private readonly CourseRepository _courseRepository;

    public AnnouncementViewModel(AnnouncementRepository announcementRepository, StudentCourseSelectionRepository studentCourseSelectionRepository, CourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
        _studentCourseSelectionRepository = studentCourseSelectionRepository;
        _announcementRepository = announcementRepository;
    }

    public void ListAnnouncements()
    {
        var announcements = _announcementRepository.GetAllAnnouncements();
        Console.WriteLine("Duyurular:");
        foreach (var a in announcements)
        {
            Console.WriteLine($"- ID: {a.Id}, Başlık: {a.Title}, Açıklama: {a.Description}");
            Console.WriteLine($"  - Süre: {a.ExpirationDate}");
            Console.WriteLine($"  - Admin: {a.Admin?.Name ?? "Yok"}, Akademisyen: {a.Lecturer?.FullName ?? "Yok"}, Ders: {a.Course?.Name ?? "Yok"}");
        }
    }

    public void AddAnnouncement()
    {
        Console.WriteLine("Yeni duyuru ekleme:");
        Console.Write("Başlık: ");
        string title = Console.ReadLine();
        Console.Write("Açıklama: ");
        string description = Console.ReadLine();
        Console.Write("Son Geçerlilik Tarihi (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime expirationDate))
        {
            Console.WriteLine("Geçersiz tarih formatı.");
            return;
        }

        Console.Write("Admin ID (varsa): ");
        int.TryParse(Console.ReadLine(), out int adminId);
        Console.Write("Akademisyen ID (varsa): ");
        int.TryParse(Console.ReadLine(), out int lecturerId);
        Console.Write("Kurs ID (varsa): ");
        int.TryParse(Console.ReadLine(), out int courseId);
        Console.Write("Öğrenci personel ID (varsa): ");
        int.TryParse(Console.ReadLine(), out int studentId);
        
        // Check if lecturer gives the course if courseId is not null
        if (lecturerId != 0 && courseId != 0)
        {
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                Console.WriteLine("Kurs bulunamadı.");
                return;
            }

            if (course.LecturerId != lecturerId)
            {
                Console.WriteLine("Bu akademisyen bu dersi vermiyor.");
                return;
            }
        }
        
        
        // Validation
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Başlık ve açıklama boş olamaz.");
            return;
        }

        var announcement = new Announcement
        {
            Title = title,
            Description = description,
            ExpirationDate = expirationDate,
            AdminId = adminId == 0 ? null : adminId,
            LecturerId = lecturerId == 0 ? null : lecturerId,
            CourseId = courseId == 0 ? null : courseId,
            StudentPersonalId = studentId == 0 ? null : studentId
        };

        try
        {
            _announcementRepository.AddAnnouncement(announcement);
            Console.WriteLine("Duyuru başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteAnnouncement()
    {
        Console.WriteLine("Duyuru silme:");
        Console.Write("Duyuru ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        try
        {
            _announcementRepository.DeleteAnnouncement(id);
            Console.WriteLine("Duyuru başarıyla silindi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
    
    public void ShowStudentAnnouncements()
    {
        Console.WriteLine("Öğrenci duyuruları:");
        Console.WriteLine("Öğrenci ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }
        
        Console.WriteLine("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out int semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        var courseSelections = _studentCourseSelectionRepository.GetSelectionsByStudentId(studentId)
            .Where(s => s.SemesterId == semesterId).SelectMany(s => s.Courses).Select(c => c.Id).ToList();
        
        // Get announcements for the selected courses or from admin, student personal
        var announcements = _announcementRepository.GetAllAnnouncements()
            .Where(a => (a.CourseId == null && a.LecturerId == null) || (a.LecturerId != null && a.CourseId != null && courseSelections.Contains(a.CourseId.Value)))
            .Where(a => DateTime.Now < a.ExpirationDate)
            .ToList();

        if (announcements.Count == 0)
        {
            Console.WriteLine("Duyuru bulunamadı.");
        }
        else
        {
            foreach (var a in announcements)
            {
                Console.WriteLine($"- ID: {a.Id}, Başlık: {a.Title}, Açıklama: {a.Description}");
                Console.WriteLine($"  - Süre: {a.ExpirationDate}");
                Console.WriteLine($"  - Admin: {a.Admin?.Name ?? "Yok"}, Akademisyen: {a.Lecturer?.FullName ?? "Yok"}, Ders: {a.Course?.Name ?? "Yok"}");
            }
        }
    }
}
