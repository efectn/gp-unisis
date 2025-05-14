using gp_unisis.Database;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace Bruh;

public class Bruh
{
    public static void Main()
    {
        var db = new ApplicationDbContext();
        db.Database.EnsureCreated();

        var facultyRepository = new FacultyRepository(db);

        var faculty = new Faculty
        {
            Name = "Faculty of Science",
            Address = "123 Science St.",
            ContactNumber = "123-456-7890",
            Dean = "Dr. Smith",
            ViceDean = "Dr. Johnson"
        };
        //facultyRepository.AddFaculty(faculty);
        var faculties = facultyRepository.GetAllFaculties();
        foreach (var fac in faculties)
        {
            Console.WriteLine($"Faculty: {fac.Name}, Address: {fac.Address}, Contact: {fac.ContactNumber}");
            Console.WriteLine("Faculty Departments:");
            foreach (var department in fac.Departments)
            {
                Console.WriteLine($"- {department.Name}");
            }
        }
        
        /*
        var departmentRepository = new DepartmentRepository(db);
        var newDepartment = new Department
        {
            Name = "Department of Physics",
            Address = "456 Physics Ave.",
            ContactNumber = "987-654-3210",
            Head = "Dr. Brown",
            ViceHead = "Dr. Green",
            FacultyId = 5
        };
        
        departmentRepository.AddDepartment(newDepartment);
        var departments = departmentRepository.GetAllDepartments();
        foreach (var dept in departments)
        {
            Console.WriteLine($"Department: {dept.Name}, Address: {dept.Address}, Contact: {dept.ContactNumber}");
            Console.WriteLine("Department Faculty:");
            Console.WriteLine($"- {dept.Faculty.Name}");
        }
        */
    }
}