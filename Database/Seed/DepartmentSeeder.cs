using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class DepartmentSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var departments = new Department[]
        {
            new Department
            {
                Id = 1,
                Name = "Bilgisayar Mühendisliği",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-55",
                Head = "Efe",
                ViceHead = "Celil",
                FacultyId = 1,
            },
            new Department
            {
                Id = 2,
                Name = "Elektrik Mühendisliği",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-56",
                Head = "Ali",
                ViceHead = "Ayşe",
                FacultyId = 1,
            },
            new Department
            {
                Id = 3,
                Name = "Tıp",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-57",
                Head = "Mehmet",
                ViceHead = "Fatma",
                FacultyId = 2,
            },
            new Department
            {
                Id = 4,
                Name = "İşletme",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-58",
                Head = "Ahmet",
                ViceHead = "Zeynep",
                FacultyId = 3,
            },
            new Department
            {
                Id = 5,
                Name = "İktisat",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-59",
                Head = "Ayşe",
                ViceHead = "Fatma",
                FacultyId = 3,
            },
            new Department
            {
                Id = 6,
                Name = "Matematik Öğretmenliği",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-59",
                Head = "Ayşe",
                ViceHead = "Fatma",
                FacultyId = 4,
            },
            new Department
            {
                Id = 7,
                Name = "Fen Bilgisi Öğretmenliği",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-59",
                Head = "Ayşe",
                ViceHead = "Fatma",
                FacultyId = 4,
            },
            new Department
            {
                Id = 8,
                Name = "Kimya Öğretmenliği",
                Address = "Uludağ Üniversitesi",
                ContactNumber = "(0555) 555-55-59",
                Head = "Ayşe",
                ViceHead = "Fatma",
                FacultyId = 4,
            },
        };
        
        modelBuilder.Entity<Department>().HasData(departments);
    }
    
}