namespace gp_unisis.Database.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int Credit { get; set; }
    public int SemesterNumber { get; set; }
    public int Quota { get; set; }
    public bool IsElective { get; set; }
    public string Description { get; set; }

    public int LecturerId { get; set; }
    public Lecturer Lecturer { get; set; }

    public ICollection<Semester> Semesters { get; set; }
    public ICollection<CourseScheduleEntry> CourseScheduleEntries { get; set; }
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
    public ICollection<CourseGroup> CourseGroups { get; set; }

}