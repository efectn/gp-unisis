using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using gp_unisis.Database.Entities;
using System.Windows;


namespace gp_unisis.ViewModel.Admin
{
    class FakülteListelemeViewModel : ViewModelBase
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
        public ICommand ÖğrenciİşleriDüzenlemeCommand { get; set; }
        public ICommand ÖğrenciİşleriListelemeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }


        public ICommand FakulteDuzenleCommand { get; set; }
        public ICommand FakulteSilCommand { get; set; }
        public ICommand FakulteEkleCommand { get; set; }

        public string YeniFakulteAdı { get; set; }

        public string YeniNumara { get; set; }

        public string YeniAdres { get; set; }

        public string YeniBaskan { get; set; }

        public string YeniBaskanYardımcısı { get; set; }

        public ObservableCollection<Database.Entities.Faculty> Faculties { get; set; }


        public FakülteListelemeViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            AkademisyenListeleCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV));
            BölümListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV));
            DersGrubuListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV));
            DönemListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV));
            ÖğrenciİşleriListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LoadFaculties();

            FakulteDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Faculty faculty)
                {
                    _mainMV.CurrentViewModel = new FakülteDüzenlemeViewModel(_mainMV, faculty.Id);
                }
            });

            FakulteSilCommand = new RelayCommand(param =>
            {
                if (param is Faculty faculty)
                {
                    try
                    {
                        _mainMV.Globals.FacultyRepository.DeleteFaculty(faculty.Id);
                        MessageBox.Show("Fakülte başarıyla silindi!");
                        _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                        throw;
                    }
                }
            });

            FakulteEkleCommand = new RelayCommand(param =>
            {
                if (string.IsNullOrWhiteSpace(YeniFakulteAdı) ||
                    string.IsNullOrWhiteSpace(YeniAdres) ||
                    string.IsNullOrWhiteSpace(YeniBaskan) ||
                    string.IsNullOrWhiteSpace(YeniBaskanYardımcısı) ||
                    string.IsNullOrWhiteSpace(YeniNumara))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var newFaculty = new Faculty
                {
                    Name = YeniFakulteAdı,
                    Address = YeniAdres,
                    Dean = YeniBaskan,
                    ViceDean = YeniBaskanYardımcısı,
                    ContactNumber = YeniNumara,
                };

                try
                {
                    _mainMV.Globals.FacultyRepository.AddFaculty(newFaculty);
                    MessageBox.Show("Fakülte başarıyla eklendi");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Hata: {e.Message}");
                }

                YeniFakulteAdı = string.Empty;
                YeniAdres = string.Empty;
                YeniBaskan = string.Empty;
                YeniBaskanYardımcısı = string.Empty;
                YeniNumara = string.Empty;

                _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV);
            });
        }
        private void LoadFaculties()
        {
            var facultyList = _mainMV.Globals.FacultyRepository.GetAllFaculties();
            Faculties = new ObservableCollection<Faculty>(facultyList.ToList());
        }
    }
}