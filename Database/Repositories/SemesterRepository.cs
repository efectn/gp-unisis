using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class SemesterRepository
{
    private readonly ApplicationDbContext _context;

    public SemesterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Semester> GetAllSemesters()
    {
        return _context.Semesters
            .Include(c => c.Courses)
            .Include(g => g.Grades)
            .Include(trs => trs.Transcripts)
            .ToList();
    }

    public Semester GetSemesterById(int id)
    {
        var semester = _context.Semesters
            .Include(c => c.Courses)
            .Include(g => g.Grades)
            .Include(trs => trs.Transcripts)
            .FirstOrDefault(s => s.Id == id);

        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {id} does not exist.");
        }

        return semester;
    }

    public void AddSemester(Semester semester)
    {
        if (semester == null)
            throw new ArgumentNullException(nameof(semester));

        if (semester.StartDate == default ||
            semester.EndDate == default ||
            semester.FinalExamDate == default ||
            semester.EndDate < semester.StartDate ||
            semester.FinalExamDate < semester.StartDate ||
            semester.FinalExamDate > semester.EndDate)
        {
            throw new ArgumentException("Semester dates must be valid and consistent.");
        }
        
        // Check course selection periods
        if (semester.CourseRegistrationStartDate == default ||
            semester.CourseRegistrationEndDate == default ||
            semester.CourseRegistrationEndDate < semester.CourseRegistrationStartDate ||
            semester.CourseRegistrationStartDate < semester.StartDate ||
            semester.CourseRegistrationEndDate > semester.EndDate)
        {
            throw new ArgumentException("Course registration dates must be valid and consistent.");
        }

        _context.Semesters.Add(semester);
        _context.SaveChanges();
    }

    public void UpdateSemester(Semester semester)
    {
        if (semester == null)
        {
            throw new ArgumentNullException(nameof(semester));
        }

        var existSemester = _context.Semesters.FirstOrDefault(s => s.Id == semester.Id);
        if (existSemester == null)
        {
            throw new InvalidOperationException($"Semester with ID {semester.Id} does not exist.");
        }

        if (semester.StartDate == default ||
            semester.EndDate == default ||
            semester.FinalExamDate == default ||
            semester.EndDate < semester.StartDate ||
            semester.FinalExamDate < semester.StartDate ||
            semester.FinalExamDate > semester.EndDate)
        {
            throw new ArgumentException("Semester dates must be valid and consistent.");
        }
        
        // Check course selection periods
        if (semester.CourseRegistrationStartDate == default ||
            semester.CourseRegistrationEndDate == default ||
            semester.CourseRegistrationEndDate < semester.CourseRegistrationStartDate ||
            semester.CourseRegistrationStartDate < semester.StartDate ||
            semester.CourseRegistrationEndDate > semester.EndDate)
        {
            throw new ArgumentException("Course registration dates must be valid and consistent.");
        }

        existSemester.StartDate = semester.StartDate;
        existSemester.EndDate = semester.EndDate;
        existSemester.FinalExamDate = semester.FinalExamDate;

        _context.SaveChanges();
    }

    public void DeleteSemester(int id)
    {
        var semester = _context.Semesters.Find(id);
        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {id} does not exist.");
        }

        _context.Semesters.Remove(semester);
        _context.SaveChanges();
    }

    public bool IsFinalExamOver(int id)
    {
        var semester = _context.Semesters.Find(id);
        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {id} does not exist.");
        }

        return DateTime.Now > semester.FinalExamDate;
    }
}