using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class CourseGroupsRepository
{
    private readonly ApplicationDbContext _context;


    public CourseGroupsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<CourseGroup> GetCourseGroupsByDepartmentId(int departmentId)
    {
        return _context.CourseGroups
            .Include(cg => cg.Courses)
            .Where(d => d.DepartmentId == departmentId)
            .ToList();
    }

    public CourseGroup GetCourseGroupById(int id)
    {
        var courseGroup = _context.CourseGroups
        .Include(d => d.Department)
        .Include(cg => cg.Courses)
        .FirstOrDefault(cg => cg.Id == id);

        if (courseGroup == null)
        {
            throw new InvalidOperationException($"Course group with ID {id} not found.");
        }

        return courseGroup;
    }

    public List<CourseGroup> GetAllCourseGroups()
    {
        return _context.CourseGroups.ToList();
    }

    public void AddCourseGroups(CourseGroup courseGroup)
    {
        if (courseGroup == null)
        {
            throw new ArgumentNullException(nameof(courseGroup));
        }

        if (string.IsNullOrEmpty(courseGroup.Name))
        {
            throw new ArgumentException("Course name must be set.");
        }

        var existCourseGroups = _context.CourseGroups
            .FirstOrDefault(cg => cg.Name == courseGroup.Name && cg.DepartmentId == courseGroup.DepartmentId);

        if (existCourseGroups != null)
        {
            throw new ArgumentException("This course group is already exist in this department.");
        }

        var existDepartment = _context.Departments.FirstOrDefault(d => d.Id == courseGroup.DepartmentId);
        if (existDepartment == null)
        {
            throw new InvalidOperationException($"Department with ID {courseGroup.DepartmentId} does not exist.");
        }

        _context.CourseGroups.Add(courseGroup);

        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException != null && e.InnerException.Message.Contains("FOREIGN KEY"))
            {
                throw new InvalidOperationException($"Department with ID {courseGroup.DepartmentId} does not exist.");
            }

            throw;
        }
    }

    public void UpdateCourseGroups(CourseGroup courseGroup)
    {
        if (courseGroup == null)
        {
            throw new ArgumentNullException(nameof(courseGroup));
        }

        var existCourseGroups = _context.CourseGroups.FirstOrDefault(cg => cg.Id == courseGroup.Id);
        if (existCourseGroups == null)
        {
            throw new InvalidOperationException($"CourseGroups with ID {courseGroup.Id} does not exist.");
        }

        if (string.IsNullOrEmpty(courseGroup.Name))
        {
            throw new ArgumentException("Course name must be set.");
        }

        existCourseGroups.Name = courseGroup.Name;
        existCourseGroups.DepartmentId = courseGroup.DepartmentId;

        _context.SaveChanges();
    }

    public void DeleteCourseGroup(int id)
    {
        var courseGroup = _context.CourseGroups.Find(id);
        if (courseGroup == null)
        {
            throw new InvalidOperationException($"Course group with ID {id} does not exist.");
        }

        _context.CourseGroups.Remove(courseGroup);
        _context.SaveChanges();
    }

    public void AddCourseToGroup()
    {
        //
    }

    public void DeleteCourseFromGroup()
    {
        //
    }
}