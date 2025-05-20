using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class GradeRepository
{
    private readonly ApplicationDbContext _context;

    public GradeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Grade> GetAllGrades()
    {
        return _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Course)     // Exam -> Course
            .Include(g => g.Exam)
                .ThenInclude(e => e.Semester)   // Exam -> Semester
            .ToList();
    }

    public Grade GetGradeById(int id)
    {
        var grade = _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Course)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Semester)
            .FirstOrDefault(g => g.Id == id);

        if (grade == null)
        {
            throw new InvalidOperationException($"Grade with ID {id} does not exist.");
        }

        return grade;
    }

    public void AddGrade(Grade grade)
    {
        if (grade == null)
            throw new ArgumentNullException(nameof(grade));

        if (grade.StudentId == 0 ||
            grade.ExamId == 0 ||
            grade.Score < 0 || grade.Score > 100)
        {
            throw new ArgumentException("All required properties must be valid and properly set.");
        }

        _context.Grades.Add(grade);
        _context.SaveChanges();
    }

    public void UpdateGrade(Grade grade)
    {
        if (grade == null)
            throw new ArgumentNullException(nameof(grade));

        var existGrade = _context.Grades.FirstOrDefault(g => g.Id == grade.Id);
        if (existGrade == null)
            throw new InvalidOperationException($"Grade with ID {grade.Id} does not exist.");

        if (grade.StudentId == 0 ||
            grade.ExamId == 0 ||
            grade.Score < 0 || grade.Score > 100)
        {
            throw new ArgumentException("All required properties must be valid and properly set.");
        }

        existGrade.StudentId = grade.StudentId;
        existGrade.ExamId = grade.ExamId;
        existGrade.Score = grade.Score;

        _context.SaveChanges();
    }

    public void DeleteGrade(int id)
    {
        var grade = _context.Grades.Find(id);
        if (grade == null)
            throw new InvalidOperationException($"Grade with ID {id} does not exist.");

        _context.Grades.Remove(grade);
        _context.SaveChanges();
    }

    public List<Grade> GetGradesByStudentId(int studentId)
    {
        return _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Course)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Semester)
            .Where(g => g.StudentId == studentId)
            .ToList();
    }

    public List<Grade> GetGradesByExamId(int examId)
    {
        return _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Course)
            .Include(g => g.Exam)
                .ThenInclude(e => e.Semester)
            .Where(g => g.ExamId == examId)
            .ToList();
    }

    public double GetAverageScoreByExamId(int examId)
    {
        var gradesExist = _context.Grades.Any(g => g.ExamId == examId);
        if (!gradesExist)
            throw new InvalidOperationException($"No grades found for Exam with ID {examId}.");

        return _context.Grades
            .Where(g => g.ExamId == examId)
            .Average(g => g.Score);
    }

    public double GetStudentAverageScore(int studentId)
    {
        var gradesExist = _context.Grades.Any(g => g.StudentId == studentId);
        if (!gradesExist)
            throw new InvalidOperationException($"No grades found for Student with ID {studentId}.");

        return _context.Grades
            .Where(g => g.StudentId == studentId)
            .Average(g => g.Score);
    }
}
