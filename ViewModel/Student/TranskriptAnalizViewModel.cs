using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace gp_unisis.ViewModel.Student
{
    class TranskriptAnalizViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AnasayfaCommand { get; set; }
        public ICommand DersSecimiCommand { get; set; }
        public ICommand DonemDersleriCommand { get; set; }
        public ICommand NotlarimCommand { get; set; }
        public ICommand SinavProgramiCommand { get; set; }
        public ICommand TranskriptCommand { get; set; }
        public ICommand DersProgramiCommand { get; set; }
        public ICommand DersGruplariCommand { get; set; }

        public ICommand LogOutCommand { get; set; }
        public string BellCurve { get; set; }
        public string Average { get; set; }
        public string Stdev { get; set; }

        public ObservableCollection<GradeInterval> Araliklar { get; set; } = new ObservableCollection<GradeInterval>();

        public TranskriptAnalizViewModel(MainWindowViewModel mainVM, int id)
        {
            _mainVM = mainVM;

            AnasayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new StudentDashboardViewModel(_mainVM));
            DersSecimiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSecimiViewModel(_mainVM));
            DonemDersleriCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DonemDerslerimViewModel(_mainVM));
            NotlarimCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new NotlarimViewModel(_mainVM));
            SinavProgramiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            TranskriptCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new TranskriptViewModel(_mainVM));
            DersProgramiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            DersGruplariCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersGruplariViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var transcript = _mainVM.Globals.TranscriptRepository.GetTranscriptById(id);
            if (transcript == null)
            {
                MessageBox.Show("Transkript girdisi bulunamadı!");
                return;
            }

            var letterGrades = _mainVM.Globals.CourseLetterGradeIntervalRepository
                .GetIntervalsByCourseAndSemester(transcript.CourseId, transcript.SemesterId).FirstOrDefault();
            if (letterGrades == null)
            {
                MessageBox.Show("Harf notu aralıkları bulunamadı!");
                return;
            }

            Average = "Ortalama: " + letterGrades.Average;
            Stdev = "Standart Sapma: " + letterGrades.Stdev;
            BellCurve = "Çan Eğrisi: " + (letterGrades.IsBellCurve ? "Var" : "Yok");

            var interval = new GradeInterval
            {
                LetterGrade = "AA",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 4.0).ToString(),
                Interval = letterGrades.AAStart + "- " + letterGrades.AAEnd,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "BA",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 3.5).ToString(),
                Interval = letterGrades.BAStart + "- " + letterGrades.BAEnd,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "BB",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 3.0).ToString(),
                Interval = letterGrades.BBStart + "- " + letterGrades.BBEnd,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "CB",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 2.5).ToString(),
                Interval = letterGrades.CBStart + "- " + letterGrades.CBEnd,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "CC",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 2.0).ToString(),
                Interval = letterGrades.CCStart + "- " + letterGrades.CCEnd,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "DC",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 1.5).ToString(),
                Interval = letterGrades.DCStart + "- " + letterGrades.DCEnd,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "DD",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 1.0).ToString(),
                Interval = letterGrades.DDStart + "- " + letterGrades.DDStart,
            };
            Araliklar.Add(interval);

            interval = new GradeInterval
            {
                LetterGrade = "FD",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 0.5).ToString(),
                Interval = letterGrades.FDStart + "- " + letterGrades.FDEnd,
            };
            Araliklar.Add(interval);
            interval = new GradeInterval
            {
                LetterGrade = "FF",
                Count = _mainVM.Globals.TranscriptRepository.GetAllTranscripts().Count(t => t.SemesterId == _mainVM.Globals.ActiveSemesterId && t.CourseId == transcript.CourseId && t.LetterGrade == 0).ToString(),
                Interval = "0 - 49",
            };
            Araliklar.Add(interval);
        }

        public class GradeInterval
        {
            public string LetterGrade { get; set; }
            public string Interval { get; set; }
            public string Count { get; set; }
        }
    }
}
