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
        Console.Write("D�nem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Ge�ersiz d�nem ID'si.");
            return;
        }

        Console.WriteLine("Bölümler:");
        foreach (var department in _departmentRepository.GetAllDepartments())
        {
            var facultyName = department.Faculty != null ? department.Faculty.Name : "Fakülte bulunamadı";
            Console.WriteLine($"- ID: {department.Id} İsim: {department.Name}, Fakülte: {facultyName}");
        }

        Console.Write("B�l�m ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Ge�ersiz b�l�m ID'si.");
            return;
        }

        var exams = _examRepository.GetExamsByDepartmentId(departmentId)
            .Where(e => e.SemesterId == semesterId).ToList();

        if (exams.Count == 0)
        {
            Console.WriteLine("Bu b�l�mde belirtilen d�neme ait s�nav bulunmamaktad�r.");
            return;
        }

        Console.WriteLine("S�nav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Ad�: {exam.Name}, ID: {exam.Id}");
        }

        Console.Write("S�nav ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Ge�ersiz s�nav ID'si.");
            return;
        }

        var getExam = _examRepository.GetExamById(examId);
        if (getExam == null)
        {
            Console.WriteLine("S�nav bulunamad�.");
            return;
        }

        var grades = _gradeRepository.GetGradesByExamId(getExam.Id);

        if (grades.Count == 0)
        {
            Console.WriteLine("Bu ders i�in herhangi bir not bulunamad�.");
            return;
        }

        Console.WriteLine("Ders notlar�:");
        foreach (var grade in grades)
        {
            Console.WriteLine($" - ��renci: {grade.Student.FirstName} {grade.Student.LastName}, Sonu�: {grade.Score}");
        }

    }
    public void AddGrade()
    {
        Console.Write("D�nem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Ge�ersiz d�nem ID'si.");
            return;
        }

        Console.WriteLine("Bölümler:");
        foreach (var department in _departmentRepository.GetAllDepartments())
        {
            var facultyName = department.Faculty != null ? department.Faculty.Name : "Fakülte bulunamadı";
            Console.WriteLine($"- ID: {department.Id} İsim: {department.Name}, Fakülte: {facultyName}");
        }

        Console.Write("B�l�m ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var departmentId))
        {
            Console.WriteLine("Ge�ersiz b�l�m ID'si.");
            return;
        }

        var exams = _examRepository.GetExamsByDepartmentId(departmentId)
            .Where(e => e.SemesterId == semesterId).ToList();

        if (exams.Count == 0)
        {
            Console.WriteLine("Bu b�l�mde belirtilen d�neme ait s�nav bulunmamaktad�r.");
            return;
        }

        Console.WriteLine("S�nav listesi: ");
        foreach (var exam in exams)
        {
            Console.WriteLine($"Ders Ad�: {exam.Name}, ID: {exam.Id}, T�r� :{exam.ExamType}");
        }

        Console.Write("S�nav ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Ge�ersiz s�nav ID'si.");
            return;
        }

        var getExam = _examRepository.GetExamById(examId);
        if (getExam == null)
        {
            Console.WriteLine("S�nav bulunamad�.");
            return;
        }

        var students = _studentRepository.GetStudentsByDepartmentId(departmentId).ToList();
        if (students == null)
        {
            Console.WriteLine("Bu b�l�mde ��renci yok.");
            return;
        }

        Console.WriteLine("Dersi alan ��renci listesi: ");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Ad: {student.FirstName} {student.LastName}");
        }

        Console.WriteLine("Not girilecek ��renci ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Ge�ersiz ��renci ID'si.");
            return;
        }

        var selectedStudent = _studentRepository.StudentGetById(studentId);
        if (selectedStudent == null)
        {
            Console.WriteLine("��renci bulunamad�.");
            return;
        }

        Console.Write("Not girin (0-100): ");
        if (!double.TryParse(Console.ReadLine(), out var score) || score < 0 || score > 100)
        {
            Console.WriteLine("Ge�ersiz not giri�i.");
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
            Console.WriteLine("S�nav ba�ar�yla eklendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            return;
        }
    }

    public void DeleteGrade()
    {
        Console.Write("Silmek istedi�iniz s�nav ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var examId))
        {
            Console.WriteLine("Ge�ersiz s�nav ID'si.");
            return;
        }

        var exam = _examRepository.GetExamById(examId);
        if (exam == null)
        {
            Console.WriteLine("S�nav bulunamad�.");
            return;
        }

        var grades = _gradeRepository.GetGradesByExamId(examId);
        if (grades.Count == 0)
        {
            Console.WriteLine("Bu s�nav i�in kay�tl� not bulunmamaktad�r.");
            return;
        }

        Console.WriteLine($"S�nav Notlar�");
        foreach (var grade in grades)
        {
            Console.WriteLine($"Not ID: {grade.Id}, ��renci: {grade.Student.FirstName} {grade.Student.LastName}, Not: {grade.Score}");
        }

        Console.Write("Silmek istedi�iniz Not ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var gradeId))
        {
            Console.WriteLine("Ge�ersiz not ID'si.");
            return;
        }

        var gradeToDelete = _gradeRepository.GetGradeById(gradeId);
        if (gradeToDelete == null)
        {
            Console.WriteLine("Belirtilen s�nav bulunamad� veya bu derse ait de�il.");
            return;
        }

        _gradeRepository.DeleteGrade(gradeToDelete.Id);
        Console.WriteLine("Not ba�ar�yla silindi.");
    }
}