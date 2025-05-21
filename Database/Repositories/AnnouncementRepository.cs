using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class AnnouncementRepository
{
    private readonly ApplicationDbContext _context;

    public AnnouncementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Announcement> GetAllAnnouncements()
    {
        return _context.Announcements
            .Include(a => a.Admin)
            .Include(a => a.Lecturer)
            .Include(a => a.Course).ToList();
    }

    public Announcement GetAnnouncementById(int id)
    {
        return _context.Announcements
            .Include(a => a.Admin)
            .Include(a => a.Lecturer)
            .Include(a => a.Course).FirstOrDefault(a => a.Id == id);
    }

    public void AddAnnouncement(Announcement announcement)
    {
        if (announcement == null)
        {
            throw new ArgumentNullException(nameof(announcement));
        }

        if (string.IsNullOrEmpty(announcement.Title) || string.IsNullOrEmpty(announcement.Description))
        {
            throw new ArgumentException("Title and descriptions must be set.");
        }
        
        if (announcement.ExpirationDate < DateTime.Now)
        {
            throw new ArgumentException("Expiration date must be in the future.");
        }
        
        if (announcement.AdminId == null && announcement.StudentPersonalId == null && announcement.LecturerId == null)
        {
            throw new ArgumentException("Admin ID, Student Personal ID or Lecturer ID must be set.");
        }
        
        if (announcement.LecturerId != null && announcement.CourseId == null)
        {
            throw new ArgumentException("Course ID must be set when Lecturer ID is set.");
        }

        if (announcement.AdminId != null)
        {
            var existAdmin = _context.Admins.FirstOrDefault(a => a.Id == announcement.AdminId);
            if (existAdmin == null)
            {
                throw new InvalidOperationException($"Admin with ID {announcement.AdminId} does not exist.");
            }
        }
        
        if (announcement.LecturerId != null)
        {
            var existLecturer = _context.Lecturers.FirstOrDefault(l => l.Id == announcement.LecturerId);
            if (existLecturer == null)
            {
                throw new InvalidOperationException($"Lecturer with ID {announcement.LecturerId} does not exist.");
            }
        }
        
        if (announcement.CourseId != null)
        {
            var existCourse = _context.Courses.FirstOrDefault(c => c.Id == announcement.CourseId);
            if (existCourse == null)
            {
                throw new InvalidOperationException($"Course with ID {announcement.CourseId} does not exist.");
            }
        }
        
        if (announcement.StudentPersonalId != null)
        {
            var existStudent = _context.Students.FirstOrDefault(s => s.Id == announcement.StudentPersonalId);
            if (existStudent == null)
            {
                throw new InvalidOperationException($"Student with ID {announcement.StudentPersonalId} does not exist.");
            }
        }
        
        try
        {
            _context.Announcements.Add(announcement);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Failed to add announcement.", ex);
        }
    }
    
    public void DeleteAnnouncement(int id)
    {
        var announcement = _context.Announcements.Find(id);
        if (announcement == null)
        {
            throw new InvalidOperationException($"Announcement with ID {id} does not exist.");
        }

        try
        {
            _context.Announcements.Remove(announcement);
            _context.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Failed to delete announcement.", ex);
        }
    }
}