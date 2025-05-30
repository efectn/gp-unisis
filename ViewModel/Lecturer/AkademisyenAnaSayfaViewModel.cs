using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gp_unisis.ViewModel;

namespace gp_unisis.ViewModel.Lecturer
{
    class AkademisyenAnaSayfaViewModel : ViewModelBase
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
        public ICommand DersListesiCommand { get; set; }
        public ICommand TranskriptGoruntulemeCommand { get; set; }
        public ICommand TranskriptHesaplamaCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        
        public string LecturerName { get; set; }
        public string LecturerDepartment { get; set; }
        public string LecturerFaculty { get; set; }
        public string LecturerCourses { get; set; }

        public AkademisyenAnaSayfaViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;

            AkademisyenAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM));
            DerseSinavEkleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM));
            DersProgramiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            SinavProgramiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            TranskriptAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM));
            DersListesiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM));
            DersEkleCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersEkleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });
            
            var user = mainVM.Globals.LoggedLecturer;
            if (user == null)
            {
                return;
            }

            var departmentStr = "";
            var faculty = "";
            if (user.Departments != null && user.Departments.Count() > 0)
            {
                departmentStr = user.Departments.First().Name;
            }

            if (user.Departments != null && user.Departments.Count() > 0)
            {
                faculty = user.Departments.First().Faculty.Name;
            }


            LecturerName = "İsim: " + user.FullName;
            LecturerDepartment = "Bölüm: " + departmentStr;
            LecturerFaculty = "Fakülte: " + faculty;
            LecturerCourses = "Ders Sayısı: " + user.Courses.Count();
        }
    }
}