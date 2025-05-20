using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class FacultySeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seed data for the Faculty table
        var faculties = new Faculty[]
        {
            new Faculty
            {
                Id = 1,
                Name = "Mühendislik Fakültesi",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-55",
                Dean = "Efe",
                ViceDean = "Celil",
            },
            new Faculty
            {
                Id = 2,
                Name = "Tıp Fakültesi",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-56",
                Dean = "Ali",
                ViceDean = "Ayşe",
            },
            new Faculty
            {
                Id = 3,
                Name = "İktisadi ve İdari Bilimler Fakültesi",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-57",
                Dean = "Mehmet",
                ViceDean = "Fatma",
            },
            new Faculty
            {
                Id = 4,
                Name = "Eğitim Fakültesi",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-58",
                Dean = "Ahmet",
                ViceDean = "Zeynep",
            },
        };
        
        modelBuilder.Entity<Faculty>().HasData(faculties);
    }
}