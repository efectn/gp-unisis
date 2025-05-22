namespace gp_unisis.Database.Entities;

public class Exam
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public int ExamCoefficient { get; set; }

    public DateTime ExamDate { get; set; }

    public int DurationMinutes { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }
    
    public int SemesterId { get; set; }
    public Semester Semester { get; set; }
    
    public bool IsExamCalculated { get; set; }
    
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}