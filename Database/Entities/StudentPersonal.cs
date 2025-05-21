using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Entities;

[Index(nameof(Email), IsUnique = true)]
public class StudentPersonal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    public int? ActiveSemesterId { get; set; }
    public Semester? ActiveSemester { get; set; }
}