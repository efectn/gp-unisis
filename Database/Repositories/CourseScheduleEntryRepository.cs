using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class CourseScheduleEntryRepository
{
    private readonly ApplicationDbContext _context;


    public CourseScheduleEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<CourseScheduleEntry> GetAllScheduleEntries()
    {
        return _context.CourseScheduleEntries.Include(cse => cse.Course).ToList();
    }

    public CourseScheduleEntry GetScheduleEntryById(int id)
    {
        var SchuleEntry = _context.CourseScheduleEntries.Include(c => c.Course).FirstOrDefault(cse => cse.Id == id);
        if (SchuleEntry == null)
        {
            throw new InvalidOperationException($"Schule with ID {id} does not exist.");
        }

        return SchuleEntry;
    }

    public void AddScheduleEntry(CourseScheduleEntry courseScheduleEntry)
    {
        if (courseScheduleEntry == null)
        {
            throw new ArgumentNullException(nameof(courseScheduleEntry));
        }

        if (courseScheduleEntry.CourseId == 0 && string.IsNullOrEmpty(courseScheduleEntry.Day))
        {
            throw new ArgumentException("All properties must be set.");
        }

        var course = _context.Courses.Find(courseScheduleEntry.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {courseScheduleEntry.CourseId} does not exist.");
        }

        _context.CourseScheduleEntries.Add(courseScheduleEntry);
        _context.SaveChanges();
    }

    public void UpdateScheduleEntry(CourseScheduleEntry courseScheduleEntry)
    {
        if (courseScheduleEntry == null)
        {
            throw new ArgumentNullException(nameof(courseScheduleEntry));
        }

        var existCSEntry = _context.CourseScheduleEntries.FirstOrDefault(cse => cse.Id == courseScheduleEntry.Id);
        if (existCSEntry == null)
        {
            throw new InvalidOperationException($"Schedule entry with ID {courseScheduleEntry.Id} does not exist.");
        }

        existCSEntry.Course = courseScheduleEntry.Course;
        existCSEntry.Day = courseScheduleEntry.Day;
        existCSEntry.StartTime = courseScheduleEntry.StartTime;
        existCSEntry.EndTime = courseScheduleEntry.EndTime;

        _context.SaveChanges();
    }

    public void DeleteScheduleEntry(int id)
    {
        var SchuleEntry = _context.CourseScheduleEntries.Find(id);
        if (SchuleEntry == null)
        {
            throw new InvalidOperationException($"Schedule entry with ID {id} does not exist.");
        }

        _context.CourseScheduleEntries.Remove(SchuleEntry);
        _context.SaveChanges();
    }

    public List<CourseScheduleEntry> GetScheduleEntriesByCourseId(int id)
    {
        return _context.CourseScheduleEntries.Include(c => c.Course).Where(c => c.CourseId == id).ToList();
    }

    public List<CourseScheduleEntry> GetScheduleEntriesByDay(string day)
    {
        return _context.CourseScheduleEntries.Include(c => c.Course).Where(c => c.Day.ToLower() == day.ToLower()).ToList();
    }
}