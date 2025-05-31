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
    class DersProgramiViewModel : ViewModelBase
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
        public ICommand DersProgramiSilCommand { get; set; }
        public ICommand DersProgramiEkleCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Ders3> Courses { get; set; } = new ObservableCollection<Ders3>();

        public DersProgramiViewModel(MainWindowViewModel mainVM)
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

            var dayOrder = new List<string>
            {
                "Pazartesi",
                "Salı",
                "Çarşamba",
                "Perşembe",
                "Cuma",
                "Cumartesi",
                "Pazar"
            };
            
            var user = _mainVM.Globals?.LoggedLecturer;
            var semesterId = _mainVM.Globals?.ActiveSemesterId;
            var semester = _mainVM.Globals?.ActiveSemester;
            
            if (user == null || semesterId == null || semester == null)
            {
                return;
            }


            var courses = _mainVM.Globals.CourseRepository.GetCoursesByLecturerId(user.Id)
                .Where(c => c.Semesters != null && c.Semesters.Any(s => s.Id == semesterId)).ToList();
            if (courses != null)
            {
                var schedules = courses.SelectMany(s => s.CourseScheduleEntries).OrderBy(e => dayOrder.IndexOf(e.Day)).ToList();
                foreach (var schedule in schedules)
                {
                    var ders = new Ders3
                    {
                        Id = schedule.Id,
                        Day = schedule.Day,
                        Name = schedule.Course.Name,
                        Time = schedule.StartTime + " - " + schedule.EndTime,
                        Lecturer = schedule.Course.Lecturer.FullName,
                    };
                    Courses.Add(ders);
                }
            }
            
            DersProgramiSilCommand = new RelayCommand(param =>
            {
                if (param is Ders3 entry)
                {
                    try
                    {
                        _mainVM.Globals.CourseScheduleRepository.DeleteScheduleEntry(entry.Id);
                        MessageBox.Show("Ders programı girdisi başarıyla silindi!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                    // Refresh page
                    _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM);
                }
            });

            DersProgramiEkleCommand = new RelayCommand(_ =>
            {
                _mainVM.CurrentViewModel = new DersProgramiEkleViewModel(_mainVM);
            });

        }
    }
}