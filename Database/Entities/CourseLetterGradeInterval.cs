namespace gp_unisis.Database.Entities;

public class CourseLetterGradeInterval
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; }
    
    public int SemesterId { get; set; }
    public Semester Semester { get; set; }
    
    public bool IsBellCurve { get; set; }
    
    public double AAStart { get; set; }
    public double AAEnd { get; set; }
    public double BAStart { get; set; }
    public double BAEnd { get; set; }
    public double BBStart { get; set; }
    public double BBEnd { get; set; }
    public double CBStart { get; set; }
    public double CBEnd { get; set; }
    public double CCStart { get; set; }
    public double CCEnd { get; set; }
    public double DCStart { get; set; }
    public double DCEnd { get; set; }
    public double DDStart { get; set; }
    public double DDEnd { get; set; }
    public double FDStart { get; set; }
    public double FDEnd { get; set; }
    
    public double Average { get; set; }
    public double Stdev { get; set; }
}