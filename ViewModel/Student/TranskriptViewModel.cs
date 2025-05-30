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
using gp_unisis.Views.Student;

namespace gp_unisis.ViewModel.Student
{
    class TranskriptViewModel : ViewModelBase
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
        public ICommand TranskriptAnalizCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Donem> Donemler { get; set; } = new ObservableCollection<Donem>();
        private ObservableCollection<Transcript> _transkriptler;
        public ObservableCollection<Transcript> Transkriptler
        {
            get => _transkriptler;
            set => SetProperty(ref _transkriptler, value);
        }
        public string GANO { get; set; }
        private string _ano;
        public string ANO
        {
            get => _ano;
            set => SetProperty(ref _ano, value);
        }
        private int _secilenDonemId;
        public int SecilenDonemId
        {
            get => _secilenDonemId;
            set
            {
                if (SetProperty(ref _secilenDonemId, value))
                {
                    try
                    {
                        var transcripts = _mainVM.Globals.TranscriptRepository.GetAllTranscripts()
                            .Where(t => t.StudentId == _mainVM.Globals.LoggedUser.Id)
                            .Where(t => t.SemesterId == _secilenDonemId)
                            .ToList();

                        Transkriptler = new ObservableCollection<Transcript>(transcripts);

                        var ano = 0.0;
                        var credits = transcripts.Sum(t => t.Course?.Credit ?? 0);
                        foreach (var t in transcripts)
                        {
                            ano += (t.Course?.Credit ?? 0) * t.LetterGrade;
                        }

                        ANO = credits > 0 ? $"ANO: {(ano / credits):0.00}" : "ANO: Hesaplanamadı";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dönem ANO'su hesaplanırken hata oluştu:\n" + ex.Message);
                    }
                }
            }
        }


        public TranskriptViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;

            // Navigation Commands
            AnasayfaCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new StudentDashboardViewModel(_mainVM));
            DersSecimiCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersSecimiViewModel(_mainVM));
            DonemDersleriCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DonemDerslerimViewModel(_mainVM));
            NotlarimCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new NotlarimViewModel(_mainVM));
            SinavProgramiCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            TranskriptCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new TranskriptViewModel(_mainVM));
            DersProgramiCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            DersGruplariCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersGruplariViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            TranskriptAnalizCommand = new RelayCommand(param =>
            {
                if (param is Transcript t)
                {
                    _mainVM.CurrentViewModel = new TranskriptAnalizViewModel(_mainVM, t.Id);
                }
            });

            try
            {
                GANO = "GANO: " + CalculateStudentCGPA().ToString("0.00");

                var semesters = _mainVM.Globals.LoggedUser?.Transcripts?
                    .Select(t => t.SemesterId)
                    .Distinct()
                    .ToList() ?? new List<int>();

                foreach (var semesterId in semesters)
                {
                    var foundSemester = _mainVM.Globals.SemesterRepository.GetSemesterById(semesterId);
                    if (foundSemester != null)
                    {
                        Donemler.Add(new Donem
                        {
                            Id = foundSemester.Id,
                            Ad = foundSemester.Name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transkript yüklenirken bir hata oluştu:\n" + ex.Message);
            }
        }


        public double CalculateStudentCGPA()
        {
            // Only take latest attempt from transcript if a student took a course for multiple times
            var transcripts = _mainVM.Globals.TranscriptRepository.GetAllTranscripts()
                .Where(t => t.StudentId == _mainVM.Globals.LoggedUser.Id)
                .GroupBy(t => t.CourseId)  // Group only by CourseId
                .Select(g => g.OrderByDescending(t => t.SemesterId).First()) // Take the one with highest SemesterId
                .ToList();

            if (transcripts.Count == 0)
            {
                Console.WriteLine("Bu öğrenciye ait transkript notu bulunamadı.");
                return 0.0;
            }

            var cgpa = 0.0;
            var totalCredits = 0;

            foreach (var transcript in transcripts)
            {
                Console.WriteLine($"Ders Adı: {transcript.CourseName}, Kredi: {transcript.Course.Credit}, Harf Notu: {transcript.LetterGrade}");
                var credits = transcript.Course.Credit;
                var totalGradePoints = transcript.LetterGrade * credits;
                cgpa += totalGradePoints;
                totalCredits += credits;
            }

            return cgpa / totalCredits;
        }
    }

    public class Donem
    {
        public int Id { get; set; }
        public string Ad { get; set; }
    }
}
