namespace gp_unisis.Database.Entities;

public class CourseScheduleEntry
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public string Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    public int SemesterId { get; set; }
    public Semester Semester { get; set; }
}