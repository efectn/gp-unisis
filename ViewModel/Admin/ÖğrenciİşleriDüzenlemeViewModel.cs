using gp_unisis.Commands;
using gp_unisis.Database.Entities;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Helpers;


namespace gp_unisis.ViewModel.Admin
{
    class ÖğrenciİşleriDüzenlemeViewModel : ViewModelBase
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
        public ICommand FakülteDüzenlemeCommand { get; set; }
        public ICommand FakülteListelemeCommand { get; set; }
        public ICommand ÖğrenciİşleriListelemeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }


        public Database.Entities.StudentPersonal StudentPersonal { get; set; }

        public ICommand StudentPersonalGuncelleCommand { get; set; }
        public string NewPassword { get; set; }

        public ÖğrenciİşleriDüzenlemeViewModel(MainWindowViewModel mainMV, int id )
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

            LoadStudentPersonal(id);

            StudentPersonalGuncelleCommand = new RelayCommand(param =>
            {
                if(StudentPersonal != null)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(NewPassword))
                        {
                            StudentPersonal.Password = MD5Helper.CreateMD5(NewPassword);
                        }
                        _mainMV.Globals.StudentPersonalRepository.UpdateStudentPersonal(StudentPersonal);
                        MessageBox.Show("Öğrenci işleri personeli başarıyla güncellendi!");
                        _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                        throw;
                    }
                }
            });
        }
        public void LoadStudentPersonal(int id)
        {
            StudentPersonal = _mainMV.Globals.StudentPersonalRepository.GetStudentPersonalById(id);
        }
    }
}