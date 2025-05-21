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
            .Include(scs => scs.Courses)
            .Include(scs => scs.Semester)
            .ToList();
    }

    public StudentCourseSelection GetSelectionById(int id)
    {
        var courseSelection = _context.StudentCourseSelections
            .Include(scs => scs.Student)
            .Include(scs => scs.Courses)
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

        var semester = _context.Semesters.FirstOrDefault(s => s.Id == selection.SemesterId);
        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {selection.SemesterId} does not exist.");
        }

        // Check courses exists for that semester
        foreach (var course in selection.Courses)
        {
            if (!course.Semesters.Any(s => s.Id == selection.SemesterId))
            {
                throw new InvalidOperationException(
                    $"Course with ID {course.Id} does not exist in semester with ID {selection.SemesterId}.");
            }
        }

        // Remove redundant courses
        selection.Courses = selection.Courses.Distinct().ToList();

        _context.StudentCourseSelections.Add(selection);
        _context.SaveChanges();
    }

    public void UpdateSelection(StudentCourseSelection selection)
    {
        if (selection == null)
        {
            throw new ArgumentNullException(nameof(selection));
        }

        var existCourseSelection = _context.StudentCourseSelections
            .Include(scs => scs.Courses)
            .FirstOrDefault(scs => scs.Id == selection.Id);

        if (existCourseSelection == null)
        {
            throw new InvalidOperationException($"Selection with ID {selection.Id} does not exist.");
        }

        // Update properties
        existCourseSelection.Confirmed = selection.Confirmed;
        existCourseSelection.Cancelled = selection.Cancelled;
        existCourseSelection.UpdatedAt = DateTime.Now;

        // Update courses
        existCourseSelection.Courses = selection.Courses.Distinct().ToList();

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
            .Include(scs => scs.Courses)
            .Include(scs => scs.Semester)
            .ToList();
    }
}