using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class DepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<Department> GetAllDepartments()
    {
        return _context.Departments.Include(d => d.Faculty).ToList();
    }
    
    public Department GetDepartmentById(int id)
    {
        return _context.Departments.Include(d => d.Faculty)
            .Include(d => d.Lecturers)
            .Include(d => d.Courses)
            .FirstOrDefault(d => d.Id == id);
    }
    
    public void AddDepartment(Department department)
    {
        if (department == null)
        {
            throw new ArgumentNullException(nameof(department));
        }
        
        // Check if the department already exists
        var existingDepartment = _context.Departments.FirstOrDefault(d => d.Name == department.Name);
        if (existingDepartment != null)
        {
            throw new InvalidOperationException($"Department with name {department.Name} already exists.");
        }
        
        // Check if faculty exists
        if (department.FacultyId != null)
        {
            var faculty = _context.Faculties.FirstOrDefault(f => f.Id == department.FacultyId);
            if (faculty == null)
            {
                throw new InvalidOperationException($"Faculty with ID {department.FacultyId} does not exist.");
            }
            department.Faculty = faculty;
        }
        
        // Check if all required properties are set
        if (string.IsNullOrEmpty(department.Name) || string.IsNullOrEmpty(department.Address) || string.IsNullOrEmpty(department.ContactNumber))
        {
            throw new ArgumentException("All required properties must be set.");
        }

        try
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException != null && e.InnerException.Message.Contains("FOREIGN KEY constraint failed"))
            {
                throw new InvalidOperationException($"Faculty with ID {department.FacultyId} does not exist.");
            }
        }
    }
    
    public void UpdateDepartment(Department department)
    {
        if (department == null)
        {
            throw new ArgumentNullException(nameof(department));
        }
        
        // Check if the department exists
        var existingDepartment = _context.Departments.FirstOrDefault(d => d.Id == department.Id);
        if (existingDepartment == null)
        {
            throw new InvalidOperationException($"Department with ID {department.Id} does not exist.");
        }
        
        // Check if all required properties are set
        if (string.IsNullOrEmpty(department.Name) || string.IsNullOrEmpty(department.Address) || string.IsNullOrEmpty(department.ContactNumber))
        {
            throw new ArgumentException("All required properties must be set.");
        }
        
        // Check if the faculty exists
        var faculty = _context.Faculties.FirstOrDefault(f => f.Id == department.FacultyId);
        if (faculty == null)
        {
            throw new InvalidOperationException($"Faculty with ID {department.FacultyId} does not exist.");
        }
        
        _context.Departments.Update(department);
        _context.SaveChanges();
    }
    
    public void DeleteDepartment(int id)
    {
        var department = _context.Departments.Find(id);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with ID {id} does not exist.");
        }
        
        _context.Departments.Remove(department);
        _context.SaveChanges();
    }
    
    public Lecturer[] GetLecturersByDepartmentId(int departmentId)
    {
        var department = _context.Departments.Include(d => d.Lecturers).FirstOrDefault(d => d.Id == departmentId);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with ID {departmentId} does not exist.");
        }
        
        return department.Lecturers.ToArray();
    }
}