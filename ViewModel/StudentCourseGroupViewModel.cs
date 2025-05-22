using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class StudentCourseGroupViewModel
{
    private readonly CourseGroupsRepository _courseGroupsRepository;
    private readonly StudentRepository _studentRepository;
    private readonly TranscriptRepository _transcriptRepository;
    
    public StudentCourseGroupViewModel(CourseGroupsRepository courseGroupsRepository, StudentRepository studentRepository, TranscriptRepository transcriptRepository)
    {
        _courseGroupsRepository = courseGroupsRepository;
        _studentRepository = studentRepository;
        _transcriptRepository = transcriptRepository;
    }
    
    public void ShowStudentCourseGroups()
    {
        Console.Write("Öğrenci ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }
        
        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        var courseGroups = _courseGroupsRepository.GetCourseGroupsByDepartmentId(student.DepartmentId);
        if (courseGroups == null || courseGroups.Count == 0)
        {
            Console.WriteLine("Kurs grupları bulunamadı.");
            return;
        }
        
        var transcriptCourses = _transcriptRepository.GetAllTranscripts()
            .Where(tc => tc.StudentId == studentId)
            .Where(tc => !tc.HasFailed)
            .Select(tc => tc.CourseCode)
            .ToList();

        Console.WriteLine($"Öğrenci: {student.FirstName} {student.LastName}");
        Console.WriteLine("Kurs Grupları:");
        
        foreach (var courseGroup in courseGroups)
        {
            var requiredCoursesCount = courseGroup.RequiredCoursesCount;
            var requiredCredits = courseGroup.RequiredCredits;
            
            Console.WriteLine($"- {courseGroup.Name}");
            Console.WriteLine($"  Minimum Alınması Gereken Ders Sayısı: {requiredCoursesCount}");
            Console.WriteLine($"  Minimum Alınması Gereken Kredi Sayısı: {requiredCredits}");
            
            foreach (var course in courseGroup.Courses)
            {
                if (transcriptCourses.Contains(course.Code))
                {
                    Console.WriteLine($"  - {course.Name} (Kodu: {course.Code}) - Alındı");
                    requiredCoursesCount--;
                    requiredCredits -= course.Credit;
                }
                else
                {
                    Console.WriteLine($"  - {course.Name} (Kodu: {course.Code}) - Alınmadı");
                }
            }
            
            if (requiredCoursesCount <= 0 && requiredCredits <= 0)
            {
                Console.WriteLine($"  Bu kurs grubunu tamamladınız.");
            }
            else
            {
                Console.WriteLine($"  Bu kurs grubunu tamamlamak için {requiredCoursesCount} ders ve {requiredCredits} kredi daha almanız gerekiyor.");
            }
        }
    }
    
    public void CanStudentGraduate()
    {
        Console.Write("Öğrenci ID'si: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz ID.");
            return;
        }
        
        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        var courseGroups = _courseGroupsRepository.GetCourseGroupsByDepartmentId(student.DepartmentId);
        if (courseGroups == null || courseGroups.Count == 0)
        {
            Console.WriteLine("Kurs grupları bulunamadı.");
            return;
        }
        
        var transcriptCourses = _transcriptRepository.GetAllTranscripts()
            .Where(tc => tc.StudentId == studentId)
            .Where(tc => !tc.HasFailed)
            .Select(tc => tc.CourseCode)
            .ToList();
        
        foreach (var courseGroup in courseGroups)
        {
            var requiredCoursesCount = courseGroup.RequiredCoursesCount;
            var requiredCredits = courseGroup.RequiredCredits;
            
            foreach (var course in courseGroup.Courses)
            {
                if (transcriptCourses.Contains(course.Code))
                {
                    requiredCoursesCount--;
                    requiredCredits -= course.Credit;
                }
            }
            
            if (requiredCoursesCount > 0 || requiredCredits > 0)
            {
                Console.WriteLine($"Bu kurs grubunu tamamlamak için {requiredCoursesCount} ders ve {requiredCredits} kredi daha almanız gerekiyor. Mezun olamazsınız");
                return;
                
            }
            
        }

        Console.WriteLine("Tüm kurs gruplarını tamamladınız. Mezun olabilirsiniz.");
        
        // For admins
        Console.WriteLine("Öğrenci mezun edilsin mi? (E/H): ");
        if (Console.ReadLine()?.ToUpper() == "E")
        {
            // Call the method to graduate the student
            student.IsGraduated = true;
            try 
            {
                _studentRepository.UpdateStudent(student);
                Console.WriteLine("Öğrenci mezun edildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mezuniyet işlemi sırasında hata oluştu: {ex.Message}");
            }
            
        }
    }
}