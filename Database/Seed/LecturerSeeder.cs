using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class LecturerSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var password = "827ccb0eea8a706c4c34a16891f84e7b"; // MD5 hash of "12345"
        var lecturers = new Entities.Lecturer[]
        {
            new Entities.Lecturer
            {
                Id = 1,
                FullName = "Efe Çetin",
                Password = password,
                Email = "efe@efe.com"
            },
            new Entities.Lecturer
            {
                Id = 2,
                FullName = "Ali Yılmaz",
                Password = password,
                Email = "ali@ali.com"
            },
            new Entities.Lecturer
            {
                Id = 3,
                FullName = "Mehmet Demir",
                Password = password,
                Email = "mehm543et@mehet.com"
            },
            new Entities.Lecturer
            {
                Id = 4,
                FullName = "Fatma Demir",
                Password = password,
                Email = "fatma@fatma.com"
            },
            new Entities.Lecturer
            {
                Id = 5,
                FullName = "Ayşe Demir",
                Password = password,
                Email = "ayse@ayse.com"
            },
            new Entities.Lecturer
            {
                Id = 6,
                FullName = "Ali Demir",
                Password = password,
                Email = "atreli@ali.com"
            },
            new Entities.Lecturer
            {
                Id = 7,
                FullName = "Muhittin Demir",
                Password = password,
                Email = "muhittin@muhittin.com"
            },
            new Entities.Lecturer
            {
                Id = 8,
                FullName = "Muhittin Demir2",
                Password = password,
                Email = "muhittin2s@muhittin.com"
            },
        };

        modelBuilder.Entity<Entities.Lecturer>().HasData(lecturers);
    }
}