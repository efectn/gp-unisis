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
            .Include(c => c.Course)
            .Include(sem => sem.Semester)
            .Include(st => st.Student)
            .ToList();
    }

    public Grade GetGradeById(int id)
    {
        var grade = _context.Grades
            .Include(c => c.Course)
            .Include(sem => sem.Semester)
            .Include(st => st.Student)
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

        if (grade.StudentId == -1 ||
            grade.CourseId == -1 ||
            grade.SemesterId == -1 ||
            string.IsNullOrWhiteSpace(grade.ExamName) ||
            grade.Percentage < 0 || grade.Percentage > 100 ||
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
        {
            throw new ArgumentNullException(nameof(grade));
        }

        var existGrade = _context.Grades.FirstOrDefault(g => g.Id == grade.Id);
        if (existGrade == null)
        {
            throw new InvalidOperationException($"Grade with ID {grade.Id} does not exist.");
        }

        if (grade.StudentId == -1 ||
            grade.CourseId == -1 ||
            grade.SemesterId == -1 ||
            string.IsNullOrWhiteSpace(grade.ExamName) ||
            grade.Percentage < 0 || grade.Percentage > 100 ||
            grade.Score < 0 || grade.Score > 100)
        {
            throw new ArgumentException("All required properties must be valid and properly set.");
        }

        existGrade.CourseId = grade.CourseId;
        existGrade.SemesterId = grade.SemesterId;
        existGrade.StudentId = grade.StudentId;
        existGrade.ExamName = grade.ExamName;
        existGrade.Percentage = grade.Percentage;
        existGrade.Score = grade.Score;

        _context.SaveChanges();
    }

    public void DeleteGrade(int id)
    {
        var grade = _context.Grades.Find(id);
        if (grade == null)
        {
            throw new InvalidOperationException($"Grade with ID {id} does not exist.");
        }

        _context.Grades.Remove(grade);
        _context.SaveChanges();
    }

    public List<Grade> GetAllGradesByStudentId(int id)
    {
        return _context.Grades
            .Include(c => c.Course)
            .Include(sem => sem.Semester)
            .Include(st => st.Student)
            .Where(st => st.StudentId == id)
            .ToList();
    }

    public List<Grade> GetAllCourseGradesByCourseId(int id)
    {
        return _context.Grades
            .Include(c => c.Course)
            .Include(sem => sem.Semester)
            .Include(st => st.Student)
            .Where(c => c.CourseId == id)
            .ToList();
    }

    public double GetAverageScoreByCourseId(int id)
    {
        var grade = _context.Grades.FirstOrDefault(g => g.CourseId == id);
        if (grade == null)
        {
            throw new InvalidOperationException($"Course with ID {id} does not exist.");
        }

        return _context.Grades
            .Where(c => c.CourseId == id)
            .Average(c => c.Score);
    }

    public double GetStudentGANO(int id)
    {
        var grade = _context.Grades.FirstOrDefault(g => g.StudentId == id);
        if (grade == null)
        {
            throw new InvalidOperationException($"Student with ID {id} does not exist.");
        }

        return _context.Grades
            .Where(st => st.StudentId == id)
            .Average(st => st.Score * st.Percentage / 100);
    }
}