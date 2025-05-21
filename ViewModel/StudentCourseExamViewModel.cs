using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class StudentCourseExamScheduleViewModel
{
    private readonly SemesterRepository _semesterRepository;
    private readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;
    private readonly CourseRepository _courseRepository;
    private readonly StudentRepository _studentRepository;
    
    public StudentCourseExamScheduleViewModel(
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
            
            var courses = studentCourseSelection.Courses;
            if (courses.Count == 0)
            {
                Console.WriteLine("Bu dönem için ders seçimi yapılmamış.");
                return;
            }

            foreach (var course in courses)
            {
                Console.WriteLine($"Ders: {course.Name} Sınavları");
                var exams = course.Exams
                    .Where(e => e.SemesterId == semesterId)
                    .ToList();

                if (exams.Count == 0)
                {
                    Console.WriteLine("Bu dönem için sınav yok.");
                }
                else
                {
                    foreach (var exam in exams)
                    {
                        Console.WriteLine($"  Sınav Tarihi: {exam.ExamDate:dd/MM/yyyy} - Süre: {exam.DurationMinutes} dakika");
                    }
                }
                
            }
    }
}