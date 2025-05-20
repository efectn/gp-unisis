using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StudentNumber { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string NationalId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsGraduated { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<Grade> Grades { get; set; }
    public ICollection<Transcript> Transcripts { get; set; }
    
    public ICollection<StudentCourseSelection> StudentCourseSelections { get; set; }
}