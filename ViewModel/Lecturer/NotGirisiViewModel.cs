using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Database.Entities;
using gp_unisis.ViewModel;

namespace gp_unisis.ViewModel.Lecturer
{
    class NotGirisiViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;

        public ICommand AkademisyenAnaSayfaCommand { get; set; }
        public ICommand DersDuzenleSilCommand { get; set; }
        public ICommand DerseSinavEkleCommand { get; set; }
        public ICommand DersProgramiCommand { get; set; }
        public ICommand DuyuruGoruntulemeCommand { get; set; }
        public ICommand NotGirisiCommand { get; set; }
        public ICommand ProgramaDersEkleCommand { get; set; }
        public ICommand SinavProgramiCommand { get; set; }
        public ICommand TranskriptAnaSayfaCommand { get; set; }
        public ICommand DersListesiCommand { get; set; }
        public ICommand TranskriptGoruntulemeCommand { get; set; }
        public ICommand TranskriptHesaplamaCommand { get; set; }
        public ICommand OnaylaCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<NotGirisi> NotGirisleri { get; set; } = new ObservableCollection<NotGirisi>();

        public NotGirisiViewModel(MainWindowViewModel mainVM, int id)
        {
            _mainVM = mainVM;

            AkademisyenAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM));
            
            DerseSinavEkleCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM));

            DersProgramiCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            
            SinavProgramiCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));

            TranskriptAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM));
            
            DersListesiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM));
            DersEkleCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersEkleViewModel(_mainVM));


            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var exam = _mainVM.Globals.LoggedLecturer.Courses.SelectMany(c => c.Exams).FirstOrDefault(e => e.Id == id);
            var grades = exam.Grades;
            var students = _mainVM.Globals.StudentCourseSelectionRepository.GetAllSelections()
                .Where(s => s.SemesterId == _mainVM.Globals.ActiveSemesterId && s.Confirmed && s.Courses.Any(c => c.Id == exam.CourseId))
                .Select(s => s.Student).ToList();

            foreach (var student in students)
            {
                var oldNot = 0d;
                var existingGrades = grades.FirstOrDefault(g => g.StudentId == student.Id);
                if (existingGrades != null)
                {
                    oldNot = existingGrades.Score;
                }
                var not = new NotGirisi
                {
                    OgrenciId = student.Id,
                    AdSoyad = student.FirstName + " " + student.LastName,
                    OgrenciNo = student.StudentNumber,
                    Not = oldNot,
                };
                
                NotGirisleri.Add(not);
            }

            OnaylaCommand = new RelayCommand(_ =>
            {
                foreach (var not in NotGirisleri)
                {
                    var existingGrade = _mainVM.Globals.GradeRepository.GetGradesByExamId(id)
                        .FirstOrDefault(g => g.StudentId == not.OgrenciId);

                    if (existingGrade != null)
                    {
                        existingGrade.Score = not.Not ?? 0.0;
                        try
                        {
                            _mainVM.Globals.GradeRepository.UpdateGrade(existingGrade);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata meydana geldi!");
                        }
                    }
                    else
                    {
                        var grade = new Grade
                        {
                            ExamId = id,
                            StudentId = not.OgrenciId,
                            Score = not.Not ?? 0.0,
                        };

                        try
                        {
                            _mainVM.Globals.GradeRepository.AddGrade(grade);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata meydana geldi!");
                        }
                    }
                }

                MessageBox.Show("Notlar hesaplandı!");
                _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM);
            });
        }
        
        public class NotGirisi : INotifyPropertyChanged
        {
            public int OgrenciId { get; set; }
            public string OgrenciNo { get; set; }
            public string AdSoyad { get; set; }

            private double? _not;
            public double? Not
            {
                get => _not;
                set
                {
                    _not = value;
                    OnPropertyChanged(nameof(Not));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}