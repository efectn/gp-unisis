using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Database.Entities;

namespace gp_unisis.ViewModel.Student
{
    class DersSecimiViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AnasayfaCommand { get; set; }
        public ICommand DersSecimiCommand { get; set; }
        public ICommand DonemDersleriCommand { get; set; }
        public ICommand NotlarimCommand { get; set; }
        public ICommand SinavProgramiCommand { get; set; }
        public ICommand TranskriptCommand { get; set; }
        public ICommand DersProgramiCommand { get; set; }
        public ICommand DersGruplariCommand { get; set; }
        public ICommand OnaylaCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Ders2> Dersler { get; set; } = new ObservableCollection<Ders2>();
        public string ActiveSemester { get; set; }
        public string Credits { get; set; } = "0 / 40";
        public string Confirmed { get; set; } = "Onaylanmadı";
        public DersSecimiViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;

            AnasayfaCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new StudentDashboardViewModel(_mainVM));
            DersSecimiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSecimiViewModel(_mainVM));
            DonemDersleriCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new DonemDerslerimViewModel(_mainVM));
            NotlarimCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new NotlarimViewModel(_mainVM));
            SinavProgramiCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            TranskriptCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new TranskriptViewModel(_mainVM));
            DersProgramiCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            DersGruplariCommand =
                new RelayCommand(param => _mainVM.CurrentViewModel = new DersGruplariViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var existingSelection = _mainVM.Globals.StudentCourseSelectionRepository.GetAllSelections()
                .FirstOrDefault(scs =>
                    scs.StudentId == _mainVM.Globals.LoggedUser.Id &&
                    scs.SemesterId == _mainVM.Globals.ActiveSemesterId);

            var courses = _mainVM.Globals.LoggedUser.Department.Courses
                .Where(c => c.Semesters != null &&  c.Semesters.Any(s => s.Id == _mainVM.Globals.ActiveSemesterId))
                .ToList();
            
            if (existingSelection != null && existingSelection.Confirmed)
            {
                Confirmed = "Onaylandı";
                Credits = existingSelection.Courses.Sum(c => c.Credit).ToString() + " / 40";

            }
            ActiveSemester = _mainVM.Globals.ActiveSemester.Name;

            foreach (var course in courses)
            {
                var isSelected = false;
                if (existingSelection != null)
                {
                    isSelected = existingSelection.Courses.Contains(course);
                }
                var ders = new Ders2
                {
                    Id = course.Id,
                    IsSelected = isSelected,
                    DersAdi = course.Name,
                    Akademisyen = course.Lecturer.FullName,
                    Kredi = course.Credit,
                    Donem = course.SemesterNumber.ToString(),
                    Kontenjan = course.Quota,
                };
                Dersler.Add(ders);
            }

            OnaylaCommand = new RelayCommand((object param) =>
            {
                if (existingSelection != null && existingSelection.Confirmed)
                {
                    MessageBox.Show("Ders seçimi onaylanmış, değişiklik yapılamaz!");
                }
                else
                {
                    // remove old one
                    if (existingSelection != null)
                    {
                        _mainVM.Globals.StudentCourseSelectionRepository.DeleteSelection(existingSelection.Id);
                    }
                    
                    var selectedCourseIds = Dersler.Where(d => d.IsSelected).Select(d => d.Id).ToList();
                    var selectedCourses = new List<Course>();
                    int totalCredits = 0;
                    MessageBox.Show(string.Join(", ", selectedCourseIds));
                    foreach (var courseId in selectedCourseIds)
                    {
                        var course = _mainVM.Globals.CourseRepository.GetCourseById(courseId);
                        if (course != null)
                        {
                            // Check quota from student course selection table
                            var selectedCount = _mainVM.Globals.StudentCourseSelectionRepository.GetAllSelections()
                                .Where(scs =>
                                    scs.SemesterId == _mainVM.Globals.ActiveSemesterId &&
                                    scs.StudentId != _mainVM.Globals.LoggedUser.Id)
                                .SelectMany(scs => scs.Courses)
                                .Count(c => c.Id == courseId);

                            if (selectedCount >= course.Quota)
                            {
                                MessageBox.Show($"Ders ID'si {courseId} için kontenjan kalmadı.");
                                continue;
                            }

                            selectedCourses.Add(course);
                            totalCredits += course.Credit;
                        }
                        else
                        {
                            MessageBox.Show($"Ders ID'si {courseId} bulunamadı.");
                        }
                    }

                    if (totalCredits > 40)
                    {
                        Console.WriteLine("Seçtiğiniz derslerin toplam kredisi 40'ı aşıyor.");
                        return;
                    }

                    // Check quota for each selected course, look at in studentselection table
                    foreach (var course in selectedCourses)
                    {
                        if (course.Quota <= 0)
                        {
                            Console.WriteLine($"Ders ID'si {course.Id} için kontenjan kalmadı.");
                            return;
                        }
                    }


                    // Create a new StudentCourseSelection object
                    var studentCourseSelection = new StudentCourseSelection
                    {
                        SemesterId = _mainVM.Globals.ActiveSemesterId,
                        StudentId = _mainVM.Globals.LoggedUser.Id,
                        Confirmed = false,
                        Cancelled = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Courses = selectedCourses
                    };

                    try
                    {
                        _mainVM.Globals.StudentCourseSelectionRepository.AddSelection(studentCourseSelection);
                        existingSelection = studentCourseSelection;
                        MessageBox.Show("Ders kaydı başarıyla gerçekleştirildi!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }
    }

    public class Ders2
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string DersAdi { get; set; }
        public string Akademisyen { get; set; }
        public int Kredi { get; set; }
        public string Donem { get; set; }
        public int Kontenjan { get; set; }
    }
}