using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class TranscriptViewModel
{
    private readonly StudentCourseSelectionRepository _studentCourseSelectionRepository;
    private readonly GradeRepository _gradeRepository;
    private readonly LecturerRepository _lecturerRepository;
    private readonly ExamRepository _examRepository;
    private readonly CourseLetterGradeIntervalRepository _letterGradeRepository;
    private readonly TranscriptRepository _transcriptRepository;

    public TranscriptViewModel(
        StudentCourseSelectionRepository studentCourseSelectionRepository,
        GradeRepository gradeRepository,
        LecturerRepository lecturerRepository,
        ExamRepository examRepository,
        CourseLetterGradeIntervalRepository letterGradeRepository,
        TranscriptRepository transcriptRepository
    )
    {
        _studentCourseSelectionRepository = studentCourseSelectionRepository;
        _gradeRepository = gradeRepository;
        _lecturerRepository = lecturerRepository;
        _examRepository = examRepository;
        _letterGradeRepository = letterGradeRepository;
        _transcriptRepository = transcriptRepository;
    }


    public void CalculateTranscriptNotesByLecturer()
    {
        Console.WriteLine("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        Console.WriteLine("Akademisyen ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var lecturerId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var lecturer = _lecturerRepository.GetLecturerById(lecturerId);
        if (lecturer == null)
        {
            Console.WriteLine("Akademisyen bulunamadı.");
            return;
        }

        var courses = lecturer.Courses
            .Where(c => c.Semesters.Any(s => s.Id == semesterId))
            .ToList();

        if (courses.Count == 0)
        {
            Console.WriteLine("Bu dönemde ders vermiyorsunuz.");
        }

        Console.WriteLine("Dersler: ");
        foreach (var course in courses)
        {
            Console.WriteLine($"ID: {course.Id}, Adı: {course.Name}, Kodu: {course.Code}, Kredi: {course.Credit}");
        }

        Console.WriteLine("Final notlarınıı hesaplamak istediğiniz dersin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }

        var courseToCalculate = courses.FirstOrDefault(c => c.Id == courseId);
        if (courseToCalculate == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }

        var studentCourseSelections = _studentCourseSelectionRepository.GetAllSelections()
            .Where(s => s.Confirmed && s.Courses.Any(c => c.Id == courseId))
            .Select(s => s.Student).ToList();
        if (studentCourseSelections.Count == 0)
        {
            Console.WriteLine("Bu derse kayıtlı öğrenci yok.");
        }

        // Check if there are exams defined and sum of coefficients is 100
        var exams = _examRepository.GetExamsByCourseId(courseId).Where(e => e.SemesterId == semesterId).ToList();
        if (exams.Count == 0)
        {
            Console.WriteLine("Bu derse ait sınav yok.");
            return;
        }

        var totalCoefficient = exams.Sum(e => e.ExamCoefficient);
        if (totalCoefficient != 100)
        {
            Console.WriteLine("Sınavların toplam katsayısı 100 olmalıdır.");
            return;
        }

        Console.WriteLine("Bağıl mı ham not mu? (B/H): ");
        var isRelative = Console.ReadLine()?.ToUpper() == "B";

        // Check if there are grades defined for students taking the courses
        foreach (var exam in exams)
        {
            var grades = _gradeRepository.GetGradesByExamId(exam.Id);
            if (grades.Count == 0)
            {
                Console.WriteLine($"Bu sınav için not yok: {exam.Name}");
                return;
            }

            // Check if all students having grades are registered for the course
            var studentsWithGrades = grades.Select(g => g.Student).ToList();
            if (studentCourseSelections.Count > studentsWithGrades.Count)
            {
                Console.WriteLine($"Bu derse kayıtlı öğrencilerden bazıları için not yok: {courseToCalculate.Name}");
                return;
            }
        }

        var finalGrades = new Dictionary<int, double>();

        // Calculate the final grade
        foreach (var student in studentCourseSelections)
        {
            var finalGrade = 0.0;
            foreach (var exam in exams)
            {
                var grade = _gradeRepository.GetGradesByExamId(exam.Id).FirstOrDefault(g => g.StudentId == student.Id);
                if (grade != null)
                {
                    finalGrade += grade.Score * exam.ExamCoefficient / 100;
                }
            }

            finalGrades[student.Id] = finalGrade;
        }

        // Set exams to calculated
        foreach (var exam in exams)
        {
            exam.IsExamCalculated = true;
            _examRepository.UpdateExam(exam);
        }

        var average = finalGrades.Values.Average();
        var standardDeviation = Math.Sqrt(finalGrades.Values.Sum(g => Math.Pow(g - average, 2)) / finalGrades.Count);

        // Calculate intervals
        var letterGrade = CreateLetterGradeItem(semesterId, courseId, isRelative, average, standardDeviation);

        // Print final grades
        Console.WriteLine("Final Notları:");
        foreach (var student in studentCourseSelections)
        {
            if (finalGrades.TryGetValue(student.Id, out var finalGrade))
            {
                Console.WriteLine($"Öğrenci ID: {student.Id}, Final Notu: {finalGrade}");
            }
        }
        
        // Add raw grades to the transcript
        if (!isRelative)
        {
            foreach (var grade in finalGrades)
            {
                var studentId = grade.Key;
                var rawGrade = grade.Value;
                var gradeWeight = GetLetterGradeWeight(letterGrade, rawGrade);

                var transcriptNote = new Transcript
                {
                    StudentId = studentId,
                    CourseId = courseToCalculate.Id,
                    SemesterId = semesterId,
                    CourseName = courseToCalculate.Name,
                    CourseCode = courseToCalculate.Code,
                    LetterGrade = gradeWeight,
                    HasFailed = gradeWeight <= 0.5
                };

                try
                {
                    _transcriptRepository.AddTranscript(transcriptNote);
                    Console.WriteLine("Transkript notu başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                }
            }
        }
        else
        {
            foreach (var grade in finalGrades)
            {
                var studentId = grade.Key;
                var rawGrade = grade.Value;

                var relativeGrade = CalculateRelativeGrade(rawGrade, average, standardDeviation);
                var gradeWeight = GetLetterGradeWeight(letterGrade, relativeGrade);

                var transcriptNote = new Transcript
                {
                    StudentId = studentId,
                    CourseId = courseToCalculate.Id,
                    SemesterId = semesterId,
                    CourseName = courseToCalculate.Name,
                    CourseCode = courseToCalculate.Code,
                    LetterGrade = gradeWeight,
                    HasFailed = gradeWeight <= 0.5
                };

                try
                {
                    _transcriptRepository.AddTranscript(transcriptNote);
                    Console.WriteLine("Transkript notu başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.InnerException.Message}");

                    Console.WriteLine($"Hata: {ex.Message}");
                }
            }
        }
    }

    public void RecalculateTranscriptNoteForStudent()
    {
        Console.WriteLine("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }

        Console.WriteLine("Akademisyen ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var lecturerId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var lecturer = _lecturerRepository.GetLecturerById(lecturerId);
        if (lecturer == null)
        {
            Console.WriteLine("Akademisyen bulunamadı.");
            return;
        }

        var courses = lecturer.Courses
            .Where(c => c.Semesters.Any(s => s.Id == semesterId))
            .ToList();

        if (courses.Count == 0)
        {
            Console.WriteLine("Bu dönemde ders vermiyorsunuz.");
        }

        Console.WriteLine("Dersler: ");
        foreach (var course in courses)
        {
            Console.WriteLine($"ID: {course.Id}, Adı: {course.Name}, Kodu: {course.Code}, Kredi: {course.Credit}");
        }

        Console.WriteLine("Final notlarınıı hesaplamak istediğiniz dersin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }

        var courseToCalculate = courses.FirstOrDefault(c => c.Id == courseId);
        if (courseToCalculate == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }

        Console.WriteLine(
            "Transkript notunu son sınav notlarına göre güncellemekj istediğiniz öğrencinin ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }

        var student = _studentCourseSelectionRepository.GetAllSelections()
            .Where(s => s.Confirmed && s.Courses.Any(c => c.Id == courseId) && s.SemesterId == semesterId)
            .Select(s => s.Student).FirstOrDefault(s => s.Id == studentId);

        if (student == null)
        {
            Console.WriteLine("Bu derse kayıtlı öğrenci yok.");
        }

        // Check if there are exams defined and sum of coefficients is 100
        var exams = _examRepository.GetExamsByCourseId(courseId).Where(e => e.SemesterId == semesterId).ToList();
        if (exams.Count == 0)
        {
            Console.WriteLine("Bu derse ait sınav yok.");
            return;
        }

        var totalCoefficient = exams.Sum(e => e.ExamCoefficient);
        if (totalCoefficient != 100)
        {
            Console.WriteLine("Sınavların toplam katsayısı 100 olmalıdır.");
            return;
        }

        // Check if there are grades defined for students taking the courses
        double finalNote = 0;
        foreach (var exam in exams)
        {
            var grades = _gradeRepository.GetGradesByExamId(exam.Id);
            if (grades.Count == 0)
            {
                Console.WriteLine($"Bu sınav için not yok: {exam.Name}");
                return;
            }

            // Check if the student has grade for the exam
            var grade = grades.FirstOrDefault(g => g.StudentId == student.Id);
            if (grade == null)
            {
                Console.WriteLine($"Bu sınav için not yok: {exam.Name}");
                return;
            }

            finalNote += grade.Score * exam.ExamCoefficient / 100;
        }
        
        // Find old transcript note
        var transcript = _transcriptRepository.GetAllTranscripts().FirstOrDefault(t =>
            t.StudentId == studentId && t.CourseId == courseId && t.SemesterId == semesterId);
        
        if (transcript == null)
        {
            Console.WriteLine("Transkript notu bulunamadı.");
        }
        
        // Update the transcript note
        var letterGrade = _letterGradeRepository.GetIntervalsByCourseAndSemester(courseId, semesterId).FirstOrDefault();
        if (letterGrade == null)
        {
            Console.WriteLine("Ders için hesaplanmış harf notu aralıkları bulunamadı.");
            return;
        }

        if (letterGrade.IsBellCurve)
        {
            var relativeGrade = CalculateRelativeGrade(finalNote, letterGrade.Average, letterGrade.Stdev);
            transcript.LetterGrade = GetLetterGradeWeight(letterGrade, relativeGrade);
        }
        else
        {
            transcript.LetterGrade = GetLetterGradeWeight(letterGrade, finalNote);
        }
        transcript.HasFailed = transcript.LetterGrade <= 0.5;
        
        // Save the updated transcript
        try
        {
            _transcriptRepository.UpdateTranscript(transcript);
            Console.WriteLine("Transkript notu başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    public void DeleteTranscript()
    {
        Console.WriteLine("Dönem ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var semesterId))
        {
            Console.WriteLine("Geçersiz dönem ID'si.");
            return;
        }
        
        Console.WriteLine("Öğrenci ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var studentId))
        {
            Console.WriteLine("Geçersiz öğrenci ID'si.");
            return;
        }
        
        Console.WriteLine("Ders ID'si girin: ");
        if (!int.TryParse(Console.ReadLine(), out var courseId))
        {
            Console.WriteLine("Geçersiz ders ID'si.");
            return;
        }
        
        var transcript = _transcriptRepository.GetAllTranscripts().FirstOrDefault(t =>
            t.StudentId == studentId && t.CourseId == courseId && t.SemesterId == semesterId);
        if (transcript == null)
        {
            Console.WriteLine("Transkript notu bulunamadı.");
        }

        try
        {
            _transcriptRepository.DeleteTranscript(transcript.Id);
            Console.WriteLine("Transkript notu başarıyla silindi.");
        }
        catch
        {
            Console.WriteLine("Transkript notu silinemedi.");
        }
    }

    public double CalculateRelativeGrade(double rawGrade, double average, double standardDeviation)
    {
        var relativeGrade = (rawGrade - average) / standardDeviation;
        return relativeGrade * 10 + 60; // Scale to 0-100
    }

    public CourseLetterGradeInterval CreateLetterGradeItem(int semesterId, int courseId, bool isRelative, double avg, double stdev)
    {
        var letterGrade = new CourseLetterGradeInterval();
        letterGrade.SemesterId = semesterId;
        letterGrade.CourseId = courseId;
        letterGrade.IsBellCurve = true;
        letterGrade.Average = avg;
        letterGrade.Stdev = stdev;

        if (!isRelative)
        {
            letterGrade.IsBellCurve = false;

            letterGrade.AAStart = 90;
            letterGrade.AAEnd = 100;
            letterGrade.BAStart = 80;
            letterGrade.BAEnd = 89;
            letterGrade.BBStart = 75;
            letterGrade.BBEnd = 79;
            letterGrade.CBStart = 70;
            letterGrade.CBEnd = 74;
            letterGrade.CCStart = 65;
            letterGrade.CCEnd = 69;
            letterGrade.DCStart = 60;
            letterGrade.DCEnd = 64;
            letterGrade.DDStart = 50;
            letterGrade.DDEnd = 59;
            letterGrade.FDStart = 35;
            letterGrade.FDEnd = 49;
        } else if (avg > 80.0 && avg <= 100)
        {
            letterGrade.AAStart = 67;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 62;
            letterGrade.BAEnd = 66.99;

            letterGrade.BBStart = 57;
            letterGrade.BBEnd = 61.99;

            letterGrade.CBStart = 52;
            letterGrade.CBEnd = 56.99;

            letterGrade.CCStart = 47;
            letterGrade.CCEnd = 51.99;

            letterGrade.DCStart = 42;
            letterGrade.DCEnd = 46.99;

            letterGrade.DDStart = 37;
            letterGrade.DDEnd = 41.99;

            letterGrade.FDStart = 32;
            letterGrade.FDEnd = 36.99;
        }
        else if (avg > 70 && avg <= 80)
        {
            letterGrade.AAStart = 69;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 64;
            letterGrade.BAEnd = 68.99;

            letterGrade.BBStart = 59;
            letterGrade.BBEnd = 63.99;

            letterGrade.CBStart = 54;
            letterGrade.CBEnd = 58.99;

            letterGrade.CCStart = 49;
            letterGrade.CCEnd = 53.99;

            letterGrade.DCStart = 44;
            letterGrade.DCEnd = 48.99;

            letterGrade.DDStart = 39;
            letterGrade.DDEnd = 43.99;

            letterGrade.FDStart = 34;
            letterGrade.FDEnd = 38.99;
        }
        else if (avg > 62.5 && avg <= 70)
        {
            letterGrade.AAStart = 71;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 66;
            letterGrade.BAEnd = 70.99;

            letterGrade.BBStart = 61;
            letterGrade.BBEnd = 65.99;

            letterGrade.CBStart = 56;
            letterGrade.CBEnd = 60.99;

            letterGrade.CCStart = 51;
            letterGrade.CCEnd = 55.99;

            letterGrade.DCStart = 46;
            letterGrade.DCEnd = 50.99;

            letterGrade.DDStart = 41;
            letterGrade.DDEnd = 45.99;

            letterGrade.FDStart = 36;
            letterGrade.FDEnd = 40.99;
        }
        else if (avg > 57.5 && avg <= 62.5)
        {
            letterGrade.AAStart = 73;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 68;
            letterGrade.BAEnd = 72.99;

            letterGrade.BBStart = 63;
            letterGrade.BBEnd = 67.99;

            letterGrade.CBStart = 58;
            letterGrade.CBEnd = 62.99;

            letterGrade.CCStart = 53;
            letterGrade.CCEnd = 57.99;

            letterGrade.DCStart = 48;
            letterGrade.DCEnd = 52.99;

            letterGrade.DDStart = 43;
            letterGrade.DDEnd = 47.99;

            letterGrade.FDStart = 38;
            letterGrade.FDEnd = 42.99;
        }
        else if (avg > 52.5 && avg <= 57.5)
        {
            letterGrade.AAStart = 75;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 70;
            letterGrade.BAEnd = 74.99;

            letterGrade.BBStart = 65;
            letterGrade.BBEnd = 69.99;

            letterGrade.CBStart = 60;
            letterGrade.CBEnd = 64.99;

            letterGrade.CCStart = 55;
            letterGrade.CCEnd = 59.99;

            letterGrade.DCStart = 50;
            letterGrade.DCEnd = 54.99;

            letterGrade.DDStart = 45;
            letterGrade.DDEnd = 49.99;

            letterGrade.FDStart = 40;
            letterGrade.FDEnd = 44.99;
        }
        else if (avg > 47.5 && avg <= 52.5)
        {
            letterGrade.AAStart = 77;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 72;
            letterGrade.BAEnd = 76.99;

            letterGrade.BBStart = 67;
            letterGrade.BBEnd = 71.99;

            letterGrade.CBStart = 62;
            letterGrade.CBEnd = 66.99;

            letterGrade.CCStart = 57;
            letterGrade.CCEnd = 61.99;

            letterGrade.DCStart = 52;
            letterGrade.DCEnd = 56.99;

            letterGrade.DDStart = 47;
            letterGrade.DDEnd = 51.99;

            letterGrade.FDStart = 42;
            letterGrade.FDEnd = 46.99;
        }
        else if (avg > 42.5 && avg <= 47.5)
        {
            letterGrade.AAStart = 79;
            letterGrade.AAEnd = 100;

            letterGrade.BAStart = 74;
            letterGrade.BAEnd = 78.99;

            letterGrade.BBStart = 69;
            letterGrade.BBEnd = 73.99;

            letterGrade.CBStart = 64;
            letterGrade.CBEnd = 68.99;

            letterGrade.CCStart = 59;
            letterGrade.CCEnd = 63.99;

            letterGrade.DCStart = 54;
            letterGrade.DCEnd = 58.99;

            letterGrade.DDStart = 49;
            letterGrade.DDEnd = 53.99;

            letterGrade.FDStart = 44;
            letterGrade.FDEnd = 48.99;
        }
        else if (avg > 0 && avg <= 42.5)
        {
            letterGrade.AAStart = 100;
            letterGrade.AAEnd = 81;

            letterGrade.BAStart = 76;
            letterGrade.BAEnd = 80.99;

            letterGrade.BBStart = 70;
            letterGrade.BBEnd = 75.99;

            letterGrade.CBStart = 66;
            letterGrade.CBEnd = 70.99;

            letterGrade.CCStart = 61;
            letterGrade.CCEnd = 65.99;

            letterGrade.DCStart = 56;
            letterGrade.DCEnd = 60.99;

            letterGrade.DDStart = 51;
            letterGrade.DDEnd = 55.99;

            letterGrade.FDStart = 46;
            letterGrade.FDEnd = 50.99;
        }

        try
        {
            _letterGradeRepository.AddCourseLetterGradeInterval(letterGrade);
            Console.WriteLine("Harf notu aralığı başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }

        return letterGrade;
    }

    public double GetLetterGradeWeight(CourseLetterGradeInterval interval, double score)
    {
        if (score >= interval.AAStart && score <= interval.AAEnd)
            return 4.0;
        if (score >= interval.BAStart && score <= interval.BAEnd)
            return 3.5;
        if (score >= interval.BBStart && score <= interval.BBEnd)
            return 3.0;
        if (score >= interval.CBStart && score <= interval.CBEnd)
            return 2.5;
        if (score >= interval.CCStart && score <= interval.CCEnd)
            return 2.0;
        if (score >= interval.DCStart && score <= interval.DCEnd)
            return 1.5;
        if (score >= interval.DDStart && score <= interval.DDEnd)
            return 1.0;
        if (score >= interval.FDStart && score <= interval.FDEnd)
            return 0.5;

        return 0.0; // Fail
    }
}