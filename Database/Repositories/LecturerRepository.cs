using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class LecturerRepository
{
    private readonly ApplicationDbContext _context;

    public LecturerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Lecturer> GetAllLecturers()
    {
        return _context.Lecturers.Include(d => d.Departments).ToList();
    }

    public Lecturer GetLecturerById(int id)
    {
        var lecturer = _context.Lecturers.Include(d => d.Departments).FirstOrDefault(l => l.Id == id);
        if (lecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with ID {id} not found.");
        }
        return lecturer;
    }

    public Lecturer GetLecturerByEmail(string email)
    {
        var lecturer = _context.Lecturers.Include(l => l.Departments).FirstOrDefault(l => l.Email == email);

        if (lecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with Email {email} not found.");
        }

        return lecturer;
    }

    public void AddLecturer(Lecturer lecturer)
    {
        if (lecturer == null)
        {
            throw new ArgumentNullException(nameof(lecturer));
        }

        if (string.IsNullOrEmpty(lecturer.FullName) || string.IsNullOrEmpty(lecturer.Password) || string.IsNullOrEmpty(lecturer.Email))
        {
            throw new ArgumentException("All required properties must be set.");
        }

        var existLecturer = _context.Lecturers.FirstOrDefault(l => l.Email == lecturer.Email);
        if (existLecturer != null)
        {
            throw new InvalidOperationException($"Lecturer with Email {lecturer.Email} does already exist.");
        }

        _context.Lecturers.Add(lecturer);
        _context.SaveChanges();
    }

    public void UpdateLecturer(Lecturer lecturer)
    {
        if (lecturer == null)
        {
            throw new ArgumentNullException(nameof(lecturer));
        }

        var existLecturer = _context.Lecturers.FirstOrDefault(l => l.Id == lecturer.Id);
        if (existLecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with ID {lecturer.Id} does not exist.");
        }

        existLecturer.FullName = lecturer.FullName;
        existLecturer.Email = lecturer.Email;
        existLecturer.Password = lecturer.Password;

        _context.SaveChanges();
    }

    public void DeleteLecturer(int id)
    {
        var lecturer = _context.Lecturers.Find(id);
        if (lecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with ID {id} does not exist.");
        }

        _context.Lecturers.Remove(lecturer);
        _context.SaveChanges();
    }

    public bool LoginLecturer(string email, string password)
    {
        return _context.Lecturers.Any(l => l.Email == email && l.Password == password);
    }

    public List<Announcement> GetLecturerAnnouncementsById(int id)
    {
        return _context.Announcements.Where(l => l.LecturerId == id).ToList();
    }

    public List<Department> GetLecturerDepartmentsById(int id)
    {
        var lecturer = _context.Lecturers
            .Include(d => d.Departments)
            .FirstOrDefault(d => d.Id == id);

        if (lecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with ID {id} not found.");
        }

        return lecturer.Departments.ToList();
    }
}