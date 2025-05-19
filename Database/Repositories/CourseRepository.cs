using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class CourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Course> GetAllCourses()
    {
        return _context.Courses
            .Include(c => c.Lecturer)
            .Include(c => c.Semesters)
            .Include(c => c.CourseScheduleEntries)
            .Include(c => c.Grades)
            .Include(c => c.Announcements)
            .Include(c => c.CourseGroups)
            .ToList();
    }

    public Course GetCourseById(int id)
    {
        var course = _context.Courses
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {id} does not exist.");
        }

        return course;
    }


    public void AddCourse(Course course)
    {
        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        if (string.IsNullOrWhiteSpace(course.Name) || string.IsNullOrWhiteSpace(course.Code))
        {
            throw new ArgumentException("Course name and code must be set.");
        }

        var existingLecturer = _context.Lecturers.FirstOrDefault(l => l.Id == course.LecturerId);
        if (existingLecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with ID {course.LecturerId} does not exist.");
        }

        var exists = _context.Courses.Any(c => c.Code == course.Code);
        if (exists)
        {
            throw new InvalidOperationException("A course with the same code already exists.");
        }

        _context.Courses.Add(course);
        _context.SaveChanges();
    }

    public void UpdateCourse(Course course)
    {
        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        var existingCourse = _context.Courses.FirstOrDefault(c => c.Id == course.Id);
        if (existingCourse == null)
        {
            throw new InvalidOperationException($"Course with ID {course.Id} does not exist.");
        }

        var lecturerExists = _context.Lecturers.Any(l => l.Id == course.LecturerId);
        if (!lecturerExists)
        {
            throw new InvalidOperationException($"Lecturer with ID {course.LecturerId} does not exist.");
        }

        existingCourse.Name = course.Name;
        existingCourse.Code = course.Code;
        existingCourse.Credit = course.Credit;
        existingCourse.SemesterNumber = course.SemesterNumber;
        existingCourse.Quota = course.Quota;
        existingCourse.IsElective = course.IsElective;
        existingCourse.Description = course.Description;
        existingCourse.LecturerId = course.LecturerId;

        _context.SaveChanges();
    }

    public void DeleteCourse(int id)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {id} does not exist.");
        }

        _context.Courses.Remove(course);
        _context.SaveChanges();
    }

    public List<Course> GetCoursesByLecturerId(int id)
    {
        return _context.Courses
            .Include(c => c.Lecturer)
            .Where(c => c.LecturerId == id)
            .ToList();
    }

    public List<Course> GetCoursesBySemesterId(int id)
    {
        return _context.Courses
            .Include(c => c.Semesters)
            .Where(c => c.Semesters.Any(s => s.Id == id))
            .ToList();
    }

    public List<Course> GetElectiveCourses()
    {
        return _context.Courses.Where(c => c.IsElective).ToList();
    }

    public List<Course> GetMandatoryCourses()
    {
        return _context.Courses.Where(c => !c.IsElective).ToList();
    }

    public double GetQuotaUsage(int id)
    {
        var course = _context.Courses
            .Include(c => c.Grades)
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {id} does not exist.");
        }

        return course.Quota > 0 ? (double)(course.Grades?.Count ?? 0) / course.Quota * 100 : 0;
    }


    public List<Course> GetTopCourses(int topCount)

    {
        return _context.Courses
            .Include(c => c.Grades)
            .OrderByDescending(c => c.Grades.Count)
            .Take(topCount)
            .ToList();
    }

    public List<Course> ApprovedCourses()
    {
        return _context.Courses.Where(c => c.IsConfirmed == true).ToList();
    }

    public List<Course> NonApprovedCourses()
    {
        return _context.Courses.Where(c => c.IsConfirmed == false).ToList();
    }

    public void ApproveCourse(int id)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        if(course == null)
        {
            throw new InvalidOperationException($"Course with ID {id} does not exist.");
        }

        course.IsConfirmed = true;
        _context.Courses.Update(course);
        _context.SaveChanges();
    }

    public void RejectCourse(int id)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        if(course == null)
        {
            throw new InvalidOperationException($"Course with ID {id} does not exist.");
        }

        course.IsConfirmed = false;
        _context.Courses.Update(course);
        _context.SaveChanges();
    }
}