using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using gp_unisis.Database.Entities;
using System.Windows;
using gp_unisis.Helpers;


namespace gp_unisis.ViewModel.Admin
{
    class ÖğrenciİşleriListelemeViewModel : ViewModelBase
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
        public ICommand ÖğrenciİşleriDüzenlemeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Database.Entities.StudentPersonal> StudentPersonals { get; set; }

        public ICommand OgrenciIsleriDuzenleCommand { get; set; }
        public ICommand OgrenciIsleriSilCommand { get; set; }
        public ICommand OgrenciIsleriEkleCommand { get; set; }

        public string YeniOgrenciIsleriAdSoyad { get; set; }

        public string YeniOgrenciIsleriEmail { get; set; }

        public string YeniOgrenciIsleriSifre { get; set; }

        public ÖğrenciİşleriListelemeViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            AkademisyenListeleCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV));
            BölümListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV));
            DersGrubuListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV));
            DönemListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV));
            FakülteListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LoadStudentPersonal();

            OgrenciIsleriDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.StudentPersonal studentPersonal)
                {
                    _mainMV.CurrentViewModel = new ÖğrenciİşleriDüzenlemeViewModel(_mainMV, studentPersonal.Id);
                }
            });

            OgrenciIsleriSilCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.StudentPersonal studentPersonal)
                {
                    try
                    {
                        _mainMV.Globals.StudentPersonalRepository.DeleteStudentPersonal(studentPersonal);
                        MessageBox.Show("Öğrenci işleri personeli başarıyla silindi!");
                        _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });

            OgrenciIsleriEkleCommand = new RelayCommand(param =>
            {
                if (!string.IsNullOrWhiteSpace(YeniOgrenciIsleriAdSoyad) &&
                    !string.IsNullOrWhiteSpace(YeniOgrenciIsleriEmail)&&
                    !string.IsNullOrWhiteSpace(YeniOgrenciIsleriSifre))
                {
                    

                    var newStudentPersonal = new Database.Entities.StudentPersonal()
                    {
                        Name = YeniOgrenciIsleriAdSoyad,
                        Email = YeniOgrenciIsleriEmail,
                        Password = MD5Helper.CreateMD5(YeniOgrenciIsleriSifre),
                    };

                    try
                    {
                        _mainMV.Globals.StudentPersonalRepository.AddStudentPersonal(newStudentPersonal);
                        MessageBox.Show("Personel başarıyla eklendi!");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }

                    YeniOgrenciIsleriAdSoyad = string.Empty;
                    YeniOgrenciIsleriEmail = string.Empty;
                    YeniOgrenciIsleriSifre = string.Empty;

                    _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV);
                }
            });
        }

        private void LoadStudentPersonal()
        {
            var StudentPersonalList = _mainMV.Globals.StudentPersonalRepository.GetAllStudentPersonalsName();
            StudentPersonals = new ObservableCollection<Database.Entities.StudentPersonal>(StudentPersonalList.ToList());
        }
    }
}