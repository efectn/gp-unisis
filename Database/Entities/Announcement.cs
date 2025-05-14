namespace gp_unisis.Database.Entities;

public class Announcement
{
    public int Id { get; set; }

    public int? AdminId { get; set; }
    public Admin Admin { get; set; }

    public int? LecturerId { get; set; }
    public Lecturer Lecturer { get; set; }

    public int? CourseId { get; set; }
    public Course Course { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ExpirationDate { get; set; }
}