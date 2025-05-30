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
using gp_unisis.ViewModel.Student;

namespace gp_unisis.ViewModel.Lecturer
{
    class DersListesiViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;

        public ICommand AkademisyenAnaSayfaCommand { get; set; }
        public ICommand DersDuzenleCommand { get; set; }
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
        public ICommand DersProgramiSilCommand { get; set; }
        public ICommand DersProgramiEkleCommand { get; set; }
        public ICommand DonemleriListeleCommand { get; set; }
        public ICommand AktifDonemeEkleCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

        public DersListesiViewModel(MainWindowViewModel mainVM)
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

            var user = _mainVM.Globals?.LoggedLecturer;
            var semesterId = _mainVM.Globals?.ActiveSemesterId;
            var semester = _mainVM.Globals?.ActiveSemester;
            
            if (user == null || semesterId == null || semester == null)
            {
                return;
            }


            var courses = user.Courses.ToList();
            if (courses != null)
            {
                Courses = new ObservableCollection<Course>(courses);
            }
            
            DonemleriListeleCommand = new RelayCommand(param =>
            {
                if (param is Course course)
                {
                    var semesters = string.Join(",", course.Semesters.Select(s => s.Name));
                    MessageBox.Show($"Aktif Dönemler\n{semesters}");
                }
            });
            
            AktifDonemeEkleCommand = new RelayCommand(param =>
            {
                if (param is Course course)
                {
                    if (course.Semesters.Any(s => s.Id == semesterId))
                    {
                        MessageBox.Show("Ders zaten aktif dönemde açılmış!");
                        return;
                    }
                    
                    course.Semesters.Add(semester);
                    try
                    {
                        _mainVM.Globals.CourseRepository.UpdateCourse(course);
                        MessageBox.Show("Ders aktif döneme eklendi!");
                        var courses = user.Courses.ToList();
                        if (courses != null)
                        {
                            Courses = new ObservableCollection<Course>(courses);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

            DersDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Course course)
                {
                    _mainVM.CurrentViewModel = new DersDuzenleSilViewModel(_mainVM, course.Id);
                }
            });
        }
    }
}