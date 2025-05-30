using System.Windows;
using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using gp_unisis.Database.Entities;


namespace gp_unisis.ViewModel.Admin
{
    class FakülteDüzenlemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AdminAnaSayfaCommand { get; set; }
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
        public ICommand FakülteListelemeCommand { get; set; }
        public ICommand ÖğrenciİşleriDüzenlemeCommand { get; set; }
        public ICommand ÖğrenciİşleriListelemeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }


        public Database.Entities.Faculty Faculty {  get; set; }

        public ObservableCollection<Database.Entities.Faculty> Faculties { get; set; }


        public ICommand FacultyGuncelleCommand { get; set; }

        public FakülteDüzenlemeViewModel(MainWindowViewModel mainMV, int facultyId)
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

            LoadFaculties();
            LoadFaculty(facultyId);

            FacultyGuncelleCommand = new RelayCommand(param =>
            {
                if(Faculty!=null)
                {
                    try
                    {
                        _mainMV.Globals.FacultyRepository.UpdateFaculty(Faculty);
                        MessageBox.Show("Fakülte güncellendi");
                        _mainMV.CurrentViewModel = new FakülteListelemeViewModel(mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });
        }

        public void LoadFaculty(int facultyId)
        {
            Faculty=_mainMV.Globals.FacultyRepository.GetFacultyById(facultyId);
        }

        private void LoadFaculties()
        {
            var facultyList = _mainMV.Globals.FacultyRepository.GetAllFaculties();
            Faculties = new ObservableCollection<Faculty>(facultyList.ToList());
        }
    }
}