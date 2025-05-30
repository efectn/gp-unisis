using System.Windows;
using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Helpers;


namespace gp_unisis.ViewModel.Admin
{
    class AdminDüzenlemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;

        public ICommand AdminAnaSayfaCommand { get; set; }
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

        public Database.Entities.Admin Admin { get; set; }

        public ICommand AdminGuncelleCommand { get; set; }
        public string NewPassword { get; set; }

        public AdminDüzenlemeViewModel(MainWindowViewModel mainMV, int adminId)
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

            // Load the admin details based on the provided adminId
            LoadAdmin(adminId);

            AdminGuncelleCommand = new RelayCommand(param =>
            {
                if (Admin != null)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(NewPassword))
                        {
                            Admin.Password = MD5Helper.CreateMD5(NewPassword);
                        }
                        
                        _mainMV.Globals.AdminRepository.UpdateAdmin(Admin);
                        MessageBox.Show("Admin güncellendi!");
                        _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });
        }

        public void LoadAdmin(int adminId)
        {
            // Assuming you have a method to get the admin by ID
            Admin = _mainMV.Globals.AdminRepository.GetAdminById(adminId);
        }
    }
}