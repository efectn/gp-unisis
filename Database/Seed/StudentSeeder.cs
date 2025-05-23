using Bogus;
using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class StudentSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var departmentIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var semesters = new[] { 1, 2, 3, 4 };
        var password = "827ccb0eea8a706c4c34a16891f84e7b"; // MD5 hash of "12345"

        var studentFaker = new Faker<Student>()
            .RuleFor(s => s.FirstName, f => f.Name.FirstName())
            .RuleFor(s => s.LastName, f => f.Name.LastName())
            .RuleFor(s => s.StudentNumber, f => f.Random.AlphaNumeric(9).ToUpper())
            .RuleFor(s => s.Password, f => password)
            .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName).ToLower())
            .RuleFor(s => s.NationalId, f => f.Random.Replace("#############"))
            .RuleFor(s => s.DateOfBirth, f => f.Date.Between(new DateTime(1980, 1, 1), new DateTime(2005, 12, 31)))
            .RuleFor(s => s.IsGraduated, false)
            .RuleFor(s => s.EntranceSemesterId, f => 1 )
            .RuleFor(s => s.DepartmentId, f => f.PickRandom(departmentIds));

        var students = studentFaker.Generate(200);

        for (int i = 0; i < students.Count; i++)
        {
            students[i].Id = i + 1;
        }
        
        modelBuilder.Entity<Student>().HasData(students);
    }
}