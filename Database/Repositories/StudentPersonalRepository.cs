using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class StudentPersonalRepository
{
    private readonly ApplicationDbContext _context;

    public StudentPersonalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddStudentPersonal(StudentPersonal studentPersonal)
    {
        if (studentPersonal == null)
        {
            throw new ArgumentNullException(nameof(studentPersonal));
        }

        var existStudentPersonal = _context.StudentPersonals.FirstOrDefault(a => a.Email == studentPersonal.Email);
        if (existStudentPersonal != null)
        {
            throw new InvalidOperationException($"StudentPersonal with email {studentPersonal.Email} already exist");
        }

        if (string.IsNullOrEmpty(studentPersonal.Name) || string.IsNullOrEmpty(studentPersonal.Email) || string.IsNullOrEmpty(studentPersonal.Password))
        {
            throw new ArgumentException("All required properties should be set");
        }

        _context.StudentPersonals.Add(studentPersonal);
        _context.SaveChanges();
    }

    public void UpdateStudentPersonal(StudentPersonal studentPersonal)
    {
        if (studentPersonal == null)
        {
            throw new ArgumentException(nameof(studentPersonal));
        }

        var existStudentPersonal = _context.StudentPersonals.FirstOrDefault(a => a.Id == studentPersonal.Id);
        if (existStudentPersonal == null)
        {
            throw new InvalidOperationException($"StudentPersonal with ID {studentPersonal.Id} does not exist.");
        }

        if (string.IsNullOrEmpty(studentPersonal.Name) || string.IsNullOrEmpty(studentPersonal.Email) || string.IsNullOrEmpty(studentPersonal.Password))
        {
            throw new ArgumentException("All required properties should be set");
        }

        existStudentPersonal.Name = studentPersonal.Name;
        existStudentPersonal.Password = studentPersonal.Password;
        existStudentPersonal.Email = studentPersonal.Email;

        _context.SaveChanges();
    }

    public void DeleteStudentPersonal(StudentPersonal studentPersonal)
    {
        if (studentPersonal == null)
        {
            throw new ArgumentNullException(nameof(studentPersonal));
        }

        var existing = _context.StudentPersonals.FirstOrDefault(a => a.Id == studentPersonal.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"StudentPersonal with ID {studentPersonal.Id} does not exist.");
        }

        _context.StudentPersonals.Remove(existing);
        _context.SaveChanges();
    }

    public List<StudentPersonal> GetAllStudentPersonalsName()
    {
        return _context.StudentPersonals.ToList();
    }
}
