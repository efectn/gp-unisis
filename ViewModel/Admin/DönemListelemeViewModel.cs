using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using gp_unisis.Database.Entities;
using System.Windows;


namespace gp_unisis.ViewModel.Admin
{
    class DönemListelemeViewModel : ViewModelBase
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
        public ICommand FakülteDüzenlemeCommand { get; set; }
        public ICommand FakülteListelemeCommand { get; set; }
        public ICommand ÖğrenciİşleriDüzenlemeCommand { get; set; }
        public ICommand ÖğrenciİşleriListelemeCommand { get; set; }
        public ICommand LogOutCommand { get; set; }


        public ICommand DonemDuzenleCommand { get; set; }
        public ICommand DonemSilCommand { get; set; }
        public ICommand DonemEkleCommand { get; set; }


        public string YeniName { get; set; }
        public DateTime YeniDonemBaslangıc { get; set; }

        public DateTime YeniDonemBitis { get; set; }

        public DateTime YeniSinavSecimBaslangic { get; set; }

        public DateTime YeniSinavSecimBitis { get; set; }

        public DateTime YeniFinalSinavZamani { get; set; }

        public ObservableCollection<Database.Entities.Semester> Semesters { get; set; }

        public DönemListelemeViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            AkademisyenListeleCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV));
            BölümListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV));
            DersGrubuListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV));
            FakülteListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV));
            ÖğrenciİşleriListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LoadSemesters();

            DonemDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Semester semester)
                {
                    _mainMV.CurrentViewModel = new DönemDüzenlemeViewModel(_mainMV, semester.Id);
                }
            });

            DonemSilCommand = new RelayCommand(param =>
            {
                if (param is Semester semester)
                {
                    try
                    {
                        _mainMV.Globals.SemesterRepository.DeleteSemester(semester.Id);
                        MessageBox.Show("Dönem başarıyla silindi!");
                        LoadSemesters();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

            DonemEkleCommand = new RelayCommand(param =>
            {

                if (string.IsNullOrWhiteSpace(YeniName) ||
                   YeniDonemBaslangıc == default ||
                    YeniDonemBitis == default||
                    //YeniSinavSecimBaslangic == default ||
                    //YeniSinavSecimBitis == default||
                    YeniFinalSinavZamani == default)
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                var newSemester = new Semester
                {
                    Name = YeniName,
                    StartDate = YeniDonemBaslangıc,
                    EndDate = YeniDonemBitis,
                    //CourseRegistrationStartDate = YeniSinavSecimBaslangic,
                    //CourseRegistrationEndDate = YeniSinavSecimBitis,
                    FinalExamDate = YeniFinalSinavZamani
                };

                try
                {
                    _mainMV.Globals.SemesterRepository.AddSemester(newSemester);
                    MessageBox.Show("Dönem başarıyla eklendi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }

                YeniName = string.Empty;
                YeniDonemBaslangıc = default;
                YeniDonemBitis = default;
                YeniFinalSinavZamani = default;
                //YeniSinavSecimBitis = default;
                //YeniSinavSecimBaslangic = default;

                _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV);
            });
        }

        private void LoadSemesters()
        {
            var semestersList = _mainMV.Globals.SemesterRepository.GetAllSemesters();
            Semesters = new ObservableCollection<Semester>(semestersList.ToList());
        }
    }
}