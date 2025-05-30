using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using gp_unisis.Database.Entities;
using System.Collections.ObjectModel;
using System.Windows;


namespace gp_unisis.ViewModel.Admin
{
    class BölümListelemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AdminAnaSayfaCommand { get; set; }
        public ICommand AdminDüzenlemeCommand { get; set; }
        public ICommand AdminListelemeCommand { get; set; }
        public ICommand AkademsyenDüzenlemeCommand { get; set; }
        public ICommand AkademisyenListeleCommand { get; set; }
        public ICommand BölümDüzenlemeCommand { get; set; }
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


        public ICommand BolumDuzenleCommand { get; set; }
        public ICommand BolumSilCommand { get; set; }
        public ICommand BolumEkleCommand { get; set; }

        public ObservableCollection<string> Faculty { get; set; }

        public string YeniBolumAdı { get; set; }

        public string YeniNumara{ get; set; }

        public string YeniAdres { get; set; }

        public string YeniBaskan { get; set; }

        public string YeniBaskanYardımcısı{ get; set; }

        public string YeniFakulte { get; set; }

        public ObservableCollection<Database.Entities.Faculty> FacultyNames { get; set; }

        public ObservableCollection<Database.Entities.Department> Departments { get; set; }



        public BölümListelemeViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            AkademisyenListeleCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV));
            DersGrubuListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV));
            DönemListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV));
            FakülteListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV));
            ÖğrenciİşleriListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LoadDepartments();
            LoadFaculyNames();

            BolumDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Department department)
                {
                    _mainMV.CurrentViewModel = new BölümDüzenlemeViewModel(_mainMV,department.Id);
                }
            });

            BolumSilCommand = new RelayCommand(param =>
            {
                if (param is Department department)
                {
                    try
                    {
                        _mainMV.Globals.DepartmentRepository.DeleteDepartment(department.Id);
                        MessageBox.Show("Bölüm başarıyla silindi!");
                        _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV);
                    }
                    catch ( Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });

            BolumEkleCommand = new RelayCommand(param =>
            {

                if (string.IsNullOrWhiteSpace(YeniBolumAdı) ||
                    string.IsNullOrWhiteSpace(YeniAdres) ||
                    string.IsNullOrWhiteSpace(YeniBaskan) ||
                    string.IsNullOrWhiteSpace(YeniBaskanYardımcısı) ||
                    string.IsNullOrWhiteSpace(YeniNumara) ||
                    string.IsNullOrWhiteSpace(YeniFakulte))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedFaculty = _mainMV.Globals.FacultyRepository
                    .GetAllFaculties()
                    .FirstOrDefault(f => f.Name == YeniFakulte);

                if (selectedFaculty == null)
                {
                    MessageBox.Show("Seçilen fakülte sistemde bulunamadı!");
                    return;
                }

                var newDepartment = new Department
                {
                    Name = YeniBolumAdı,
                    Address = YeniAdres,
                    Head = YeniBaskan,
                    ViceHead = YeniBaskanYardımcısı,
                    ContactNumber = YeniNumara,
                    Faculty = selectedFaculty
                };

                try
                {
                    _mainMV.Globals.DepartmentRepository.AddDepartment(newDepartment);
                    MessageBox.Show("Bölüm başarıyla eklendi!");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Hata: {e.Message}");
                }

                YeniBolumAdı = string.Empty;
                YeniAdres = string.Empty;
                YeniBaskan = string.Empty;
                YeniBaskanYardımcısı = string.Empty;
                YeniNumara = string.Empty;
                YeniFakulte = string.Empty;

                _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV);
            });
        }

        private void LoadDepartments()
        {
            var departmentList = _mainMV.Globals.DepartmentRepository.GetAllDepartments();
            Departments = new ObservableCollection<Department>(departmentList.ToList());
        }

        private void LoadFaculyNames()

        {
            var facultyList = _mainMV.Globals.FacultyRepository.GetAllFaculties();
            FacultyNames = new ObservableCollection<Faculty>(facultyList.ToList());
        }
    }
}