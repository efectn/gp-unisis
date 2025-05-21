using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class GradeViewModel
{
    private readonly GradeRepository _gradeRepository;
    private readonly StudentRepository _studentRepository;
    private readonly ExamRepository _examRepository;
    private readonly SemesterRepository _semesterRepository;
    private readonly DepartmentRepository _departmentRepository;


    public GradeViewModel(GradeRepository gradeRepository, StudentRepository studentRepository, ExamRepository examRepository, SemesterRepository semesterRepository, DepartmentRepository departmentRepository)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _examRepository = examRepository;
        _semesterRepository = semesterRepository;
        _departmentRepository = departmentRepository;
    }


    public void ListExamGrades()
    {
        Console.Write("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        Console.WriteLine("Bölümler:");
        foreach (var department in _departmentRepository.GetAllDepartments())
        {
            var facultyName = department.Faculty != null ? department.Faculty.Name : "Fakülte bulunamadı";
            Console.WriteLine($"- ID: {department.Id} İsim: {department.Name}, Fakülte: {facultyName}");
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
            Console.WriteLine("Bu bölümde belirtilen döneme ait sınav bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Sınav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Adı: {exam.Name}, ID: {exam.Id}");
        }

        Console.Write("Sınav ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Geçersiz sınav ID'si.");
            return;
        }

        var getExam = _examRepository.GetExamById(examId);
        if (getExam == null)
        {
            Console.WriteLine("Sınav bulunamadı.");
            return;
        }

        var grades = _gradeRepository.GetGradesByExamId(getExam.Id);

        if (grades.Count == 0)
        {
            Console.WriteLine("Bu ders için herhangi bir not bulunamadı.");
            return;
        }

        Console.WriteLine("Ders notları:");
        foreach (var grade in grades)
        {
            Console.WriteLine($" - Öğrenci: {grade.Student.FirstName} {grade.Student.LastName}, Sonuç: {grade.Score}");
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

        Console.WriteLine("Bölümler:");
        foreach (var department in _departmentRepository.GetAllDepartments())
        {
            var facultyName = department.Faculty != null ? department.Faculty.Name : "Fakülte bulunamadı";
            Console.WriteLine($"- ID: {department.Id} İsim: {department.Name}, Fakülte: {facultyName}");
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
            Console.WriteLine("Bu bölümde belirtilen döneme ait sınav bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Sınav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Adı: {exam.Name}, ID: {exam.Id}, Türü :{exam.ExamType}");
        }

        Console.Write("Sınav ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Geçersiz sınav ID'si.");
            return;
        }

        var getExam = _examRepository.GetExamById(examId);
        if (getExam == null)
        {
            Console.WriteLine("Sınav bulunamadı.");
            return;
        }

        var students = _studentRepository.GetStudentsByDepartmentId(departmentId).ToList();
        if (students == null)
        {
            Console.WriteLine("Bu bölümde öğrenci yok.");
            return;
        }

        Console.WriteLine("Dersi alan öğrenci listesi: ");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Ad: {student.FirstName} {student.LastName}");
        }

        Console.WriteLine("Not girilecek öğrenci ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var selectedStudent = _studentRepository.StudentGetById(studentId);
        if (selectedStudent == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        Console.Write("Not girin (0-100): ");
        if (!double.TryParse(Console.ReadLine(), out var score) || score < 0 || score > 100)
        {
            Console.WriteLine("Geçersiz not giriii.");
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
            Console.WriteLine("Not başarıyla eklendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            return;
        }
    }

    public void DeleteGrade()
    {
        Console.Write("Silmek istediğiniz sınav ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Geçersiz sınav ID'si.");
            return;
        }

        var exam = _examRepository.GetExamById(examId);
        if (exam == null)
        {
            Console.WriteLine("Sınav bulunamad..");
            return;
        }

        var grades = _gradeRepository.GetGradesByExamId(examId);
        if (grades.Count == 0)
        {
            Console.WriteLine("Bu sınav için kayıtlı not bulunmamaktadır.");
            return;
        }

        Console.WriteLine($"Sınav Notlar: ");
        foreach (var grade in grades)
        {
            Console.WriteLine($"Not ID: {grade.Id}, Öğrenci: {grade.Student.FirstName} {grade.Student.LastName}, Not: {grade.Score}");
        }

        Console.Write("Silmek istediğiniz Not ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var gradeId))
        {
            Console.WriteLine("Geçersiz not ID'si.");
            return;
        }

        var gradeToDelete = _gradeRepository.GetGradeById(gradeId);
        if (gradeToDelete == null)
        {
            Console.WriteLine("Belirtilen sınav bulunamadı veya bu derse ait değil.");
            return;
        }

        _gradeRepository.DeleteGrade(gradeToDelete.Id);
        Console.WriteLine("Not başarıyla silindi.");
    }
}