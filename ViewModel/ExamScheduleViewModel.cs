using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class ExamScheduleViewModel
{
    private readonly ExamRepository _examRepository;
    private readonly SemesterRepository _semesterRepository;
    private readonly CourseRepository _courseRepository;

    public ExamScheduleViewModel(ExamRepository examRepository, SemesterRepository semesterRepository, CourseRepository courseRepository )
    {
        _examRepository = examRepository;
        _courseRepository = courseRepository;
        _semesterRepository = semesterRepository;
    }


    public void ListExamSchedule()
    {
        Console.WriteLine("Dönem ID'si girin: ");
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

        var exams = _examRepository.GetExamsByDepartmentId(int.Parse(departmentId))
            .Where(e => e.SemesterId == int.Parse(semesterId)).ToList();

        if (exams.Count == 0)
        {
            Console.WriteLine("Bu bölümde belirtilen döneme ait sınav bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Sınav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Adı: {exam.Name}, ID: {exam.Id}");
        }

        Console.WriteLine("Sınav ID'si girin: ");
        var examId = Console.ReadLine();
        if (string.IsNullOrEmpty(examId))
        {
            Console.WriteLine("Geçersiz sınav ID'si.");
            return;
        }

        var selectedExam = _examRepository.GetExamById(int.Parse(examId));
        if (selectedExam == null)
        {
            Console.WriteLine("Böyle bir sınav sınav bulunmamaktadır.");
            return;
        }

        Console.WriteLine($"Sınav Adı: {selectedExam.Name}");
        Console.WriteLine("Sınav Programı:");

        if (selectedExam.SemesterId == int.Parse(semesterId))
        {
            string tarih = selectedExam.ExamDate.ToString("dd.MM.yyyy");

            Console.WriteLine($"Tarih: {tarih}, Süre: {selectedExam.DurationMinutes} dakika");
        }
        else
        {
            Console.WriteLine("Bu sınav belirtilen döneme ait değil.");
        }
    }

    public void AddExamSchedule()
    {
        Console.WriteLine("Dönem ID'si girin: ");
        var semesterId = Console.ReadLine();
        if (!int.TryParse(semesterId, out int parsedSemesterId))
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

        Console.WriteLine("Ders ID'si girin: ");
        var courseId = Console.ReadLine();
        if (!int.TryParse(courseId, out int parsedCourseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }

        var course = _courseRepository.GetCourseById(parsedCourseId);
        if (course == null)
        {
            Console.WriteLine("Bu ders bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Sınav adı: ");
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Geçersiz sınav adı.");
            return;
        }

        Console.WriteLine("Sınav tarihi girin (GG.AA.YYYY): ");
        var dateInput = Console.ReadLine();

        Console.WriteLine("Başlangıç saati girin (HH:MM): ");
        var startTimeInput = Console.ReadLine();
        if (string.IsNullOrEmpty(dateInput) || string.IsNullOrEmpty(startTimeInput) ||
            !DateTime.TryParseExact(dateInput + " " + startTimeInput, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime examDate))
        {
            Console.WriteLine("Geçersiz tarih veya saat formatı.");
            return;
        }

        Console.WriteLine("Sınav süresi girin (dakika): ");
        var examDuration = Console.ReadLine();
        if (!int.TryParse(examDuration, out int parsedDuration))
        {
            Console.WriteLine("Geçersiz sınav süresi.");
            return;
        }

        var semester = _semesterRepository.GetSemesterById(parsedSemesterId);
        if (semester == null)
        {
            Console.WriteLine("Böyle bir dönem bulunamadı.");
            return;
        }

        // Check if the course is available in the semester
        if (course.Semesters == null || course.Semesters.Any(s => s.Id != parsedSemesterId))
        {
            Console.WriteLine("Bu ders belirtilen dönemde mevcut değil.");
            return;
        }

        Console.WriteLine("Sınavın yüzdelik etkisi:");
        var examCoefficient = Console.ReadLine();
        if (!int.TryParse(examCoefficient, out int parsedExamCoefficient))
        {
            Console.WriteLine("Geçersiz yüzdelik etki.");
            return;
        }
        if (parsedExamCoefficient < 0 || parsedExamCoefficient > 100)
        {
            Console.WriteLine("Yüzdelik etki 0 ile 100 arasında olmalıdır.");
            return;
        }
        
        var newExam = new Exam
        {
            Name = name,
            SemesterId = parsedSemesterId,
            CourseId = course.Id,
            ExamDate = examDate,
            DurationMinutes = parsedDuration,
            ExamCoefficient = parsedExamCoefficient,
        };

        try
        {
            _examRepository.AddExam(newExam);
            Console.WriteLine("Sınav başarıyla eklendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
        }
    }

    public void RemoveExamSchedule()
    {
        Console.WriteLine("Ders ID'si girin: ");
        var courseId = Console.ReadLine();
        if (!int.TryParse(courseId, out int parsedCourseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }

        var course = _courseRepository.GetCourseById(parsedCourseId);
        if (course == null)
        {
            Console.WriteLine("Bu ders bulunmamaktadır.");
            return;
        }

        var exams = _examRepository.GetExamsByCourseId(parsedCourseId).ToList();
        if (!exams.Any())
        {
            Console.WriteLine("Bu derse ait sınav bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Bu derse ait sınavlar:");
        foreach (var exam in exams)
        {
            Console.WriteLine($"ID: {exam.Id}, Adı: {exam.Name}, Tarih: {exam.ExamDate:dd.MM.yyyy HH:mm}");
        }

        Console.WriteLine("Silmek istediğiniz sınavın ID'sini girin: ");
        var examIdInput = Console.ReadLine();
        if (!int.TryParse(examIdInput, out int examId))
        {
            Console.WriteLine("Geçersiz sınav ID'si.");
            return;
        }

        var examToDelete = _examRepository.GetExamById(examId);
        if (examToDelete == null || examToDelete.CourseId != parsedCourseId)
        {
            Console.WriteLine("Belirtilen sınav bulunamadı veya bu derse ait değil.");
            return;
        }

        _examRepository.DeleteExam(examToDelete);
        Console.WriteLine("Sınav başarıyla silindi.");
    }

}