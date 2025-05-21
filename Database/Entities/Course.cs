namespace gp_unisis.Database.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int Credit { get; set; }
    public int SemesterNumber { get; set; }
    public int Quota { get; set; }
    public bool IsElective { get; set; }
    public string Description { get; set; }
    public bool IsConfirmed { get; set; }

    public int LecturerId { get; set; }
    public Lecturer Lecturer { get; set; }
    
    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<Semester> Semesters { get; set; }
    public ICollection<CourseScheduleEntry> CourseScheduleEntries { get; set; }
    public ICollection<Grade> Grades { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
    public ICollection<CourseGroup> CourseGroups { get; set; } = new List<CourseGroup>();
    
    public ICollection<StudentCourseSelection> StudentCourseSelections { get; set; } = new List<StudentCourseSelection>();
    
    public ICollection<Exam> Exams { get; set; } = new List<Exam>();

}