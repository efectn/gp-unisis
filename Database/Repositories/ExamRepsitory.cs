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
        if (exam == null)
            throw new ArgumentNullException(nameof(exam));

        if (string.IsNullOrEmpty(exam.Name))
            throw new ArgumentException("Exam name must be provided.");

        var existExam = _context.Exams
            .FirstOrDefault(e => e.Name == exam.Name && e.Semester.Id == exam.Semester.Id);
        if (existExam != null)
            throw new InvalidOperationException($"Exam with name {exam.Name} in this semester already exists.");

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
        existExam.ExamType = exam.ExamType;
        existExam.examCoefficient = exam.examCoefficient;
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
            .FirstOrDefault(e => e.Id == id);
    }

    public List<Exam> GetExamsBySemesterId(int semesterId)
    {
        return _context.Exams
            .Include(e => e.Semester)
            .Include(e => e.Course)
            .Where(e => e.SemesterId == semesterId)
            .ToList();
    }
}