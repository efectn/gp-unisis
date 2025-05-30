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
    class DersProgramiEkleViewModel : ViewModelBase
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
        public ICommand SaveCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; } 

        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();
        public ObservableCollection<string> Days { get; set; } = new ObservableCollection<string>
        {
            "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"
        };

        public int SelectedCourseId { get; set; }
        public string SelectedDay { get; set; }
        public string StartTime { get; set; }  // Format: "HH:mm"
        public string EndTime { get; set; }


        public DersProgramiEkleViewModel(MainWindowViewModel mainVM)
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


            var courses = user.Courses
                .Where(c => c.Semesters.Any(s => s.Id == semesterId)).ToList();
            if (courses != null)
            {
                Courses = new ObservableCollection<Course>(courses);
            }
            
            SaveCommand = new RelayCommand(param =>
            {
                if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime) ||
                    string.IsNullOrEmpty(SelectedDay))
                {
                    MessageBox.Show("Tüm alanları doldurun!");
                }
                else
                {
                    var startTime = TimeSpan.Parse(StartTime);
                    var endTime = TimeSpan.Parse(EndTime);
                    
                    if (startTime >= endTime)
                    {
                        MessageBox.Show("Bitiş saati başlangıç saatinden önce olamaz.");
                        return;
                    }
                    var entry = new CourseScheduleEntry
                    {
                        CourseId = SelectedCourseId,
                        SemesterId = semesterId.Value,
                        StartTime = startTime,
                        EndTime = endTime,
                        Day = SelectedDay,
                    };

                    var course = _mainVM.Globals.CourseRepository.GetCourseById(SelectedCourseId);
                    if (course == null)
                    {
                        MessageBox.Show("Ders bulunamadı!");
                        return;
                    }
                    
                    // Check for overlapping schedule entries
                    foreach (var oldEntry in course.CourseScheduleEntries)
                    {
                        if (oldEntry.Day == SelectedDay && 
                            ((startTime >= entry.StartTime && startTime < oldEntry.EndTime) || 
                             (endTime > entry.StartTime && endTime <= oldEntry.EndTime)))
                        {
                            MessageBox.Show("Bu zaman diliminde zaten bir ders programı girişi bulunmaktadır. Lütfen kontrol edin");
                            return;
                        }
                    }

                    try
                    {
                        _mainVM.Globals.CourseScheduleRepository.AddScheduleEntry(entry);
                        MessageBox.Show("Ders programı girdisi başarıyla eklendi!");
                        _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

        }
    }
}