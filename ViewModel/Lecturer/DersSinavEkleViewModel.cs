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
using gp_unisis.ViewModel;

namespace gp_unisis.ViewModel.Lecturer
{
    class DersSinavEkleViewModel : ViewModelBase
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
        public ICommand DersEkleCommand { get; set; }


        public ICommand SinavEkleCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Course> Courses { get; set; }

        private string _name;
        private int _examCoefficient;
        private DateTime _examDate = DateTime.Now;
        private int _durationMinutes;
        private string _examTime;
        private int _courseId;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int ExamCoefficient
        {
            get => _examCoefficient;
            set
            {
                _examCoefficient = value;
                OnPropertyChanged();
            }
        }

        public DateTime ExamDate
        {
            get => _examDate;
            set
            {
                _examDate = value;
                OnPropertyChanged();
            }
        }

        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                _durationMinutes = value;
                OnPropertyChanged();
            }
        }

        public string ExamTime
        {
            get => _examTime;
            set
            {
                _examTime = value;
                OnPropertyChanged();
            }
        }

        public int CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
                OnPropertyChanged();
            }
        }


        public DersSinavEkleViewModel(MainWindowViewModel mainVM)
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

            var user = _mainVM.Globals?.LoggedLecturer;
            var semesterId = _mainVM.Globals?.ActiveSemesterId;
            var semester = _mainVM.Globals?.ActiveSemester;

            if (user == null || semesterId == null || semester == null)
            {
                return;
            }

            var courses = user.Courses
                .Where(c => c.Semesters.Any(s => s.Id == semesterId)).ToList();

            if (courses == null)
            {
                MessageBox.Show("Bu dönemde ders vermiyorsunuz!");
                _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM);
            }

            Courses = new ObservableCollection<Course>(courses);

            SinavEkleCommand = new RelayCommand(_ =>
            {
                try
                {
                    if (!TimeSpan.TryParse(ExamTime, out TimeSpan examTimeSpan))
                    {
                        MessageBox.Show("Sınav saati geçerli bir formatta değil. Örn: 14:30");
                        return;
                    }

                    // ExamDate ile ExamTime birleşimi
                    DateTime combinedDateTime = ExamDate.Date + examTimeSpan;

                    if (ExamCoefficient < 0 || ExamCoefficient > 100)
                    {
                        MessageBox.Show("Sınav ağrırlığı 0 ile 100 arasında olmalı");
                        return;
                    }
                    
                    var exam = new Exam
                    {
                        Name = Name,
                        CourseId = CourseId,
                        ExamCoefficient = ExamCoefficient,
                        ExamDate = combinedDateTime,
                        DurationMinutes = DurationMinutes,
                        SemesterId = _mainVM.Globals.ActiveSemesterId,
                        IsExamCalculated = false
                    };
                    
                    _mainVM.Globals.ExamRepository.AddExam(exam);
                    MessageBox.Show("Sınav başarıyla eklendi.");
                    _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sınav eklenirken hata oluştu: " + ex.Message);
                }
            });
        }
    }
}