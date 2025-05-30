using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Database.Entities;
using gp_unisis.ViewModel;

namespace gp_unisis.ViewModel.Lecturer
{
    class DersEkleViewModel : ViewModelBase
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

        public string IsSelectedDisplay { get; set; }

        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int CourseCredit { get; set; }
        public int CourseSemesterNumber { get; set; }
        public int CourseQuota { get; set; }
        public string CourseDescription { get; set; }


        public DersEkleViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            AkademisyenAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM));
            DerseSinavEkleCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM));
            DersProgramiCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            SinavProgramiCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            TranskriptAnaSayfaCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM));
            DersListesiCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            DersEkleCommand = new RelayCommand(param =>
            {
                if (string.IsNullOrEmpty(CourseName) || string.IsNullOrEmpty(CourseCode) || string.IsNullOrEmpty(CourseDescription))
                {
                    MessageBox.Show("Gerekli yerleri doldurun!");
                    return;
                }
                
                try
                {
                    var course = new Course
                    {
                        Name = CourseName,
                        Code = CourseCode,
                        Description = CourseDescription,
                        Credit = CourseCredit,
                        Quota = CourseQuota,
                        IsElective = IsSelectedDisplay == "EVET",
                        LecturerId = _mainVM.Globals.LoggedLecturer.Id,
                        SemesterNumber = CourseSemesterNumber,
                        DepartmentId = _mainVM.Globals.LoggedLecturer.Departments.First().Id
                    };
                    _mainVM.Globals.CourseRepository.AddCourse(course);
                    MessageBox.Show("Ders başarıyla eklendi!");
                    _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }
            });
        }
    }
}