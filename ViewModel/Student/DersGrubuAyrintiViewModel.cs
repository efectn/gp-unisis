using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using gp_unisis.Database.Entities;

namespace gp_unisis.ViewModel.Student
{
    class DersGrubuAyrintiViewModel : ViewModelBase
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

        public ObservableCollection<Ders> Dersler { get; set; } = new ObservableCollection<Ders>();
        public int Credits { get; set; } = 0;
        public int CoursesCount { get; set; } = 0;
        public int RequiredCourses { get; set; } = 0;
        public int RequiredCredits { get; set; } = 0;

        public DersGrubuAyrintiViewModel(MainWindowViewModel mainVM, CourseGroup courseGroup)
        {
            _mainVM = mainVM;

            // Navigation Commands
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
                _mainVM.Globals.LoggedUser = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var user = _mainVM.Globals?.LoggedUser;
            var transcriptRepo = _mainVM.Globals?.TranscriptRepository;

            if (user == null || transcriptRepo == null || courseGroup == null)
                return;

            var transcriptCourses = transcriptRepo.GetAllTranscripts()
                ?.Where(tc => tc.StudentId == user.Id && !tc.HasFailed)
                ?.Select(tc => tc.CourseCode)
                ?.ToList() ?? new List<string>();

            var courses = courseGroup.Courses ?? new List<Course>();

            RequiredCourses = courseGroup.RequiredCoursesCount;
            RequiredCredits = courseGroup.RequiredCredits;

            foreach (var course in courses)
            {
                bool passed = transcriptCourses.Contains(course.Code);
                if (passed)
                {
                    Credits += course.Credit;
                    CoursesCount++;
                }

                Dersler.Add(new Ders
                {
                    Name = course.Name,
                    Credit = course.Credit.ToString(),
                    Passed = passed ? "Evet" : "Hayır",
                    Lecturer = course.Lecturer?.FullName ?? "Bilinmiyor",
                    Semester = course.SemesterNumber.ToString()
                });
            }
        }
    }

    public class Ders
    {
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Passed { get; set; }
        public string Lecturer { get; set; }
        public string Semester { get; set; }
    }
}
