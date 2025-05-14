namespace gp_unisis.Database.Entities;

public class StudentCourseSelection
{
    public int Id { get; set; }
    
    public int SemesterId { get; set; }
    public Semester Semester { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public bool Confirmed { get; set; }
    public bool Cancelled { get; set; }
}