using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Admin
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}