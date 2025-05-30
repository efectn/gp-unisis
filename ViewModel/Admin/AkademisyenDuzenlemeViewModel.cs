using System.Windows;
using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Helpers;
using gp_unisis.Views.Admin;


namespace gp_unisis.ViewModel.Admin
{
    class AkademisyenDuzenlemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AdminAnaSayfaCommand { get; set; }
        public ICommand AdminDüzenlemeCommand { get; set; }
        public ICommand AdminListelemeCommand { get; set; }
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

        public Database.Entities.Lecturer Lecturer { get; set; }
        public ICommand AkademisyenGuncelleCommand { get; set; }
        public string NewPassword { get; set; }

        public ICommand LogOutCommand { get; set; }

        public AkademisyenDuzenlemeViewModel(MainWindowViewModel mainMV,int lecturerId)
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


            LoadLecturer(lecturerId);

            AkademisyenGuncelleCommand = new RelayCommand(param =>
            {
                if (Lecturer != null)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(NewPassword))
                        {
                            Lecturer.Password = MD5Helper.CreateMD5(NewPassword);
                        }
                        _mainMV.Globals.LecturerRepository.UpdateLecturer(Lecturer);
                        MessageBox.Show("Akademisyen güncellendi!");
                        _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });
        }
        public void LoadLecturer(int lecturerId)
        {
            Lecturer = _mainMV.Globals.LecturerRepository.GetLecturerById(lecturerId);
        }
    }
}