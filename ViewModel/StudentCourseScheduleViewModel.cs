using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class StudentCourseScheduleViewModel
{
    private readonly SemesterRepository _semesterRepository;
    private readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;
    private readonly CourseRepository _courseRepository;
    private readonly StudentRepository _studentRepository;
    
    public StudentCourseScheduleViewModel(
        SemesterRepository semesterRepository,
        StudentCourseSelectionRepository studentCourseSelectionRepository,
        CourseRepository courseRepository,
        StudentRepository studentRepository)
    {
        _semesterRepository = semesterRepository;
        _studentCourseSelectionRepository = studentCourseSelectionRepository;
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }
    
    public void ListStudentCourseSchedule()
    {
       Console.WriteLine("Semester ID'si girin: ");
         var semesterId = int.Parse(Console.ReadLine() ?? string.Empty);
            var semester = _semesterRepository.GetSemesterById(semesterId);
            if (semester == null)
            {
                Console.WriteLine("Böyle bir dönem bulunamadı.");
                return;
            }
            
            Console.WriteLine("Öğrenci ID'si girin: ");
            var studentId = int.Parse(Console.ReadLine() ?? string.Empty);
            var student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                Console.WriteLine("Böyle bir öğrenci bulunamadı.");
                return;
            }
            
            var studentCourseSelection = _studentCourseSelectionRepository.GetSelectionsByStudentId(studentId)
                .FirstOrDefault(s => s.SemesterId == semesterId);
            if (studentCourseSelection == null)
            {
                Console.WriteLine("Öğrenci bu dönem için ders seçimi yapmamış.");
                return;
            }
            
            Console.WriteLine($"Dönem: {semester.Name}");
            Console.WriteLine($"Öğrenci: {student.FirstName} {student.LastName}");
            
            var days = new[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };

            var courseScheduleEntries = studentCourseSelection.Courses.SelectMany(c => c.CourseScheduleEntries)
                .ToList();
            if (courseScheduleEntries.Count == 0)
            {
                Console.WriteLine("Öğrenci bu dönem için ders seçimi yapmamış.");
                return;
            }
            
            // Group courses by day
            var groupedEntries = courseScheduleEntries
                .GroupBy(e => e.Day)
                .OrderBy(g => Array.IndexOf(days, g.Key))
                .ToList();

            foreach (var group in groupedEntries)
            {
                Console.WriteLine($"{group.Key}:");
                foreach (var entry in group)
                {
                    var course = _courseRepository.GetCourseById(entry.CourseId);
                    Console.WriteLine($"  {course.Name} - {entry.StartTime:hh\\:mm} - {entry.EndTime:hh\\:mm}");
                }
            }
    }
}