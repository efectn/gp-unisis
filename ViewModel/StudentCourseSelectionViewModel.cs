using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class StudentCourseSelectionViewModel
{
    public readonly SemesterRepository _semesterRepository;
    public readonly CourseRepository _courseRepository;
    public readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;
    public readonly StudentRepository _studentRepository;

    public StudentCourseSelectionViewModel(SemesterRepository semesterRepository, 
        CourseRepository courseRepository,
        StudentRepository studentRepository, 
        StudentCourseSelectionRepository studentCourseSelectionRepository
        )
    {
        _semesterRepository = semesterRepository;
        _courseRepository = courseRepository;
        _studentCourseSelectionRepository = studentCourseSelectionRepository;
        _studentRepository = studentRepository;
    }

    public void AddCourses()
    {
        Console.WriteLine("Dönem Seçimi:");
        var semesters = _semesterRepository.GetAllSemesters();
        foreach (var semester in semesters)
        {
            Console.WriteLine($"- ID: {semester.Id}, Dönem: {semester.Name}");
        }

        Console.Write("Dönem ID: ");
        if (!int.TryParse(Console.ReadLine(), out int semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        Console.WriteLine("Öğrenci ID'si girin:");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }
        
        var selectedSemester = _semesterRepository.GetSemesterById(semesterId);
        if (selectedSemester == null)
        {
            Console.WriteLine("Dönem bulunamadı.");
            return;
        }
        
        // Check semester course registration dates
        if (DateTime.Now < selectedSemester.CourseRegistrationStartDate || DateTime.Now > selectedSemester.CourseRegistrationEndDate)
        {
            Console.WriteLine("Ders kayıt dönemi kapalı. Seçim yapamazsınız.");
            return;
        }

        var transcript = student.Transcripts;

        // Check if the student is already registered for the semester
        var existingSelection = _studentCourseSelectionRepository.GetAllSelections()
            .FirstOrDefault(scs => scs.StudentId == studentId && scs.SemesterId == semesterId);
        if (existingSelection != null)
        {
            Console.WriteLine(
                "Bu dönem için zaten ders seçimi yapılmış. Onaylanmadıysa güncelleme kısmından değiştirebilirsiniz.");
            return;
        }

        var department = student.Department;


        Console.WriteLine("Alabileceği dersler listedeki gibidir. Maksimum 40 kredilik ders alabilirsin.");
        var courses = department.Courses
            .Where(c => c.Semesters.Any(s => s.Id == semesterId))
            .ToList();
        foreach (var course in courses)
        {
            // Skip if student already passed the course successfully
            if (transcript != null && transcript.Any(tc => tc.CourseId == course.Id && !tc.HasFailed))
            {
                Console.WriteLine(
                    $"{course.Name} dersi zaten başarıyla verilmiş. Bu dersi tekrar seçemezsiniz.");
                continue;
            }
            
            Console.WriteLine(
                $"- ID: {course.Id}, Ad: {course.Name}, Kredi: {course.Credit}, Kontenjan: {course.Quota}");
        }

        Console.WriteLine("Seçmek istediğiniz derslerin ID'lerini girin (virgülle ayırarak):");
        var selectedCourseIds = Console.ReadLine()?.Split(',').Select(id => id.Trim()).ToList();
        if (selectedCourseIds == null || selectedCourseIds.Count == 0)
        {
            Console.WriteLine("Hiç ders seçmediniz.");
            return;
        }

        // Validate selected courses and check if they exceed the credit limit
        var selectedCourses = new List<Course>();
        int totalCredits = 0;
        foreach (var courseIdStr in selectedCourseIds)
        {
            if (int.TryParse(courseIdStr, out int courseId))
            {
                var course = _courseRepository.GetCourseById(courseId);
                if (course != null)
                {
                    // Check quota from student course selection table
                    var selectedCount = _studentCourseSelectionRepository.GetAllSelections()
                        .Where(scs => scs.SemesterId == semesterId && scs.StudentId != studentId)
                        .SelectMany(scs => scs.Courses)
                        .Count(c => c.Id == courseId);

                    if (selectedCount >= course.Quota)
                    {
                        Console.WriteLine($"Ders ID'si {courseId} için kontenjan kalmadı.");
                        continue;
                    }

                    selectedCourses.Add(course);
                    totalCredits += course.Credit;
                }
                else
                {
                    Console.WriteLine($"Ders ID'si {courseId} bulunamadı.");
                }
            }
            else
            {
                Console.WriteLine($"Geçersiz ders ID'si: {courseIdStr}");
            }
        }

        if (totalCredits > 40)
        {
            Console.WriteLine("Seçtiğiniz derslerin toplam kredisi 40'ı aşıyor.");
            return;
        }

        // Check quota for each selected course, look at in studentselection table
        foreach (var course in selectedCourses)
        {
            if (course.Quota <= 0)
            {
                Console.WriteLine($"Ders ID'si {course.Id} için kontenjan kalmadı.");
                return;
            }
        }


        // Create a new StudentCourseSelection object
        var studentCourseSelection = new StudentCourseSelection
        {
            SemesterId = semesterId,
            StudentId = studentId,
            Confirmed = false,
            Cancelled = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Courses = selectedCourses
        };
        
        try
        {
            Console.WriteLine("Ders seçimi kaydediliyor...");
            _studentCourseSelectionRepository.AddSelection(studentCourseSelection);
            Console.WriteLine("Ders seçimi işlemi tamamlandı.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            throw;
        }
    }

    public void UpdateCourses()
    {
        Console.WriteLine("Ders güncelleme:");
        Console.Write("Öğrenci ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        Console.WriteLine("Dönem ID'sini girin:");
        if (!int.TryParse(Console.ReadLine(), out int semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        var semester = _semesterRepository.GetSemesterById(semesterId);
        if (semester == null)
        {
            Console.WriteLine("Dönem bulunamadı.");
            return;
        }
        
        // Check semester course registration dates
        if (DateTime.Now < semester.CourseRegistrationStartDate || DateTime.Now > semester.CourseRegistrationEndDate)
        {
            Console.WriteLine("Ders kayıt dönemi kapalı. Güncelleme yapamazsınız.");
            return;
        }

        var selection = _studentCourseSelectionRepository.GetAllSelections()
            .FirstOrDefault(scs => scs.StudentId == studentId && scs.SemesterId == semesterId);
        if (selection == null)
        {
            Console.WriteLine("Seçim bulunamadı.");
            return;
        }

        // Cancel the update if the selection is already confirmed
        if (selection.Confirmed)
        {
            Console.WriteLine("Bu seçim zaten onaylanmış. Güncelleyemezsiniz.");
            return;
        }

        Console.WriteLine("Seçili dersler:");
        foreach (var course in selection.Courses)
        {
            Console.WriteLine($"- ID: {course.Id}, Ad: {course.Name}, Kredi: {course.Credit}");
        }

        Console.WriteLine("Bölümde açılan aktif dersler:");
        var department = student.Department;
        // Get department courses of semester
        var departmentCourses = _courseRepository.GetCoursesByDepartment(department.Id)
            .Where(c => c.Semesters.Any(s => s.Id == semesterId))
            .ToList();

        foreach (var course in departmentCourses)
        {
            Console.WriteLine(
                $"- ID: {course.Id}, Ad: {course.Name}, Kredi: {course.Credit}, Kontenjan: {course.Quota}");
        }

        Console.WriteLine("Yeni derslerin ID'lerini girin (virgülle ayırarak):");
        var selectedCourseIds = Console.ReadLine()?.Split(',').Select(id => id.Trim()).ToList();

        if (selectedCourseIds == null || selectedCourseIds.Count == 0)
        {
            Console.WriteLine("Hiç ders seçmediniz.");
            return;
        }

        // Validate selected courses and check if they exceed the credit limit
        var selectedCourses = new List<Course>();
        int totalCredits = 0;
        foreach (var courseIdStr in selectedCourseIds)
        {
            if (int.TryParse(courseIdStr, out int courseId))
            {
                var course = _courseRepository.GetCourseById(courseId);
                if (course != null)
                {
                    // Check quota from student course selection table
                    var selectedCount = _studentCourseSelectionRepository.GetAllSelections()
                        .Where(scs => scs.SemesterId == semesterId && scs.StudentId != studentId)
                        .SelectMany(scs => scs.Courses)
                        .Count(c => c.Id == courseId);

                    if (selectedCount >= course.Quota)
                    {
                        Console.WriteLine($"Ders ID'si {courseId} için kontenjan kalmadı.");
                        continue;
                    }

                    selectedCourses.Add(course);
                    totalCredits += course.Credit;
                }
                else
                {
                    Console.WriteLine($"Ders ID'si {courseId} bulunamadı.");
                }
            }
            else
            {
                Console.WriteLine($"Geçersiz ders ID'si: {courseIdStr}");
            }
        }

        if (totalCredits > 40)
        {
            Console.WriteLine("Seçtiğiniz derslerin toplam kredisi 40'ı aşıyor.");
            return;
        }

        try
        {
            Console.WriteLine("Seçilen dersler güncelleniyor...");
            // Update the selection
            selection.Courses = selectedCourses;
            selection.UpdatedAt = DateTime.Now;

            _studentCourseSelectionRepository.UpdateSelection(selection);
            Console.WriteLine("Ders güncelleme işlemi tamamlandı.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            throw;
        }
    }
    
    public void ConfirmOrCancelCourseSelection()
    {
        Console.WriteLine("Ders seçimini onayla veya iptal et:");
        Console.Write("Öğrenci ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        Console.WriteLine("Dönem ID'sini girin:");
        if (!int.TryParse(Console.ReadLine(), out int semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        var selection = _studentCourseSelectionRepository.GetAllSelections()
            .FirstOrDefault(scs => scs.StudentId == studentId && scs.SemesterId == semesterId);
        if (selection == null)
        {
            Console.WriteLine("Seçim bulunamadı.");
            return;
        }

        // Cancel the update if the selection is already confirmed
        if (selection.Confirmed)
        {
            Console.WriteLine("Bu seçim zaten onaylanmış. Güncelleyemezsiniz.");
            return;
        }

        Console.WriteLine("Seçim durumu (onayla/iptal):");
        var status = Console.ReadLine()?.ToLower();
        
        if (status == "onayla")
        {
            selection.Confirmed = true;
            selection.Cancelled = false;
            Console.WriteLine("Ders seçimi onaylandı.");
        }
        else if (status == "iptal")
        {
            selection.Cancelled = true;
            selection.Confirmed = false;
            Console.WriteLine("Ders seçimi iptal edildi.");
        }
        
        try
        {
            _studentCourseSelectionRepository.UpdateSelection(selection);
            Console.WriteLine("Ders seçimi durumu güncellendi.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            throw;
        }
    }
    
    public void DeleteCourseSelection()
    {
        Console.WriteLine("Ders seçimini sil:");
        Console.Write("Öğrenci ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var student = _studentRepository.GetStudentById(studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        Console.WriteLine("Dönem ID'sini girin:");
        if (!int.TryParse(Console.ReadLine(), out int semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        var selection = _studentCourseSelectionRepository.GetAllSelections()
            .FirstOrDefault(scs => scs.StudentId == studentId && scs.SemesterId == semesterId);
        if (selection == null)
        {
            Console.WriteLine("Seçim bulunamadı.");
            return;
        }

        try
        {
            _studentCourseSelectionRepository.DeleteSelection(selection.Id);
            Console.WriteLine("Ders seçimi silindi.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
            throw;
        }
    }
    
    public void ListCourseSelections()
    {
        Console.WriteLine("Ders seçimlerini listele:");
        var selections = _studentCourseSelectionRepository.GetAllSelections();
        foreach (var selection in selections)
        {
            Console.WriteLine($"- ID: {selection.Id}, Öğrenci ID: {selection.StudentId}, Dönem ID: {selection.SemesterId}");
            Console.WriteLine("Seçilen dersler:");
            foreach (var course in selection.Courses)
            {
                Console.WriteLine($"  - ID: {course.Id}, Ad: {course.Name}, Kredi: {course.Credit}");
            }
        }
    }
    
    public void ListCourseSelectionsByStudentId()
    {
        Console.WriteLine("Öğrenci ID'si gir:");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }
        
        Console.WriteLine("Öğrenci ID'sine göre ders seçimlerini listele:");
        var selections = _studentCourseSelectionRepository.GetSelectionsByStudentId(studentId);
        foreach (var selection in selections)
        {
            Console.WriteLine($"- ID: {selection.Id}, Öğrenci ID: {selection.StudentId}, Dönem ID: {selection.SemesterId}");
            Console.WriteLine("Seçilen dersler:");
            foreach (var course in selection.Courses)
            {
                Console.WriteLine($"  - ID: {course.Id}, Ad: {course.Name}, Kredi: {course.Credit}");
            }
        }
    }
}