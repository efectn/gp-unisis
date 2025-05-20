namespace gp_unisis.Database.Entities;

public class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int ExamId { get; set; }
    public Exam Exam { get; set; }

    public double Score { get; set; }
}