using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class AdminSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var password = "827ccb0eea8a706c4c34a16891f84e7b"; // MD5 hash of "12345"
        var lecturers = new Entities.Admin[]
        {
            new Entities.Admin
            {
                Id = 1,
                Name  = "Efe Çetin",
                Password  = password,
                Email = "efe@efe.com"
            },
        };
        
        modelBuilder.Entity<Entities.Admin>().HasData(lecturers);
    }
}