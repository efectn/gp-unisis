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
using gp_unisis.ViewModel.Admin;

namespace gp_unisis.ViewModel.Lecturer
{
    class TranskriptHesaplamaViewModel : ViewModelBase
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
        public ICommand DersEkleCommand { get; set; }

        public ICommand LogOutCommand { get; set; }
        
        public ObservableCollection<Sonuc> Sonuclar { get; set; } = new ObservableCollection<Sonuc>();
        
        public double Ortalama { get; set; }
        public double StandartSapma { get; set; }
        
        public ICommand HamHesaplaCommand { get; set; }
        public ICommand BagilHesaplaCommand { get; set; }
        public ICommand DersListesiCommand { get; set; }

        public TranskriptHesaplamaViewModel(MainWindowViewModel mainVM, int id)
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
            
            var exams = _mainVM.Globals.ExamRepository.GetExamsByCourseId(id).Where(e => e.SemesterId == _mainVM.Globals.ActiveSemesterId).ToList();
            if (exams.Count == 0)
            {
                MessageBox.Show("Derse ait sınav yok!");
                _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM);
            }

            if (exams[0].IsExamCalculated)
            {
                MessageBox.Show(
                    "Bu sınav için zaten harf notu hesaplaması yapılmış. Görüntüle kısmından öğrenci bazında değişiklik yapabilirsiniz.");
            }
            
            var totalCoefficient = exams.Sum(e => e.ExamCoefficient);
            if (totalCoefficient != 100)
            {
                MessageBox.Show("Sınavların toplam katsayısı 100 olmalıdır.");
                _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM);
            }

            foreach (var student in students)
            {
                double finalNote = 0;
                foreach (var exam in exams)
                {
                    var grades = _mainVM.Globals.GradeRepository.GetGradesByExamId(exam.Id);
                    
                    // Check if the student has grade for the exam
                    var grade = grades.FirstOrDefault(g => g.StudentId == student.Id);
                    if (grade != null)
                    {
                        finalNote += grade.Score * exam.ExamCoefficient / 100;
                    }
                }

                var sonuc = new Sonuc
                {
                    Id = student.Id,
                    OgrenciAdi = student.FirstName + " " + student.LastName,
                    OgrenciNo = student.StudentNumber,
                    HamNot = finalNote,
                };
                Sonuclar.Add(sonuc);
            }
            
            if (Sonuclar.Count == 0)
            {
                MessageBox.Show("Dersi alan öğrenci yok!");
            }


            var average = 0.0;
            var standardDeviation = 0.0;
            if (Sonuclar.Count > 0)
            {
                average = Sonuclar.Average(s => s.HamNot);
                standardDeviation = Math.Sqrt(Sonuclar.Sum(g => Math.Pow(g.HamNot - average, 2)) / Sonuclar.Count);
            }

            Ortalama = average;
            StandartSapma = standardDeviation;

            foreach (var sonuc in Sonuclar)
            {
                sonuc.BagilNot = CalculateRelativeGrade(sonuc.HamNot, average, standardDeviation);
            }

            HamHesaplaCommand = new RelayCommand((_) =>
            {
                if (exams.Count != 0 && exams[0].IsExamCalculated)
                {
                    MessageBox.Show(
                        "Bu sınav için zaten harf notu hesaplaması yapılmış. Eski hesaplamalar siliniyor.");
                    var transcripts = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Where(c => c.SemesterId == _mainVM.Globals.ActiveSemesterId)
                        .Where(c => c.CourseId == course.Id).ToList();
                        foreach (var transcript in transcripts)
                        {
                            _mainVM.Globals.TranscriptRepository.DeleteTranscript(transcript.Id);
                        }
                }
                
                // Remove old letter grade if exists
                var oldLetterGrade = _mainVM.Globals.CourseLetterGradeIntervalRepository.GetIntervalsByCourseAndSemester(course.Id, _mainVM.Globals.ActiveSemesterId).FirstOrDefault();
                if (oldLetterGrade != null) 
                {
                    _mainVM.Globals.CourseLetterGradeIntervalRepository.DeleteCourseLetterGradeInterval(oldLetterGrade.Id);
                }
                var letterGrade = CreateLetterGradeItem(_mainVM.Globals.ActiveSemesterId, course.Id, false, average, standardDeviation);

                foreach (var grade in Sonuclar)
                {
                    var studentId = grade.Id;
                    var rawGrade = grade.HamNot;
                    var gradeWeight = GetLetterGradeWeight(letterGrade, rawGrade);

                    var transcriptNote = new Transcript
                    {
                        StudentId = studentId,
                        CourseId = course.Id,
                        SemesterId = _mainVM.Globals.ActiveSemesterId,
                        CourseName = course.Name,
                        CourseCode = course.Code,
                        LetterGrade = gradeWeight,
                        HasFailed = gradeWeight <= 0.5
                    };

                    try
                    {
                        _mainVM.Globals.TranscriptRepository.AddTranscript(transcriptNote);
                        foreach (var exam in exams)
                        {
                            exam.IsExamCalculated = true;
                            _mainVM.Globals.ExamRepository.UpdateExam(exam);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                }
            });

            BagilHesaplaCommand = new RelayCommand(_ =>
            {
                if (exams.Count != 0 && exams[0].IsExamCalculated)
                {
                    MessageBox.Show(
                        "Bu sınav için zaten harf notu hesaplaması yapılmış. Eski hesaplamalar siliniyor.");
                    var transcripts = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Where(c => c.SemesterId == _mainVM.Globals.ActiveSemesterId)
                        .Where(c => c.CourseId == course.Id).ToList();
                    foreach (var transcript in transcripts)
                    {
                        _mainVM.Globals.TranscriptRepository.DeleteTranscript(transcript.Id);
                     }
                }
                
                // Remove old letter grade if exists
                var oldLetterGrade = _mainVM.Globals.CourseLetterGradeIntervalRepository.GetIntervalsByCourseAndSemester(course.Id, _mainVM.Globals.ActiveSemesterId).FirstOrDefault();
                if (oldLetterGrade != null) 
                {
                    _mainVM.Globals.CourseLetterGradeIntervalRepository.DeleteCourseLetterGradeInterval(oldLetterGrade.Id);
                }
                var letterGrade = CreateLetterGradeItem(_mainVM.Globals.ActiveSemesterId, course.Id, true, average, standardDeviation);
                
                foreach (var grade in Sonuclar)
                {
                    var studentId = grade.Id;
                    var relativeGrade = grade.BagilNot;
                    var gradeWeight = GetLetterGradeWeight(letterGrade, relativeGrade);

                    var transcriptNote = new Transcript
                    {
                        StudentId = studentId,
                        CourseId = course.Id,
                        SemesterId = _mainVM.Globals.ActiveSemesterId,
                        CourseName = course.Name,
                        CourseCode = course.Code,
                        LetterGrade = gradeWeight,
                        HasFailed = gradeWeight <= 0.5
                    };

                    try
                    {
                        _mainVM.Globals.TranscriptRepository.AddTranscript(transcriptNote);
                        foreach (var exam in exams)
                        {
                            exam.IsExamCalculated = true;
                            _mainVM.Globals.ExamRepository.UpdateExam(exam);
                        }
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
            if (standardDeviation == 0) standardDeviation = 1;
            var relativeGrade = (rawGrade - average) / standardDeviation;
            return relativeGrade * 10 + 60; // Scale to 0-100
        }

        public CourseLetterGradeInterval CreateLetterGradeItem(int semesterId, int courseId, bool isRelative,
            double avg, double stdev)
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
            }
            else if (avg > 80.0 && avg <= 100)
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
                _mainVM.Globals.CourseLetterGradeIntervalRepository.AddCourseLetterGradeInterval(letterGrade);
                MessageBox.Show("Harf notu aralığı başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
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

    public class Sonuc
    {
        public int Id { get; set; }
        public string OgrenciNo { get; set; }
        public string OgrenciAdi { get; set; }
        public double HamNot { get; set; }
        public double BagilNot { get; set; }
    }
}