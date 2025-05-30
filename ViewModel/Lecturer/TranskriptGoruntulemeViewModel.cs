using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Database.Entities;
using gp_unisis.ViewModel;

namespace gp_unisis.ViewModel.Lecturer
{
    class TranskriptGoruntulemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;

        public ICommand AkademisyenAnaSayfaCommand { get; set; }
        public ICommand DersDuzenleSilCommand { get; set; }
        public ICommand DerseSinavEkleCommand { get; set; }
        public ICommand DersProgramiCommand { get; set; }
        public ICommand DuyuruGoruntulemeCommand { get; set; }
        public ICommand NotGirisiCommand { get; set; }
        public ICommand ProgramaDersEkleCommand { get; set; }
        public ICommand SinavProgramiCommand { get; set; }
        public ICommand TranskriptAnaSayfaCommand { get; set; }
        public ICommand TranskriptGoruntulemeCommand { get; set; }
        public ICommand TranskriptHesaplamaCommand { get; set; }
        public ICommand NotTekrarHesapla { get; set; }
        public ICommand DersListesiCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }

        public ICommand LogOutCommand { get; set; }
        public CourseLetterGradeInterval LetterGradeInterval { get; set; }

        public ObservableCollection<Sonuc2> Sonuclar { get; set; } = new ObservableCollection<Sonuc2>();

        public TranskriptGoruntulemeViewModel(MainWindowViewModel mainVM, int id)
        {
            _mainVM = mainVM;

            AkademisyenAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM));
            
            DerseSinavEkleCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM));

            DersProgramiCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            
            SinavProgramiCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));

            TranskriptAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM));
            
            DersListesiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM));
            DersEkleCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersEkleViewModel(_mainVM));


            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var course = _mainVM.Globals.CourseRepository.GetCourseById(id);
            if (course == null)
            {
                MessageBox.Show("Ders bulunamadı.");
                return;
            }

            var students = _mainVM.Globals.StudentCourseSelectionRepository.GetAllSelections()
                .Where(s => s.Confirmed && s.Courses.Any(c => c.Id == course.Id) &&
                            s.SemesterId == _mainVM.Globals.ActiveSemesterId)
                .Select(s => s.Student).ToList();

            var letterGrades = _mainVM.Globals.CourseLetterGradeIntervalRepository
                .GetIntervalsByCourseAndSemester(course.Id, _mainVM.Globals.ActiveSemesterId).FirstOrDefault();
            if (letterGrades == null)
            {
                MessageBox.Show(
                    "Bu sınavın harf notu henüz hesaplanmamış!");
            }
            else
            {
                LetterGradeInterval = letterGrades;
            }

            LoadSonuclar(id);

            NotTekrarHesapla = new RelayCommand(param =>
            {
                if (param is Sonuc2 s)
                {
                    var semesterId = _mainVM.Globals.ActiveSemesterId;
                    var studentId = s.StudentId;
                    var courseId = s.CourseId;
                    
                    var student = _mainVM.Globals.StudentCourseSelectionRepository.GetAllSelections()
                        .Where(s => s.Confirmed && s.Courses.Any(c => c.Id == courseId) && s.SemesterId == semesterId)
                        .Select(s => s.Student).FirstOrDefault(s => s.Id == studentId);

                    if (student == null)
                    {
                        MessageBox.Show("Bu derse kayıtlı öğrenci yok.");
                    }

                    // Check if there are exams defined and sum of coefficients is 100
                    var exams = _mainVM.Globals.ExamRepository.GetExamsByCourseId(courseId)
                        .Where(e => e.SemesterId == semesterId).ToList();
                    if (exams.Count == 0)
                    {
                        MessageBox.Show("Bu derse ait sınav yok.");
                        return;
                    }

                    var totalCoefficient = exams.Sum(e => e.ExamCoefficient);
                    if (totalCoefficient != 100)
                    {
                        MessageBox.Show("Sınavların toplam katsayısı 100 olmalıdır.");
                        return;
                    }

                    // Check if there are grades defined for students taking the courses
                    double finalNote = 0;
                    foreach (var exam in exams)
                    {
                        var grades = _mainVM.Globals.GradeRepository.GetGradesByExamId(exam.Id);
                        if (grades.Count == 0)
                        {
                            MessageBox.Show($"Bu sınav için not yok: {exam.Name}");
                            return;
                        }

                        // Check if the student has grade for the exam
                        var grade = grades.FirstOrDefault(g => g.StudentId == student.Id);
                        if (grade == null)
                        {
                            MessageBox.Show($"Bu sınav için not yok: {exam.Name}");
                            return;
                        }

                        finalNote += grade.Score * exam.ExamCoefficient / 100;
                    }

                    // Find old transcript note
                    var transcript = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().FirstOrDefault(t =>
                        t.StudentId == studentId && t.CourseId == courseId && t.SemesterId == semesterId);

                    if (transcript == null)
                    {
                        MessageBox.Show("Güncellenecek transkript notu bulunamadı.");
                    }

                    // Update the transcript note
                    var letterGrade = _mainVM.Globals.CourseLetterGradeIntervalRepository
                        .GetIntervalsByCourseAndSemester(courseId, semesterId).FirstOrDefault();
                    if (letterGrade == null)
                    {
                        MessageBox.Show("Ders için hesaplanmış harf notu aralıkları bulunamadı.");
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
                        _mainVM.Globals.TranscriptRepository.UpdateTranscript(transcript);
                        MessageBox.Show("Transkript notu başarıyla güncellendi.");
                        Sonuclar.Clear();
                        LoadSonuclar(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                }
            });
        }
        
        public double CalculateRelativeGrade(double rawGrade, double average, double standardDeviation)
        {
            var relativeGrade = (rawGrade - average) / standardDeviation;
            return relativeGrade * 10 + 60; // Scale to 0-100
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

        public void LoadSonuclar(int id)
        {
            var transcripts = _mainVM.Globals.TranscriptRepository.GetAllTranscripts()
                .Where(t => t.CourseId == id && t.SemesterId == _mainVM.Globals.ActiveSemesterId).ToList();

            foreach (var transcript in transcripts)
            {
                var sonuc = new Sonuc2
                {
                    Id = transcript.Id,
                    OgrenciAdi = transcript.Student.FirstName + " " + transcript.Student.LastName,
                    OgrenciNo = transcript.Student.StudentNumber,
                    HarfNotu = LetterGradeToString(transcript.LetterGrade),
                    Status = transcript.LetterGrade <= 0.5 ? "Başarısız" : "Başarılı",
                    StudentId = transcript.Student.Id,
                    CourseId = transcript.Course.Id,
                };
                Sonuclar.Add(sonuc);
            }
        }

        public string LetterGradeToString(double grade)
        {
            switch (grade)
            {
                case 4:
                    return "AA";
                case 3.5:
                    return "BA";
                case 3:
                    return "BB";
                case 2.5:
                    return "CB";
                case 2:
                    return "CC";
                case 1.5:
                    return "DC";
                case 1:
                    return "DD";
                case 0.5:
                    return "FD";
                case 0:
                    return "FF";
                default:
                    return "-";
            }
        }

        public class Sonuc2
        {
            public int Id { get; set; }
            public int StudentId { get; set; }
            public int CourseId { get; set; }

            public string OgrenciNo { get; set; }
            public string OgrenciAdi { get; set; }
            public string HarfNotu { get; set; }
            public string Status { get; set; }
        }
    }
}