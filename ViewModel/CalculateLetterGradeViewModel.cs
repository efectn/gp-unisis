using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class CalculateLetterGradeViewModel
{
	private readonly ExamRepository _examRepository;
	private readonly GradeRepository _gradeRepository;
	private readonly StudentRepository _studentRepository;
	private readonly CourseRepository _courseRepository;

	public CalculateLetterGradeViewModel(ExamRepository examRepository, GradeRepository gradeRepository, StudentRepository studentRepository, CourseRepository courseRepository)
	{
		_examRepository = examRepository;
		_gradeRepository = gradeRepository;
		_studentRepository = studentRepository;
		_courseRepository = courseRepository;
	}
	public void CalculateStudentCourseGrade()
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
            Console.WriteLine("Bu bölümde ders bulunamadı.");
            return;
        }
        
        Console.WriteLine("Ders listesi: ");
        foreach (var course in courses)
        {
            Console.WriteLine($"Ders ID: {course.Id} Ders Adı: {course.Name}");
        }
        
		Console.Write("Ders ID: ");
		int courseId = int.Parse(Console.ReadLine());

		var students = _studentRepository.GetStudentsByDepartmentId(int.Parse(departmentId));
		if (students == null)
		{
			Console.WriteLine("Bu departmande öğrenci bulunamadı.");
			return;
		}

		Console.WriteLine("Öğrenci listesi:");
		foreach (var student in students)
		{
			Console.WriteLine($"Öğrenci ID: {student.Id} Öğrenci Adı: {student.FirstName} {student.LastName}");
		}

		Console.Write("Öğrenci ID: ");
		int studentId = int.Parse(Console.ReadLine());

		var grades = _gradeRepository.GetGradesByStudentAndCourse(studentId, courseId);

		if (grades == null || grades.Count == 0)
		{
			Console.WriteLine("Bu öğrenci için bu derse ait sınav notu bulunamadı.");
			return;
		}

		double totalWeightedScore = 0;
		int totalCoefficient = 0;

		foreach (var grade in grades)
		{
			int coefficient = grade.Exam.examCoefficient;
			double score = grade.Score;

			totalWeightedScore += score * coefficient;
			totalCoefficient += coefficient;

			Console.WriteLine($"Sınav: {grade.Exam.Name}  %{coefficient} - Not: {score}");
		}

		if (totalCoefficient == 0)
		{
			Console.WriteLine("Sınav katkı yüzdeleri sıfır. Not hesaplanamaz.");
			return;
		}

		double average = totalWeightedScore / totalCoefficient;
		string letterGrade = ConvertToLetterGrade(average);

		Console.WriteLine($"Ortalama: {average:F2} => Harf Notu: {letterGrade}");
	}
	
	private string ConvertToLetterGrade(double score)
    {
        if (score >= 90) return "AA";
        if (score >= 85) return "BA";
        if (score >= 80) return "BB";
        if (score >= 75) return "CB";
        if (score >= 70) return "CC";
        if (score >= 65) return "DC";
        if (score >= 60) return "DD";
        if (score >= 50) return "FD";
        return "FF";
    }
}