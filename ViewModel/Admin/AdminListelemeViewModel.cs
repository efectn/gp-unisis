using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using gp_unisis.Database.Entities;
using System.Windows;
using gp_unisis.Helpers;


namespace gp_unisis.ViewModel.Admin
{
    class AdminListelemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;

        public ICommand AdminAnaSayfaCommand { get; set; }
        public ICommand AdminDüzenlemeCommand { get; set; }
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

        public ObservableCollection<Database.Entities.Admin> Admins { get; set; }

        public ICommand AdminDuzenleCommand { get; set; }
        public ICommand AdminSilCommand { get; set; }
        public ICommand AdminEkleCommand { get; set; }

        public string YeniAdminAdSoyad { get; set; }

        public string YeniAdminEmail { get; set; }

        public string YeniAdminPassword { get; set; }


        public AdminListelemeViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
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

            LoadAdmins();

            AdminDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Admin admin)
                {
                    _mainMV.CurrentViewModel = new AdminDüzenlemeViewModel(_mainMV, admin.Id);
                }
            });
            AdminSilCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Admin admin)
                {
                    try
                    {
                        _mainMV.Globals.AdminRepository.DeleteAdmin(admin);
                        MessageBox.Show("Admin silindi!");
                        _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                        throw;
                    }
                }
            });

            AdminEkleCommand = new RelayCommand(param =>
            {
                // Check if yeni admin details are provided
                if (!string.IsNullOrWhiteSpace(YeniAdminAdSoyad) &&
                    !string.IsNullOrWhiteSpace(YeniAdminEmail) &&
                    !string.IsNullOrWhiteSpace(YeniAdminPassword))
                {
                    // Create a new admin entity
                    var newAdmin = new Database.Entities.Admin
                    {
                        Name = YeniAdminAdSoyad,
                        Email = YeniAdminEmail,
                        Password = MD5Helper.CreateMD5(YeniAdminPassword),
                    };

                    try
                    {
                        // Add the new admin to the repository
                        _mainMV.Globals.AdminRepository.AddAdmin(newAdmin);
                        MessageBox.Show("Admin eklendi!");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                    
                    // Clear the input fields
                    YeniAdminAdSoyad = string.Empty;
                    YeniAdminEmail = string.Empty;
                    YeniAdminPassword = string.Empty;
                    // Refresh page
                    _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV);
                }
                else
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.");
                }
            });

        }

        private void LoadAdmins()
        {
            var adminList = _mainMV.Globals.AdminRepository.GetAllAdminsName();
            Admins = new ObservableCollection<Database.Entities.Admin>(adminList.ToList());
        }
    }
}