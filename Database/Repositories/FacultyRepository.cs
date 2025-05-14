using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class FacultyRepository
{
    private readonly ApplicationDbContext _context;

    public FacultyRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public  List<Faculty> GetAllFaculties()
    {
        return _context.Faculties.Include(f => f.Departments).ToList();
    }
    
    public Faculty GetFacultyById(int id)
    {
        return _context.Faculties.Include(f => f.Departments).FirstOrDefault(f => f.Id == id);
    }
    
    public void AddFaculty(Faculty faculty)
    {
        if (faculty == null)
        {
            throw new ArgumentNullException(nameof(faculty));
        }
        
        // Check if the faculty already exists
        var existingFaculty = _context.Faculties.FirstOrDefault(f => f.Name == faculty.Name);
        if (existingFaculty != null)
        {
            throw new InvalidOperationException($"Faculty with name {faculty.Name} already exists.");
        }
        
        // Check if all required properties are set
        if (string.IsNullOrEmpty(faculty.Name) || string.IsNullOrEmpty(faculty.Address) || string.IsNullOrEmpty(faculty.ContactNumber))
        {
            throw new ArgumentException("All required properties must be set.");
        }
        
        _context.Faculties.Add(faculty);
        _context.SaveChanges();
    }
    
    public void UpdateFaculty(Faculty faculty)
    {
        if (faculty == null)
        {
            throw new ArgumentNullException(nameof(faculty));
        }
        
        // Check if the faculty exists
        var existingFaculty = _context.Faculties.FirstOrDefault(f => f.Id == faculty.Id);
        if (existingFaculty == null)
        {
            throw new InvalidOperationException($"Faculty with ID {faculty.Id} does not exist.");
        }
        
        // Update the properties
        existingFaculty.Name = faculty.Name;
        existingFaculty.Address = faculty.Address;
        existingFaculty.ContactNumber = faculty.ContactNumber;
        existingFaculty.Dean = faculty.Dean;
        existingFaculty.ViceDean = faculty.ViceDean;

        _context.SaveChanges();
    }
    
    public void DeleteFaculty(int id)
    {
        var faculty = _context.Faculties.Find(id);
        if (faculty == null)
        {
            throw new InvalidOperationException($"Faculty with ID {id} does not exist.");
        }
        
        _context.Faculties.Remove(faculty);
        _context.SaveChanges();
    }
    
    public List<Department> GetFacultyDepartments(int facultyId)
    {
        var faculty = _context.Faculties.Include(f => f.Departments).FirstOrDefault(f => f.Id == facultyId);
        if (faculty == null)
        {
            throw new InvalidOperationException($"Faculty with ID {facultyId} does not exist.");
        }
        
        return faculty.Departments.ToList();
    }
}