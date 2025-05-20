using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class SemesterSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var semesters = new Semester[]
        {
            new Semester
            {
                Id = 1,
                Name = "2023-2024 Güz Dönemi",
                StartDate = new DateTime(2023, 10, 2),
                EndDate = new DateTime(2024, 01, 20),
                FinalExamDate = new DateTime(2024, 01, 12),
            },
            new Semester
            {
                Id = 2,
                Name = "2023-2024 Bahar Dönemi",
                StartDate = new DateTime(2024, 02, 5),
                EndDate = new DateTime(2024, 06, 15),
                FinalExamDate = new DateTime(2024, 06, 7),
            },
            new Semester
            {
                Id = 3,
                Name = "2024-2025 Güz Dönemi",
                StartDate = new DateTime(2024, 10, 1),
                EndDate = new DateTime(2025, 01, 20),
                FinalExamDate = new DateTime(2025, 01, 12),
            },
            new Semester
            {
                Id = 4,
                Name = "2024-2025 Bahar Dönemi",
                StartDate = new DateTime(2025, 02, 3),
                EndDate = new DateTime(2025, 06, 15),
                FinalExamDate = new DateTime(2025, 06, 6),
            },
        };
        
        modelBuilder.Entity<Semester>().HasData(semesters);
    }
}