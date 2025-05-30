using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gp_unisis.Database.Entities;

namespace gp_unisis.ViewModel.Student
{
    class DonemDerslerimViewModel : ViewModelBase
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
        
        public ObservableCollection<Course> Courses { get; set; }
        public string CreditsCount {get;set;}
        public string ActiveSemester { get; set; }

        public DonemDerslerimViewModel(MainWindowViewModel mainVM)
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

            var user = _mainVM.Globals?.LoggedUser;
            var repo = _mainVM.Globals?.StudentCourseSelectionRepository;
            var semesterId = _mainVM.Globals?.ActiveSemesterId;
            var semester = _mainVM.Globals?.ActiveSemester;

            if (user == null || repo == null || semesterId == null || semester == null)
            {
                CreditsCount = "0 / 40";
                ActiveSemester = "Bilinmiyor";
                return;
            }

            var selection = repo.GetSelectionsByStudentId(user.Id)
                .FirstOrDefault(s => s.SemesterId == semesterId && s.Confirmed);

            var selectedCourses = selection?.Courses?.ToList() ?? new List<Course>();
            Courses = new ObservableCollection<Course>(selectedCourses);

            CreditsCount = selectedCourses.Sum(c => c.Credit) + " / 40";
            ActiveSemester = semester.Name;
        }
    }
}
