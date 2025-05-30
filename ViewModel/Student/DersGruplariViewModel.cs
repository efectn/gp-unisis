using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using gp_unisis.Database.Entities;

namespace gp_unisis.ViewModel.Student
{
    class DersGruplariViewModel : ViewModelBase
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

        public ObservableCollection<CourseGroup> CourseGroups { get; set; } = new ObservableCollection<CourseGroup>();
        public ICommand CourseGroupAyrintiCommand { get; set; }
        public string MezunOlabilirMi { get; set; }
        public string Credits { get; set; }
        public string Courses { get; set; }
        public string AktifDonem { get; set; }

        public DersGruplariViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;

            // Navigation commands
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

            var user = _mainVM.Globals?.LoggedUser;
            var activeSemester = _mainVM.Globals?.ActiveSemester;

            if (user != null && activeSemester != null)
            {
                var courseGroups = _mainVM.Globals.CourseGroupRepository
                    ?.GetCourseGroupsByDepartmentId(user.DepartmentId)
                    ?.Where(cg => cg.EntranceSemesterId == user.EntranceSemesterId)
                    ?.ToList() ?? new List<CourseGroup>();

                var transcriptCourses = _mainVM.Globals.TranscriptRepository
                    ?.GetAllTranscripts()
                    ?.Where(tc => tc.StudentId == user.Id && !tc.HasFailed)
                    ?.ToList() ?? new List<Transcript>();

                var transcripts2 = transcriptCourses
                    .GroupBy(t => t.CourseId)
                    .Select(g => g.OrderByDescending(t => t.SemesterId).First())
                    .ToList();

                Credits = $"{transcripts2.Sum(t => t.Course?.Credit ?? 0)} / {courseGroups.Sum(c => c.RequiredCredits)}";
                Courses = $"{transcripts2.Count} / {courseGroups.Sum(c => c.RequiredCoursesCount)}";
                AktifDonem = activeSemester.Name;
                CourseGroups = new ObservableCollection<CourseGroup>(courseGroups);

                var transcriptCourseCodes = transcriptCourses.Select(tc => tc.CourseCode).ToList();
                MezunOlabilirMi = CanStudentGraduate(courseGroups, transcriptCourseCodes) ? "Evet" : "Hayır";
            }

            CourseGroupAyrintiCommand = new RelayCommand(param =>
            {
                if (param is CourseGroup courseGroup)
                {
                    _mainVM.CurrentViewModel = new DersGrubuAyrintiViewModel(_mainVM, courseGroup);
                }
            });
        }

        public bool CanStudentGraduate(List<CourseGroup> courseGroups, List<string> transcriptCourses)
        {
            foreach (var courseGroup in courseGroups)
            {
                int requiredCoursesCount = courseGroup.RequiredCoursesCount;
                int requiredCredits = courseGroup.RequiredCredits;

                foreach (var course in courseGroup.Courses ?? new List<Course>())
                {
                    if (transcriptCourses.Contains(course.Code))
                    {
                        requiredCoursesCount--;
                        requiredCredits -= course.Credit;
                    }
                }

                if (requiredCoursesCount > 0 || requiredCredits > 0)
                    return false;
            }

            return true;
        }
    }
}
