using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class GradeViewModel
{
    private readonly GradeRepository _gradeRepository;
    private readonly StudentRepository _studentRepository;
    private readonly ExamRepository _examRepository;
    private readonly SemesterRepository _semesterRepository;


    public GradeViewModel(GradeRepository gradeRepository, StudentRepository studentRepository, ExamRepository examRepository, SemesterRepository semesterRepository)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _examRepository = examRepository;
        _semesterRepository = semesterRepository;
    }


    public void ListExamGrades()
    {
        Console.Write("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        Console.Write("Bölüm ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Geçersiz bölüm ID'si.");
            return;
        }

        var exams = _examRepository.GetExamsByDepartmentId(departmentId)
            .Where(e => e.SemesterId == semesterId).ToList();

        if (exams.Count == 0)
        {
            Console.WriteLine("Bu bölümde belirtilen döneme ait sýnav bulunmamaktadýr.");
            return;
        }

        Console.WriteLine("Sýnav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Adý: {exam.Name}, ID: {exam.Id}");
        }

        Console.Write("Sýnav ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Geçersiz sýnav ID'si.");
            return;
        }

        var getExam = _examRepository.GetExamById(examId);
        if (getExam == null)
        {
            Console.WriteLine("Sýnav bulunamadý.");
            return;
        }

        var grades = _gradeRepository.GetGradesByExamId(getExam.Id);

        if (grades.Count == 0)
        {
            Console.WriteLine("Bu ders için herhangi bir not bulunamadý.");
            return;
        }

        Console.WriteLine("Ders notlarý:");
        foreach (var grade in grades)
        {
            Console.WriteLine($" - Öðrenci: {grade.Student.FirstName} {grade.Student.LastName}, Sonuç: {grade.Score}");
        }

    }
    public void AddGrade()
    {
        Console.Write("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        Console.Write("Bölüm ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Geçersiz bölüm ID'si.");
            return;
        }

        var exams = _examRepository.GetExamsByDepartmentId(departmentId)
            .Where(e => e.SemesterId == semesterId).ToList();

        if (exams.Count == 0)
        {
            Console.WriteLine("Bu bölümde belirtilen döneme ait sýnav bulunmamaktadýr.");
            return;
        }

        Console.WriteLine("Sýnav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Adý: {exam.Name}, ID: {exam.Id}, Türü :{exam.ExamType}");
        }

        Console.Write("Sýnav ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Geçersiz sýnav ID'si.");
            return;
        }

        var getExam = _examRepository.GetExamById(examId);
        if (getExam == null)
        {
            Console.WriteLine("Sýnav bulunamadý.");
            return;
        }

        var students = _studentRepository.GetStudentsByDepartmentId(departmentId).ToList();
        if (students == null)
        {
            Console.WriteLine("Bu bölümde öðrenci yok.");
            return;
        }

        Console.WriteLine("Dersi alan öðrenci listesi: ");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Ad: {student.FirstName} {student.LastName}");
        }

        Console.WriteLine("Not girilecek öðrenci ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz öðrenci ID'si.");
            return;
        }

        var selectedStudent = _studentRepository.StudentGetById(studentId);
        if (selectedStudent == null)
        {
            Console.WriteLine("Öðrenci bulunamadý.");
            return;
        }

        Console.Write("Not girin (0-100): ");
        if (!double.TryParse(Console.ReadLine(), out var score) || score < 0 || score > 100)
        {
            Console.WriteLine("Geçersiz not giriþi.");
            return;
        }

        var newGrade = new Grade
        {
            StudentId = studentId,
            ExamId = examId,
            Score = score
        };

        try
        {
            _gradeRepository.AddGrade(newGrade);
            Console.WriteLine("Sýnav baþarýyla eklendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            return;
        }
    }

    public void DeleteGrade()
    {
        Console.Write("Silmek istediðiniz sýnav ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Geçersiz sýnav ID'si.");
            return;
        }

        var exam = _examRepository.GetExamById(examId);
        if (exam == null)
        {
            Console.WriteLine("Sýnav bulunamadý.");
            return;
        }

        var grades = _gradeRepository.GetGradesByExamId(examId);
        if (grades.Count == 0)
        {
            Console.WriteLine("Bu sýnav için kayýtlý not bulunmamaktadýr.");
            return;
        }

        Console.WriteLine($"Sýnav Notlarý");
        foreach (var grade in grades)
        {
            Console.WriteLine($"Not ID: {grade.Id}, Öðrenci: {grade.Student.FirstName} {grade.Student.LastName}, Not: {grade.Score}");
        }

        Console.Write("Silmek istediðiniz Not ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var gradeId))
        {
            Console.WriteLine("Geçersiz not ID'si.");
            return;
        }

        var gradeToDelete = _gradeRepository.GetGradeById(gradeId);
        if (gradeToDelete == null)
        {
            Console.WriteLine("Belirtilen sýnav bulunamadý veya bu derse ait deðil.");
            return;
        }

        _gradeRepository.DeleteGrade(gradeToDelete.Id);
        Console.WriteLine("Not baþarýyla silindi.");
    }
}