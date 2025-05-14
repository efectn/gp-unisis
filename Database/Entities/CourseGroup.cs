namespace gp_unisis.Database.Entities;

public class CourseGroup
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Course> Courses { get; set; }
    
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}