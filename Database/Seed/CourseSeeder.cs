using Bogus;
using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Seed;

public class CourseSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var departmentCodes = new[] { 
            "CS", "MATH", "PHYS", "ENG", "HIST", "CHEM", "BIO", "ECON", "PSY", "ART",
            "MATH-EDU", "SCI-EDU", "ELEC", "EE", "EDU", "CHEM-EDU", "PHYS-EDU"
        };
        
        var lecturerIds = new[] { 1,2,3,4,5,6,7 };
        var departmentIds = new[] { 1,2,3,4,5,6,7,8 };

        var courseNames = new[]
        {
            // Bilgisayar Bilimleri
            "Introduction to Programming",
            "Data Structures and Algorithms",
            "Database Systems",
            "Computer Networks",
            "Operating Systems",
            "Software Engineering",
            "Web Development",
            "Mobile Application Development",
            "Machine Learning",
            "Artificial Intelligence",
            "Computer Graphics",
            "Human-Computer Interaction",
            
            // Matematik ve Matematik Öğretmenliği
            "Calculus I",
            "Calculus II",
            "Linear Algebra",
            "Discrete Mathematics",
            "Abstract Algebra",
            "Mathematics Education",
            "Teaching Mathematics in Secondary Schools",
            "Geometry for Teachers",
            "Probability and Statistics for Educators",
            
            // Fen Bilimleri ve Fen Öğretmenliği
            "General Physics I",
            "General Physics II",
            "Modern Physics",
            "Science Teaching Methods",
            "Laboratory Techniques in Science Education",
            "Environmental Science for Educators",
            "Astronomy for Teachers",
            "Biology for Teachers",
            "Chemistry for Teachers",
            "Physics for Teachers",
            
            // Kimya ve Kimya Öğretmenliği
            "General Chemistry I",
            "General Chemistry II",
            "Organic Chemistry",
            "Physical Chemistry",
            "Biochemistry",
            "Chemistry Education",
            "Teaching Chemistry in Secondary Schools",
            "Laboratory Safety in Chemistry Education",
            "Analytical Chemistry for Educators",
            "Inorganic Chemistry for Teachers",
            
            
            // Elektrik-Elektronik Mühendisliği
            "Circuit Theory",
            "Digital Electronics",
            "Analog Electronics",
            "Electromagnetic Theory",
            "Power Systems",
            "Control Systems",
            "Signal Processing",
            "Microprocessors",
            "Telecommunication Systems",
            "Embedded Systems",
            
            // Ekonomi
            "Microeconomics",
            "Macroeconomics",
            "Econometrics",
            "International Economics",
            "Development Economics",
            
            // Diğer
            "Academic Writing",
            "Research Methods",
            "Technical Drawing",
            "Engineering Ethics",
            "History of Science"
        };

        var courseFaker = new Faker<Course>()
            .RuleFor(c => c.Name, f => f.PickRandom(courseNames))
            .RuleFor(c => c.Code, f => {
                var deptCode = f.PickRandom(departmentCodes);
                var courseNum = f.Random.Number(100, 599);
                return $"{deptCode}-{courseNum}";
            })
            .RuleFor(c => c.Credit, f => f.Random.Int(1, 6))
            .RuleFor(c => c.SemesterNumber, f => f.Random.Int(1, 8))
            .RuleFor(c => c.Quota, f => f.Random.Int(20, 100))
            .RuleFor(c => c.IsElective, f => f.Random.Bool(0.3f))
            .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
            .RuleFor(c => c.IsConfirmed, f => f.Random.Bool(0.9f)) // %90 onaylı
            .RuleFor(c => c.LecturerId, f => f.PickRandom(lecturerIds))
            .RuleFor(c => c.DepartmentId, f => f.PickRandom(departmentIds));

        var courses = courseFaker.Generate(50);

        // ID ataması
        for (int i = 0; i < courses.Count; i++)
        {
            courses[i].Id = i+1;
        }
        
        // Add the courses to the modelBuilder
        modelBuilder.Entity<Course>().HasData(courses);
    }
}