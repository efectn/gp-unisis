using System.Windows;
using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using gp_unisis.Database.Entities;
using System.Collections.ObjectModel;


namespace gp_unisis.ViewModel.Admin
{
    class BölümDüzenlemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AdminAnaSayfaCommand { get; set; }
        public ICommand AdminDüzenlemeCommand { get; set; }
        public ICommand AdminListelemeCommand { get; set; }
        public ICommand AkademsyenDüzenlemeCommand { get; set; }
        public ICommand AkademisyenListeleCommand { get; set; }
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

        public ObservableCollection<Database.Entities.Faculty> FacultyNames { get; set; }

        public Database.Entities.Department Department { get; set; }
        public ICommand BolumGuncelleCommand { get; set; }

        public BölümDüzenlemeViewModel(MainWindowViewModel mainMV,int departmentId)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
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
            LoadFaculyNames();
            LoadDepartment(departmentId);

            BolumGuncelleCommand = new RelayCommand(param =>
            {
                if(Department!=null)
                {
                    try
                    {
                        _mainMV.Globals.DepartmentRepository.UpdateDepartment(Department);
                        MessageBox.Show("Bölüm güncellendi!");
                        _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });
        }
        public void LoadDepartment(int departmentId)
        {
            Department=_mainMV.Globals.DepartmentRepository.GetDepartmentById(departmentId);
        }

        private void LoadFaculyNames()

        {
            var facultyList = _mainMV.Globals.FacultyRepository.GetAllFaculties();
            FacultyNames = new ObservableCollection<Faculty>(facultyList.ToList());
        }
    }
}