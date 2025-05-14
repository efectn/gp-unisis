namespace gp_unisis.Database.Entities;

public class Grade
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public int SemesterId { get; set; }
    public Semester Semester { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public string ExamName { get; set; }
    public double Percentage { get; set; }
    public double Score { get; set; }
}