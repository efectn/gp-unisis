using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class StudentCourseSelectionRepository
{
    private readonly ApplicationDbContext _context;

    public StudentCourseSelectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<StudentCourseSelection> GetAllSelections()
    {
        return _context.StudentCourseSelections
            .Include(scs => scs.Student)
            .Include(scs => scs.Course)
            .Include(scs => scs.Semester)
            .ToList();
    }

    public StudentCourseSelection GetSelectionById(int id)
    {
        var courseSelection = _context.StudentCourseSelections
            .Include(scs => scs.Student)
            .Include(scs => scs.Course)
            .Include(scs => scs.Semester)
            .FirstOrDefault(scs => scs.Id == id);

        if (courseSelection == null)
        {
            throw new InvalidOperationException($"Selection with ID {id} does not exist.");
        }

        return courseSelection;
    }


    public void AddSelection(StudentCourseSelection selection)
    {
        if (selection == null)
        {
            throw new ArgumentNullException(nameof(selection));
        }

        if (selection.StudentId <= 0 || selection.CourseId <= 0 || selection.SemesterId <= 0)
        {
            throw new ArgumentException("StudentId, CourseId and SemesterId must be valid.");
        }

        var student = _context.Students.FirstOrDefault(s => s.Id == selection.StudentId);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID {selection.StudentId} does not exist.");
        }

        var course = _context.Courses.FirstOrDefault(c => c.Id == selection.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {selection.CourseId} does not exist.");
        }

        var semester = _context.Semesters.FirstOrDefault(s => s.Id == selection.SemesterId);
        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {selection.SemesterId} does not exist.");
        }

        var exist = _context.StudentCourseSelections.Any(scs =>
            scs.StudentId == selection.StudentId &&
            scs.CourseId == selection.CourseId &&
            scs.SemesterId == selection.SemesterId);

        if (exist)
        {
            throw new InvalidOperationException("This course selection already exists for the student in the given semester.");
        }

        _context.StudentCourseSelections.Add(selection);
        _context.SaveChanges();
    }

    public void UpdateSelection(StudentCourseSelection selection)
    {
        if (selection == null)
        {
            throw new ArgumentNullException(nameof(selection));
        }

        var existCourseSelection = _context.StudentCourseSelections.FirstOrDefault(scs => scs.Id == selection.Id);
        if (existCourseSelection == null)
        {
            throw new InvalidOperationException($"Selection with ID {selection.Id} does not exist.");
        }

        existCourseSelection.Confirmed = selection.Confirmed;
        existCourseSelection.Cancelled = selection.Cancelled;
        existCourseSelection.StudentId = selection.StudentId;
        existCourseSelection.CourseId = selection.CourseId;
        existCourseSelection.SemesterId = selection.SemesterId;

        _context.SaveChanges();
    }

    public void DeleteSelection(int id)
    {
        var existCourseSelection = _context.StudentCourseSelections.FirstOrDefault(scs => scs.Id == id);
        if (existCourseSelection == null)
        {
            throw new InvalidOperationException($"Selection with ID {id} does not exist.");
        }

        _context.StudentCourseSelections.Remove(existCourseSelection);
        _context.SaveChanges();
    }

    public List<StudentCourseSelection> GetSelectionsByStudentId(int id)
    {
        return _context.StudentCourseSelections
            .Where(scs => scs.StudentId == id)
            .Include(scs => scs.Course)
            .Include(scs => scs.Semester)
            .ToList();
    }

    public List<StudentCourseSelection> GetSelectionsByCourseId(int id)
    {
        return _context.StudentCourseSelections
            .Where(scs => scs.CourseId == id)
            .Include(scs => scs.Student)
            .Include(scs => scs.Semester)
            .ToList();
    }

    public List<StudentCourseSelection> GetConfirmedSelections()
    {
        return _context.StudentCourseSelections
            .Where(scs => scs.Confirmed && !scs.Cancelled)
            .Include(scs => scs.Student)
            .Include(scs => scs.Course)
            .Include(scs => scs.Semester)
            .ToList();
    }
}
