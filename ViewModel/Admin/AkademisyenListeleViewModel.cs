using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using System.Windows;
using gp_unisis.Database.Entities;


namespace gp_unisis.ViewModel.Admin
{
    class AkademisyenListeleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AdminAnaSayfaCommand { get; set; }
        public ICommand AdminDüzenlemeCommand { get; set; }
        public ICommand AdminListelemeCommand { get; set; }
        public ICommand AkademsyenDüzenlemeCommand { get; set; }
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

        public ObservableCollection<Database.Entities.Lecturer> Lecturers { get; set; }

        public ObservableCollection<Department> Departments { get; set; }

        public ICommand AkademisyenDuzenleCommand { get; set; }
        public ICommand AkademisyenSilCommand { get; set; }
        public ICommand AkademisyenEkleCommand { get; set; }

        public string YeniAkademisyenAdSoyad { get; set; }

        public string YeniAkademisyenEmail { get; set; }

        public string YeniAkademisyenPassword { get; set; }

        public string YeniAkademisyenDepartmanIdleri { get; set; }


        public AkademisyenListeleViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand =
                new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand =
                new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            BölümDüzenlemeCommand =
            BölümListelemeCommand =
                new RelayCommand(param => _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV));
            DersGrubuListelemeCommand =
                new RelayCommand(param => _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV));
            DönemListelemeCommand =
                new RelayCommand(param => _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV));
            FakülteListelemeCommand =
                new RelayCommand(param => _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV));
            ÖğrenciİşleriListelemeCommand = new RelayCommand(param =>
                _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LoadDepartments();
            LoadLecturers();

            AkademisyenDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Lecturer Lecturer)
                {
                    _mainMV.CurrentViewModel = new AkademisyenDuzenlemeViewModel(_mainMV,Lecturer.Id);
                }
            });

            AkademisyenSilCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Lecturer Lecturer)
                {
                    try
                    {
                        _mainMV.Globals.LecturerRepository.DeleteLecturer(Lecturer.Id);
                        MessageBox.Show("Akademisyen başarıyla silindi!");
                        _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });

            AkademisyenEkleCommand = new RelayCommand(param =>
            {
                if (!string.IsNullOrWhiteSpace(YeniAkademisyenAdSoyad) &&
                    !string.IsNullOrWhiteSpace(YeniAkademisyenEmail) &&
                    !string.IsNullOrWhiteSpace(YeniAkademisyenPassword) &&
                    !string.IsNullOrWhiteSpace(YeniAkademisyenDepartmanIdleri))
                {
                    var departmanIdListesi = YeniAkademisyenDepartmanIdleri
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(idStr =>
                        {
                            bool success = int.TryParse(idStr.Trim(), out int id);
                            return new { success, id };
                        })
                        .ToList();

                    var departments = new List<Department>();

                    foreach (var item in departmanIdListesi)
                    {
                        var department = _mainMV.Globals.DepartmentRepository.GetDepartmentById(item.id);
                        if (department == null)
                        {
                            MessageBox.Show($"Departman bulunamadı: ID = {item.id}");
                            return;
                        }

                        departments.Add(department);
                    }

                    var newLecturer = new Database.Entities.Lecturer
                    {
                        FullName = YeniAkademisyenAdSoyad,
                        Email = YeniAkademisyenEmail,
                        Password = YeniAkademisyenPassword,
                        Departments = departments,
                    };

                    try
                    {
                        _mainMV.Globals.LecturerRepository.AddLecturer(newLecturer);
                        MessageBox.Show("Akademisyem başarıyla eklendi!");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }

                    YeniAkademisyenAdSoyad = string.Empty;
                    YeniAkademisyenEmail = string.Empty;
                    YeniAkademisyenPassword = string.Empty;
                    YeniAkademisyenDepartmanIdleri = string.Empty;

                    _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV);
                }
            });
        }

        private void LoadLecturers()
        {
            var LecturerList = _mainMV.Globals.LecturerRepository.GetAllLecturers();
            Lecturers = new ObservableCollection<Database.Entities.Lecturer>(LecturerList.ToList());
        }

        private void LoadDepartments()
        {
            var departmentList = _mainMV.Globals.DepartmentRepository.GetAllDepartments();
            Departments = new ObservableCollection<Department>(departmentList.ToList());
        }
    }
}