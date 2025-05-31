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
    class SinavProgramiViewModel : ViewModelBase
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
        public ICommand SinavEkleCommand { get; set; }
        public ICommand SilCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<Exam> Exams { get; set; }

        public SinavProgramiViewModel(MainWindowViewModel mainVM)
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

            var exams = _mainVM.Globals.CourseRepository.GetCoursesByLecturerId(user.Id).SelectMany(c => c.Exams).Where(e => e.SemesterId == semesterId).ToList();
            Exams = new ObservableCollection<Exam>(exams);

            SinavEkleCommand = new RelayCommand(_ =>
            {
                _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM);
            });

            SilCommand = new RelayCommand(param =>
            {
                if (param is Exam entry)
                {
                    try
                    {
                        if (entry.IsExamCalculated)
                        {
                            MessageBox.Show("Hesaplanans ınav silinemez!");
                            return;
                        }
                        _mainVM.Globals.ExamRepository.DeleteExam(entry);
                        MessageBox.Show("Sınav başarıyla silindi!");
                        _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                    // Refresh page
                    _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM);
                }
            });
            
            NotGirisiCommand = new RelayCommand(param =>
            {
                if (param is Exam entry)
                {
                    _mainVM.CurrentViewModel = new NotGirisiViewModel(_mainVM, entry.Id);
                }
            });
        }
    }
}