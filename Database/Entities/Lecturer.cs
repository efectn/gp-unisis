namespace gp_unisis.Database.Entities;

public class Lecturer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public ICollection<Department> Departments { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
}