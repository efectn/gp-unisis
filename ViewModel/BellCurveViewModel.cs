using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class BellCurveViewModel
{
    private readonly ExamRepository _examRepository;
    private readonly GradeRepository _gradeRepository;
    private readonly StudentRepository _studentRepository;
    private readonly CourseRepository _courseRepository;
    private readonly DepartmentRepository _departmentRepository;
    private readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;

    public BellCurveViewModel(ExamRepository examRepository, GradeRepository gradeRepository, StudentRepository studentRepository, CourseRepository courseRepository, DepartmentRepository departmentRepository, StudentCourseSelectionRepository studentCourseSelectionRepository)
    {
        _examRepository = examRepository;
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _departmentRepository = departmentRepository;
        _studentCourseSelectionRepository = studentCourseSelectionRepository;
    }

    public void CalculateBellCurve()
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
        var courses = _courseRepository.GetAllCourses();
        foreach (var course in courses)
        {
            Console.WriteLine($"Ders ID : {course.Id} , Ders Adı : {course.Name}");
        }

        Console.WriteLine("Ders ID'si girin");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }

        double StudentTotalScore = 0;
        int StudentCount = 0;

        var students = _studentCourseSelectionRepository.GetStudentByCourseId(courseId);
        if (students == null)
        {
            Console.WriteLine("Bu derse kayıtlı herhangi bir öğrenci bulunamadı.");
            return;
        }

        List<double> averageScores = new List<double>();
        List<(Student student, double avgScore)> studentScores = new List<(Student, double)>();

        foreach (var student in students)
        {
            var grades = _gradeRepository.GetGradesByStudentAndCourse(student.Id, courseId);
            if (grades == null)
            {
                Console.WriteLine("Herhangi bir not bulunamadı.");
            }
            double avgScore = grades.Any() ? grades.Average(g => g.Score) : 0;

            StudentTotalScore += avgScore;
            StudentCount++;
            averageScores.Add(avgScore);
            studentScores.Add((student, avgScore));
        }

        var Mean = CalculateMean(StudentTotalScore, StudentCount);
        var stdDev = CalculateStdDev(Mean, averageScores, StudentCount);
        if (stdDev == 0)
        {
            Console.WriteLine("En az bir not girişi yapılmış olmalı.");
            return;
        }

        List<(Student student, double zScore)> TScores = new List<(Student, double)>();

        foreach (var (student, avgScore) in studentScores)
        {
            double zScore = stdDev != 0 ? (avgScore - Mean) / stdDev : 0;
            TScores.Add((student, CalculateTScore(zScore)));
        }

        Console.WriteLine("Her Öğrenci için harf notu listesi:");
        foreach (var (student, avgScore) in studentScores)
        {
            double zScore = stdDev != 0 ? (avgScore - Mean) / stdDev : 0;
            double TScore = CalculateTScore(zScore);
            string letterGrade = GetLetterGrade(avgScore, TScore);

            Console.WriteLine($"Öğrenci: {student.FirstName} {student.LastName} - Ortalama: {avgScore:F2} - T-Score: {TScore:F2} - Harf Notu: {letterGrade}");
        }   
    }

    public double CalculateMean(double Total, int Count)
    {
        return Total / Count;
    }
    public double CalculateStdDev(double mean, List<Double> scores, double totalCount)
    {
        if (totalCount == 0)
        {
            return 0;
        }

        double totalForStdDev = 0;
        foreach (var score in scores)
        {
            totalForStdDev += Math.Pow(score - mean, 2);
        }
        return Math.Sqrt(totalForStdDev / totalCount);
    }

    public double CalculateTScore(double ZScore)
    {
        return 50 + 10 * ZScore;
    }

    public string GetLetterGrade(double ort, double t)
    {
        if (ort >= 62.5 && ort <= 69.99)
        {
            if (t >= 61) return "AA";
            else if (t >= 56) return "BA";
            else if (t >= 51) return "BB";
            else if (t >= 46) return "CB";
            else if (t >= 41) return "CC";
            else if (t >= 36) return "DC";
            else if (t >= 31) return "DD";
            else if (t >= 26) return "FD";
            else return "FF";
        }
        else if (ort >= 57.5 && ort <= 62.49)
        {
            if (t >= 63) return "AA";
            else if (t >= 58) return "BA";
            else if (t >= 53) return "BB";
            else if (t >= 48) return "CB";
            else if (t >= 43) return "CC";
            else if (t >= 38) return "DC";
            else if (t >= 33) return "DD";
            else if (t >= 28) return "FD";
            else return "FF";
        }
        else if (ort >= 52.5 && ort <= 57.49)
        {
            if (t >= 65) return "AA";
            else if (t >= 60) return "BA";
            else if (t >= 55) return "BB";
            else if (t >= 50) return "CB";
            else if (t >= 45) return "CC";
            else if (t >= 40) return "DC";
            else if (t >= 35) return "DD";
            else if (t >= 30) return "FD";
            else return "FF";
        }
        else if (ort >= 47.5 && ort <= 52.49)
        {
            if (t >= 67) return "AA";
            else if (t >= 62) return "BA";
            else if (t >= 57) return "BB";
            else if (t >= 52) return "CB";
            else if (t >= 47) return "CC";
            else if (t >= 42) return "DC";
            else if (t >= 37) return "DD";
            else if (t >= 32) return "FD";
            else return "FF";
        }
        else if (ort >= 42.5 && ort <= 47.49)
        {
            if (t >= 69) return "AA";
            else if (t >= 64) return "BA";
            else if (t >= 59) return "BB";
            else if (t >= 54) return "CB";
            else if (t >= 49) return "CC";
            else if (t >= 44) return "DC";
            else if (t >= 39) return "DD";
            else if (t >= 34) return "FD";
            else return "FF";
        }
        else if (ort >= 0.0 && ort <= 42.49)
        {
            if (t >= 71) return "AA";
            else if (t >= 66) return "BA";
            else if (t >= 61) return "BB";
            else if (t >= 56) return "CB";
            else if (t >= 51) return "CC";
            else if (t >= 46) return "DC";
            else if (t >= 41) return "DD";
            else if (t >= 36) return "FD";
            else return "FF";
        }
        else
        {
            return "FF";
        }
    }
}