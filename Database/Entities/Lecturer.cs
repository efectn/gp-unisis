using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;


[Index(nameof(Email), IsUnique = true)]
public class Lecturer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    public ICollection<Department> Departments { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();

}