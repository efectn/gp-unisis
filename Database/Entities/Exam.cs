namespace gp_unisis.Database.Entities;

public class Exam
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool ExamType { get; set; } // Vize final ayrımı

    public int examCoefficient { get; set; }

    public DateTime ExamDate { get; set; }

    public int DurationMinutes { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
    
    public int SemesterId { get; set; }
    public Semester Semester { get; set; }
}