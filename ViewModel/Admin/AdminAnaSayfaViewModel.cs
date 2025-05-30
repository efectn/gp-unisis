using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;


namespace gp_unisis.ViewModel.Admin
{
    class AdminAnaSayfaViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AnasayfaCommand { get; set; }
        public ICommand AdminDüzenlemeCommand { get; set; }
        public ICommand AdminListelemeCommand { get; set; }
        public ICommand AkademsyenDüzenlemeCommand { get; set; }
        public ICommand AkademisyenListeleCommand { get; set; }
        public ICommand BölümDüzenlemeCommand { get; set; }
        public ICommand BölümListelemeCommand { get; set; }
        public ICommand DersGrubuDüzenlemeCommand { get; set; }
        public ICommand DersGrubuListelemeCommand { get; set; }
        public ICommand DuyuruEklemeCommand { get; set; }
        public ICommand DönemDüzenlemeCommand { get; set; }
        public ICommand DönemListelemeCommand { get; set; }
        public ICommand FakülteDüzenlemeCommand { get; set; }
        public ICommand FakülteListelemeCommand { get; set; }
        public ICommand ÖğrenciİşleriDüzenlemeCommand { get; set; }
        public ICommand ÖğrenciİşleriListelemeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public int LecturerCount {get;set;}
        public int FacultyCount {get;set;}
        public int DepartmentCount {get;set;}
        public int StudentCount {get;set;}

        public AdminAnaSayfaViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AnasayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            AkademisyenListeleCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV));
            BölümListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV));
            DersGrubuListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV));
            DönemListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV));
            FakülteListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV));
            ÖğrenciİşleriListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LecturerCount = _mainMV.Globals.LecturerRepository.GetAllLecturers().Count;
            FacultyCount = _mainMV.Globals.FacultyRepository.GetAllFaculties().Count;
            DepartmentCount = _mainMV.Globals.DepartmentRepository.GetAllDepartments().Count;
            StudentCount = _mainMV.Globals.StudentRepository.GetAllStudents().Count;
        }
    }
}