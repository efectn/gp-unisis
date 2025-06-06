namespace gp_unisis.Database.Entities;

public class Semester
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime FinalExamDate { get; set; }
    //public DateTime CourseRegistrationStartDate { get; set; }
   //public DateTime CourseRegistrationEndDate { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Transcript> Transcripts { get; set; } = new List<Transcript>();
}