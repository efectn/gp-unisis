using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class StudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Student> GetAllStudents()
    {
        return _context.Students
            .Include(s => s.Department)
            .Include(s => s.Grades)
            .Include(s => s.Transcripts)
            .Include(s => s.StudentCourseSelections)
            .ToList();
    }

    public Student GetStudentById(int id)
    {
        return _context.Students
            .Include(s => s.Department)
            .Include(s => s.Grades)
            .Include(s => s.Transcripts)
            .Include(s => s.StudentCourseSelections)
            .FirstOrDefault(s => s.Id == id);
    }

    public bool LoginStudent(string email, string password)
    {
        return _context.Students.Any(s => s.Email == email && s.Password == password);
    }

    public void AddStudent(Student student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (string.IsNullOrEmpty(student.StudentNumber) ||
            string.IsNullOrEmpty(student.Password) ||
            string.IsNullOrEmpty(student.FirstName) ||
            string.IsNullOrEmpty(student.LastName) ||
            string.IsNullOrEmpty(student.Email) ||
            string.IsNullOrEmpty(student.NationalId))
        {
            throw new ArgumentException("All required properties must be set.");
        }

        var existStudent = _context.Students.FirstOrDefault(s => s.StudentNumber == student.StudentNumber || s.Email == student.Email);
        if (existStudent != null)
        {
            throw new InvalidOperationException("A student with the same email already exists.");
        }

        _context.Students.Add(student);
        _context.SaveChanges();
    }

    public void UpdateStudent(Student student)
    {
        var existing = _context.Students.FirstOrDefault(s => s.Id == student.Id);

        if (existing == null)
        {
            throw new InvalidOperationException($"Student with ID {student.Id} does not exist.");
        }

        if (string.IsNullOrEmpty(student.StudentNumber) ||
            string.IsNullOrEmpty(student.Password) ||
            string.IsNullOrEmpty(student.FirstName) ||
            string.IsNullOrEmpty(student.LastName) ||
            string.IsNullOrEmpty(student.Email) ||
            string.IsNullOrEmpty(student.NationalId))
        {
            throw new ArgumentException("All required properties must be set.");
        }

        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.Email = student.Email;
        existing.Password = student.Password;
        existing.DepartmentId = student.DepartmentId;
        existing.DateOfBirth = student.DateOfBirth;
        existing.NationalId = student.NationalId;
        existing.IsGraduated = student.IsGraduated;

        _context.SaveChanges();
    }

    public void DeleteStudent(int id)
    {
        var existing = _context.Students.FirstOrDefault(s => s.Id == id);

        if (existing == null)
            throw new InvalidOperationException($"Student with ID {id} does not exist.");

        _context.Students.Remove(existing);
        _context.SaveChanges();
    }

    public List<Student> GetStudentsByDepartmentId(int id)
    {
        return _context.Students
            .Where(s => s.DepartmentId == id)
            .Include(s => s.Department)
            .ToList();
    }

    public List<Student> GetGraduatedStudents()
    {
        return _context.Students
            .Where(s => s.IsGraduated)
            .ToList();
    }

    public List<Student> GetActiveStudents()
    {
        return _context.Students
            .Where(s => !s.IsGraduated)
            .ToList();
    }

    public Student GetStudentByNationalId(string id)
    {
        return _context.Students.FirstOrDefault(s => s.NationalId == id);
    }

    public Student StudentGetById(int id)
    {
        return _context.Students.Include(s => s.Grades).FirstOrDefault(s => s.Id == id);
    }
}