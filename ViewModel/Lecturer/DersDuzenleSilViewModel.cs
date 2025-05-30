using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Database.Entities;
using gp_unisis.ViewModel;

namespace gp_unisis.ViewModel.Lecturer
{
    class DersDuzenleSilViewModel : ViewModelBase
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
        public ICommand DersDuzenleCommand { get; set; }

        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }

        public Course Course { get; set; }
        
        public string IsSelectedDisplay
        {
            get => Course.IsElective ? "EVET" : "HAYIR";
            set => Course.IsElective = value == "EVET";
        }


        public DersDuzenleSilViewModel(MainWindowViewModel mainVM, int id)
        {
            _mainVM = mainVM;
            AkademisyenAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM));
            DerseSinavEkleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM));
            DersProgramiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));
            SinavProgramiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            TranskriptAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM));
            DersListesiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM));
            DersEkleCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersEkleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            // Load the admin details based on the provided adminId
            LoadCourse(id);

            DersDuzenleCommand = new RelayCommand(param =>
            {
                if (Course != null)
                {
                    try
                    {
                        _mainVM.Globals.CourseRepository.UpdateCourse(Course);
                        MessageBox.Show("Ders başarıyla düzenlendi!");
                        _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                    return;
                }
            });
        }

        public void LoadCourse(int id)
        {
            Course = _mainVM.Globals.CourseRepository.GetCourseById(id);
        }

    }
}