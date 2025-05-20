namespace gp_unisis.Database.Entities;

public class CourseGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RequiredCredits { get; set; }
    public int RequiredCoursesCount { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}