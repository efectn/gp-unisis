using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class CourseScheduleViewModel
{
    private readonly CourseRepository _courseRepository;
    private readonly SemesterRepository _semesterRepository;
    
    public CourseScheduleViewModel(CourseRepository courseRepository, SemesterRepository semesterRepository)
    {
        _courseRepository = courseRepository;
        _semesterRepository = semesterRepository;
    }

    public void ListCourseSchedule()
    {
        Console.WriteLine("Semester ID'si girin: ");
        var semesterId = Console.ReadLine();
        if (string.IsNullOrEmpty(semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }
        
        Console.WriteLine("Bölüm ID'si girin: ");
        var departmentId = Console.ReadLine();
        if (string.IsNullOrEmpty(departmentId))
        {
            Console.WriteLine("Geçersiz bölüm ID'si.");
            return;
        }
        
        var courses = _courseRepository.GetCoursesByDepartment(int.Parse(departmentId))
            .Where(c => c.Semesters.Any(s => s.Id == int.Parse(semesterId)))
            .ToArray();
        if (courses.Length == 0)
        {
            Console.WriteLine("Bu bölümde ders bulunmamaktadır.");
            return;
        }
        
        Console.WriteLine("Ders listesi: ");
        foreach (var course in courses)
        {
            Console.WriteLine($"Ders Adı: {course.Name}, ID: {course.Id}");
        }
        
        Console.WriteLine("Ders ID'si girin: ");
        var courseId = Console.ReadLine();
        if (string.IsNullOrEmpty(courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }
        
        var selectedCourse = _courseRepository.GetCourseById(int.Parse(courseId));
        if (selectedCourse == null)
        {
            Console.WriteLine("Bu ders bulunmamaktadır.");
            return;
        }
        
        Console.WriteLine($"Ders Adı: {selectedCourse.Name}");
        Console.WriteLine("Ders Programı: ");
        var entries = selectedCourse.CourseScheduleEntries
            .Where(e => e.SemesterId == int.Parse(semesterId))
            .ToArray();
        foreach (var entry in entries)
        {
            Console.WriteLine($"Gün: {entry.Day}, Saat: {entry.StartTime} - {entry.EndTime}");
        }
    }
    
    public void AddCourseScheduleEntry()
    {
        Console.WriteLine("Dönem ID'si girin: ");
        var semesterId = Console.ReadLine();
        if (string.IsNullOrEmpty(semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }
        
        var semester = _semesterRepository.GetSemesterById(int.Parse(semesterId));
        if (semester == null)
        {
            Console.WriteLine("Bu dönem bulunmamaktadır.");
            return;
        }
        
        Console.WriteLine("Ders ID'si girin: ");
        var courseId = Console.ReadLine();
        if (string.IsNullOrEmpty(courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }
        
        Console.WriteLine("Gün girin (Pazartesi, Salı, Çarşamba, Perşembe, Cuma): ");
        var day = Console.ReadLine();
        if (string.IsNullOrEmpty(day))
        {
            Console.WriteLine("Geçersiz gün.");
            return;
        }
        
        Console.WriteLine("Başlangıç saati girin (HH:MM): ");
        var startTimeInput = Console.ReadLine();
        if (string.IsNullOrEmpty(startTimeInput) || !TimeSpan.TryParse(startTimeInput, out var startTime))
        {
            Console.WriteLine("Geçersiz başlangıç saati.");
            return;
        }
        
        Console.WriteLine("Bitiş saati girin (HH:MM): ");
        var endTimeInput = Console.ReadLine();
        if (string.IsNullOrEmpty(endTimeInput) || !TimeSpan.TryParse(endTimeInput, out var endTime))
        {
            Console.WriteLine("Geçersiz bitiş saati.");
            return;
        }
        
        if (startTime >= endTime)
        {
            Console.WriteLine("Bitiş saati başlangıç saatinden önce olamaz.");
            return;
        }
        
        // Check if the course exists
        var courseIdParsed = int.Parse(courseId);
        if (courseIdParsed <= 0)
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }
        
        var course = _courseRepository.GetCourseById(courseIdParsed);
        if (course == null)
        {
            Console.WriteLine("Bu ders bulunmamaktadır.");
            return;
        }
        
        // Check if the course is already scheduled for the given semester
        if (course.Semesters.All(s => s.Id != semester.Id))
        {
            Console.WriteLine("Bu ders bu dönemde kayıtlı değildir.");
            return;
        }
        
        // Create a new schedule entry
        var newEntry = new CourseScheduleEntry
        {
            CourseId = course.Id,
            Day = day,
            StartTime = startTime,
            EndTime = endTime,
            SemesterId = semester.Id
        };
        
        // Check for overlapping schedule entries
        foreach (var entry in course.CourseScheduleEntries)
        {
            if (entry.Day == day && 
                ((startTime >= entry.StartTime && startTime < entry.EndTime) || 
                 (endTime > entry.StartTime && endTime <= entry.EndTime)))
            {
                Console.WriteLine("Bu zaman diliminde zaten bir ders programı girişi bulunmaktadır. Lütfen kontrol edin");
                return;
            }
        }
        
        course.CourseScheduleEntries.Add(newEntry);
        _courseRepository.UpdateCourse(course);
        
        Console.WriteLine("Ders programı girişi başarılı.");
    }
    
    public void RemoveCourseScheduleEntry()
    {
        Console.WriteLine("Ders ID'si girin: ");
        var courseId = Console.ReadLine();
        if (string.IsNullOrEmpty(courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }
        
        Console.WriteLine("Ders programı girişi ID'si girin: ");
        var entryId = Console.ReadLine();
        if (string.IsNullOrEmpty(entryId))
        {
            Console.WriteLine("Geçersiz ders programı girişi ID'si.");
            return;
        }
        
        var course = _courseRepository.GetCourseById(int.Parse(courseId));
        if (course == null)
        {
            Console.WriteLine("Bu ders bulunmamaktadır.");
            return;
        }
        
        var entryIdParsed = int.Parse(entryId);
        var entry = course.CourseScheduleEntries.FirstOrDefault(e => e.Id == entryIdParsed);
        if (entry == null)
        {
            Console.WriteLine("Bu ders programı girişi bulunmamaktadır.");
            return;
        }
        
        course.CourseScheduleEntries.Remove(entry);
        _courseRepository.UpdateCourse(course);
        
        Console.WriteLine("Ders programı girişi silindi.");
    }
}