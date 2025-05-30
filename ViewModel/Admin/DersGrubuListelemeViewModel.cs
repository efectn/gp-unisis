using gp_unisis.Commands;
using System.Windows.Input;
using gp_unisis.Views.Admin;
using System.Collections.ObjectModel;
using gp_unisis.Database.Entities;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;

namespace gp_unisis.ViewModel.Admin
{
    class DersGrubuListelemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainMV;
        public ICommand AdminAnaSayfaCommand { get; set; }
        public ICommand AdminListelemeCommand { get; set; }
        public ICommand AkademisyenListeleCommand { get; set; }
        public ICommand BölümListelemeCommand { get; set; }
        public ICommand DuyuruEklemeCommand { get; set; }
        public ICommand DönemListelemeCommand { get; set; }
        public ICommand FakülteListelemeCommand { get; set; }
        public ICommand ÖğrenciİşleriListelemeCommand { get; set; }

        public ICommand LogOutCommand { get; set; }


        public ICommand DersGrubuDuzenleCommand { get; set; }
        public ICommand DersGrubuSilCommand { get; set; }
        public ICommand DersGrubuEkleCommand { get; set; }

        public string YeniDersGrubuAdı { get; set; }
        public string YeniDersGrubuİstenenKredisi { get; set; }
        public string YeniDersGrubuİstenenDers { get; set; }
        public string YeniDersGrubuDersEkle { get; set; }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (_selectedDepartment != value)
                {
                    _selectedDepartment = value;
                    OnPropertyChanged(nameof(SelectedDepartment));
                    LoadCoursesForSelectedDepartment();
                }
            }
        }

        public Semester SelectedSemester { get; set; }

        public ObservableCollection<CourseGroup> CourseGroups { get; set; }
        public ObservableCollection<Semester> Semesters { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Course> FilteredCourses { get; set; }

        public DersGrubuListelemeViewModel(MainWindowViewModel mainMV)
        {
            _mainMV = mainMV;

            // Navigation commands
            AdminAnaSayfaCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminAnaSayfaViewModel(_mainMV));
            AdminListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AdminListelemeViewModel(_mainMV));
            AkademisyenListeleCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new AkademisyenListeleViewModel(_mainMV));
            BölümListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new BölümListelemeViewModel(_mainMV));
            DönemListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new DönemListelemeViewModel(_mainMV));
            FakülteListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new FakülteListelemeViewModel(_mainMV));
            ÖğrenciİşleriListelemeCommand = new RelayCommand(param => _mainMV.CurrentViewModel = new ÖğrenciİşleriListelemeViewModel(_mainMV));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainMV.CurrentViewModel = new LoginViewModel(_mainMV);
            });

            LoadDepartments();
            LoadSemesters();
            LoadCourseGroups();

            DersGrubuSilCommand = new RelayCommand(param =>
            {
                if (param is CourseGroup courseGroup)
                {
                    try
                    {
                        _mainMV.Globals.CourseGroupRepository.DeleteCourseGroup(courseGroup.Id);
                        MessageBox.Show("Ders grubu başarıyla silindi!");
                        _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                }
            });

            DersGrubuEkleCommand = new RelayCommand(param =>
            {
                if (string.IsNullOrWhiteSpace(YeniDersGrubuAdı) ||
                    string.IsNullOrWhiteSpace(YeniDersGrubuİstenenDers) ||
                    string.IsNullOrWhiteSpace(YeniDersGrubuİstenenKredisi) ||
                    string.IsNullOrWhiteSpace(YeniDersGrubuDersEkle) ||
                    SelectedDepartment == null ||
                    SelectedSemester == null)
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dersIdListesi = YeniDersGrubuDersEkle
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(idStr =>
                    {
                        bool success = int.TryParse(idStr.Trim(), out int id);
                        return new { success, id };
                    })
                    .ToList();

                var courses = new List<Course>();

                foreach (var item in dersIdListesi)
                {
                    if (!item.success)
                    {
                        MessageBox.Show($"Geçersiz ders ID formatı: '{item.id}'");
                        return;
                    }

                    var course = _mainMV.Globals.CourseRepository.GetCourseById(item.id);
                    if (course == null)
                    {
                        MessageBox.Show($"Ders bulunamadı: ID = {item.id}");
                        return;
                    }

                    courses.Add(course);
                }

                if (!int.TryParse(YeniDersGrubuİstenenDers, out int dersSayısı) ||
                    !int.TryParse(YeniDersGrubuİstenenKredisi, out int kredi))
                {
                    MessageBox.Show("Kredi ve ders sayısı sayısal olmalıdır!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newCourseGroup = new CourseGroup
                {
                    Name = YeniDersGrubuAdı,
                    RequiredCoursesCount = dersSayısı,
                    RequiredCredits = kredi,
                    DepartmentId = SelectedDepartment.Id,
                    EntranceSemesterId = SelectedSemester.Id,
                    Courses = courses
                };

              
                
                    _mainMV.Globals.CourseGroupRepository.AddCourseGroups(newCourseGroup);
                    MessageBox.Show("Ders grubu başarıyla eklendi!");
                    YeniDersGrubuAdı = string.Empty;
                    YeniDersGrubuİstenenDers = string.Empty;
                    YeniDersGrubuİstenenKredisi = string.Empty;
                    YeniDersGrubuDersEkle = string.Empty;
                    SelectedDepartment = null;
                    SelectedSemester = null;

                _mainMV.CurrentViewModel = new DersGrubuListelemeViewModel(_mainMV);

                });

                LoadDepartments();
                LoadSemesters();
                LoadCourseGroups();
        }

        private void LoadDepartments()
        {
            var departmentList = _mainMV.Globals.DepartmentRepository.GetAllDepartments();
            Departments = new ObservableCollection<Department>(departmentList.ToList());
        }

        private void LoadCourseGroups()
        {
            var courseGroups = _mainMV.Globals.CourseGroupRepository.GetAllCourseGroups();
            CourseGroups = new ObservableCollection<CourseGroup>(courseGroups.ToList());
        }

        private void LoadSemesters()
        {
            var semesterList = _mainMV.Globals.SemesterRepository.GetAllSemesters();
            Semesters = new ObservableCollection<Semester>(semesterList.ToList());
        }

        private void LoadCoursesForSelectedDepartment()
        {
            if (SelectedDepartment != null)
            {
                var courses = _mainMV.Globals.CourseRepository
                    .GetAllCourses()
                    .Where(c => c.DepartmentId == SelectedDepartment.Id)
                    .ToList();

                FilteredCourses = new ObservableCollection<Course>(courses);
            }
            else
            {
                FilteredCourses = new ObservableCollection<Course>();
            }
            OnPropertyChanged(nameof(FilteredCourses));
        }
    }
}
