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
    class TranskriptAnaSayfaViewModel : ViewModelBase
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
        public ICommand TranskriptGoruntulemeCommand { get; set; }
        public ICommand TranskriptHesaplamaCommand { get; set; }
        public ICommand DersListesiCommand { get; set; }
        public ICommand DersEkleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }
        public ObservableCollection<Course> Courses { get; set; }

        public TranskriptAnaSayfaViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;


            TranskriptAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new TranskriptAnaSayfaViewModel(_mainVM));

            AkademisyenAnaSayfaCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM));
            
            DerseSinavEkleCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new DersSinavEkleViewModel(_mainVM));

            DersProgramiCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new DersProgramiViewModel(_mainVM));

            
            SinavProgramiCommand = new RelayCommand(param =>
                _mainVM.CurrentViewModel = new SinavProgramiViewModel(_mainVM));
            
            DersListesiCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersListesiViewModel(_mainVM));
            DersEkleCommand = new RelayCommand(_ => _mainVM.CurrentViewModel = new DersEkleViewModel(_mainVM));


            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedLecturer = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var courses = _mainVM.Globals.LoggedLecturer.Courses
                .Where(c => c.Semesters.Any(s => s.Id == _mainVM.Globals.ActiveSemesterId)).ToList();
            Courses = new ObservableCollection<Course>(courses);

            TranskriptHesaplamaCommand = new RelayCommand(param =>
            {
                if (param is Course c)
                {
                    _mainVM.CurrentViewModel = new TranskriptHesaplamaViewModel(_mainVM, c.Id);
                }
            });
            
            TranskriptGoruntulemeCommand = new RelayCommand(param =>
            {
                if (param is Course c)
                {
                    _mainVM.CurrentViewModel = new TranskriptGoruntulemeViewModel(_mainVM, c.Id);
                }
            });
        }
    }
}