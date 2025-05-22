using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class ExamRepository
{
    private readonly ApplicationDbContext _context;

    public ExamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddExam(Exam exam)
    {
        var existExam = _context.Exams
            .FirstOrDefault(e => e.Name == exam.Name && e.SemesterId == exam.SemesterId);

        if (existExam != null)
            throw new InvalidOperationException($"Exam with name {exam.Name} in this semester already exists.");

        // Check if coefficient doesnt exceed 100 for the same semester and course
        var existingExams = _context.Exams
            .Where(e => e.CourseId == exam.CourseId && e.SemesterId == exam.SemesterId)
            .ToList();
        var totalCoefficient = existingExams.Sum(e => e.ExamCoefficient) + exam.ExamCoefficient;
        if (totalCoefficient > 100)
            throw new InvalidOperationException(
                "Total exam coefficient for the same semester and course cannot exceed 100.");

        _context.Exams.Add(exam);
        _context.SaveChanges();
    }

    public void UpdateExam(Exam exam)
    {
        if (exam == null)
            throw new ArgumentNullException(nameof(exam));

        var existExam = _context.Exams.FirstOrDefault(e => e.Id == exam.Id);
        if (existExam == null)
            throw new InvalidOperationException($"Exam with ID {exam.Id} does not exist.");

        if (string.IsNullOrEmpty(exam.Name))
            throw new ArgumentException("Exam name must be provided.");

        existExam.Name = exam.Name;
        existExam.ExamCoefficient = exam.ExamCoefficient;
        existExam.ExamDate = exam.ExamDate;
        existExam.DurationMinutes = exam.DurationMinutes;
        existExam.CourseId = exam.CourseId;
        existExam.Semester = exam.Semester;

        _context.SaveChanges();
    }

    public void DeleteExam(Exam exam)
    {
        if (exam == null)
            throw new ArgumentNullException(nameof(exam));

        var existExam = _context.Exams.FirstOrDefault(e => e.Id == exam.Id);
        if (existExam == null)
            throw new InvalidOperationException($"Exam with ID {exam.Id} does not exist.");

        _context.Exams.Remove(existExam);
        _context.SaveChanges();
    }

    public List<Exam> GetAllExams()
    {
        return _context.Exams
            .Include(e => e.Course)
            .Include(e => e.Semester)
            .ToList();
    }

    public Exam GetExamById(int id)
    {
        return _context.Exams
            .Include(e => e.Course)
            .Include(e => e.Semester)
            .Include(e => e.Grades)
            .FirstOrDefault(e => e.Id == id);
    }

    public List<Exam> GetExamsByCourseId(int courseId)
    {
        return _context.Exams.Where(e => e.CourseId == courseId).ToList();
    }

    public List<Exam> GetExamsBySemesterId(int semesterId)
    {
        return _context.Exams
            .Include(e => e.Semester)
            .Include(e => e.Course)
            .Where(e => e.SemesterId == semesterId)
            .ToList();
    }

    public List<Exam> GetExamsByDepartmentId(int departmentId)
    {
        return _context.Exams
            .Include(e => e.Semester)
            .Include(e => e.Course)
                .ThenInclude(c => c.Department)
            .Include(e => e.Grades)
                .ThenInclude(e => e.Student)
            .Where(e => e.Course.DepartmentId == departmentId)
            .ToList();
    }
}