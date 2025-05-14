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

        var existAnnouncement = _context.Announcements
            .FirstOrDefault(a => a.Title == announcement.Title && a.Description == announcement.Description);

        if (existAnnouncement != null)
        {
            throw new InvalidOperationException("Announcement already exist.");
        }

        var existAdmin = _context.Admins.FirstOrDefault(a => a.Id == announcement.AdminId);
        if (existAdmin == null)
        {
            throw new InvalidOperationException($"Admin with ID {announcement.AdminId} does not exist.");
        }

        var existLecturer = _context.Lecturers.FirstOrDefault(a => a.Id == announcement.LecturerId);
        if (existLecturer == null)
        {
            throw new InvalidOperationException($"Lecturer with ID {announcement.LecturerId} does not exist.");
        }

        var existCourse = _context.Courses.FirstOrDefault(a => a.Id == announcement.CourseId);
        if (existCourse == null)
        {
            throw new InvalidOperationException($"Course with ID {announcement.CourseId} does not exist.");
        }

        _context.Announcements.Add(announcement);
        _context.SaveChanges();
    }

    public void UpdateAnnouncement(Announcement announcement)
    {
        if (announcement == null)
        {
            throw new ArgumentNullException(nameof(announcement));
        }

        var existAnnouncement = _context.Announcements.FirstOrDefault(a => a.Id == announcement.Id);
        if (existAnnouncement == null)
        {
            throw new InvalidOperationException($"Announcement with ID {announcement.Id} does not exist.");
        }

        existAnnouncement.Title = announcement.Title;
        existAnnouncement.Description = announcement.Description;
        existAnnouncement.ExpirationDate = announcement.ExpirationDate;
        existAnnouncement.AdminId = announcement.AdminId;
        existAnnouncement.LecturerId = announcement.LecturerId;
        existAnnouncement.CourseId = announcement.CourseId;

        _context.SaveChanges();
    }

    public void DeleteAnnouncement(Announcement announcement)
    {
        if (announcement == null)
        {
            throw new ArgumentNullException(nameof(announcement));
        }

        var existing = _context.Announcements.FirstOrDefault(a => a.Id == announcement.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Announcement with ID {announcement.Id} does not exist.");
        }

        _context.Announcements.Remove(existing);
        _context.SaveChanges();
    }
}