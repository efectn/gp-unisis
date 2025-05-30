namespace gp_unisis.Database.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Head { get; set; }
    public string ViceHead { get; set; }

    public int? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }

    public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    //public ICollection<CourseGroupDepartment> CourseGroupDepartments { get; set; }
}