namespace gp_unisis.Database.Entities;

public class Semester
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime FinalExamDate { get; set; }

    public ICollection<Course> Courses { get; set; }
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Transcript> Transcripts { get; set; }
}