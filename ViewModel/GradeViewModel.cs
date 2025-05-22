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
    private readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;
    private readonly CourseRepository _courseRepository;


    public GradeViewModel(GradeRepository gradeRepository, StudentRepository studentRepository, ExamRepository examRepository, SemesterRepository semesterRepository, DepartmentRepository departmentRepository, StudentCourseSelectionRepository studentCourseSelectionRepository, CourseRepository courseRepository)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _examRepository = examRepository;
        _semesterRepository = semesterRepository;
        _departmentRepository = departmentRepository;
        _studentCourseSelectionRepository = studentCourseSelectionRepository;
        _courseRepository = courseRepository;
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

        var grades = getExam.Grades.ToList();
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

        Console.WriteLine("Dersler:");
        var courses = _courseRepository.GetCoursesByDepartment(departmentId)
            .Where(c => c.Semesters.Any(s => s.Id == semesterId)).ToList();
        foreach (var course in courses)
        {
            Console.WriteLine($"- ID: {course.Id} İsim: {course.Name}");
        }

        Console.Write("Ders ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }

        var courseSelected = _courseRepository.GetCourseById(courseId);
        if (courseSelected == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }


        var exams = _examRepository.GetExamsByCourseId(courseId)
            .Where(e => e.SemesterId == semesterId).ToList();

        if (exams.Count == 0)
        {
            Console.WriteLine("Bu bölümde belirtilen derse ve döneme ait sınav bulunmamaktadır.");
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

        var students = _studentCourseSelectionRepository.GetAllSelections()
            .Where(s => s.SemesterId == semesterId && s.Confirmed && s.Courses.Any(c => c.Id == courseId))
            .Select(s => s.Student).ToList();
        if (students.Count == 0)
        {
            Console.WriteLine("Bu ders için kayıtlı öğrenci bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Öğrenciler:");
        foreach (var student in students)
        {
            Console.WriteLine($"- ID: {student.Id} İsim: {student.FirstName} {student.LastName}");
        }

        Console.Write("Öğrenci ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }
        
        // Check ID is in students
        var studentExists = students.Any(s => s.Id == studentId);
        if (!studentExists)
        {
            Console.WriteLine("Bu öğrenci belirtilen derse kayıtlı değil.");
            return;
        }

        var studentSelected = _studentRepository.GetStudentById(studentId);
        if (studentSelected == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        Console.Write("Notu girin: ");
        if (!int.TryParse(Console.ReadLine(), out var score))
        {
            Console.WriteLine("Geçersiz not.");
            return;
        }

        if (score < 0 || score > 100)
        {
            Console.WriteLine("Not 0 ile 100 arasında olmalıdır.");
            return;
        }

        // Check if the student already has a grade for this exam
        var existingGrade = _gradeRepository.GetGradesByExamId(examId)
            .FirstOrDefault(g => g.StudentId == studentId && g.ExamId == examId);
        if (existingGrade != null)
        {
            Console.WriteLine("Bu öğrenci için bu sınavda zaten bir not var, güncelleniyor.");
            existingGrade.Score = score;
            _gradeRepository.UpdateGrade(existingGrade);
            Console.WriteLine("Not başarıyla güncellendi.");
            return;
        }

        var grade = new Grade
        {
            ExamId = examId,
            StudentId = studentId,
            Score = score
        };

        try 
        {
            _gradeRepository.AddGrade(grade);
            Console.WriteLine("Not başarıyla eklendi.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteGrade()
    {
        Console.Write("Silmek istediğiniz not için sınav ID'sini girin: ");
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

        // Can't delete exam if it is already calculated
        if (exam.IsExamCalculated)
        {
            Console.WriteLine("Bu sınavın notları hesaplandığı için silinemez. Yalnızca aktif dönem içinde değiştirebilirsiniz");
            return;
        }

        if (exam.Grades.Count == 0)
        {
            Console.WriteLine("Bu sınav için kayıtlı not bulunmamaktadır.");
            return;
        }

        Console.WriteLine($"Sınav Notlar: ");
        foreach (var grade in exam.Grades)
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

    public void ShowGradesStudent()
    {
        Console.WriteLine("Dönem ID'si");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }
        
        Console.WriteLine("Öğrenci ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }
        
        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        var courses = student.StudentCourseSelections.FirstOrDefault(scs => scs.SemesterId == semesterId && scs.Confirmed)
            .Courses.ToList();

        foreach (var course in courses)
        {
            var exams = course.Exams;
            if (exams.Count == 0)
            {
                continue;
            }

            Console.WriteLine($"Ders: {course.Name}");
            foreach (var exam in exams)
            {
                var grades = exam.Grades.FirstOrDefault(g => g.StudentId == studentId);
                if (grades != null)
                {
                    Console.WriteLine($" - Sınav: {exam.Name}, Not: {grades.Score}");
                }
            }
        }
    }
}