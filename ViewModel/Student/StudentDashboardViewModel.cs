using gp_unisis.Commands;
using gp_unisis.ViewModel.Student;
using gp_unisis.Views.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gp_unisis.ViewModel.Student
{
    class StudentDashboardViewModel : ViewModelBase
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
        
        public string StudentName { get; set; }
        public string StudentNumber { get; set; }
        public string StudentDepartment { get; set; }
        public string StudentFaculty { get; set; }
        public string StudentEntranceSemester { get; set; }
        public string Graduation { get; set; }
        

        public StudentDashboardViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainVM = mainWindowViewModel;

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

            var student = _mainVM.Globals.LoggedUser;
            StudentName = "Ad Soyad: " + student.FirstName + " " + student.LastName;
            StudentNumber = "Öğrenci No: " + student.StudentNumber;
            StudentDepartment = "Bölüm: " + student.Department.Name;
            StudentFaculty = "Fakülte: " + student.Department.Faculty.Name;
            StudentEntranceSemester = "Giriş Dönemi: " + student.EntranceSemester.Name;
            Graduation = student.IsGraduated ? "Mezun" : "Devamlı Öğrenci";
        }
    }
}
