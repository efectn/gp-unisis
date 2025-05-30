using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gp_unisis.Database.Entities;

namespace gp_unisis.ViewModel.StudentAffairs
{
    class DersSecimiListeleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AffairsAnaSayfaCommand { get; set; }
        public ICommand DersSecimiGoruntuleCommand { get; set; }
        public ICommand OgrenciEkleCommand { get; set; }
        public ICommand OgrenciListeleCommand { get; set; }
        public ICommand DersSecimiSilCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ObservableCollection<StudentCourseSelection> Selections { get; set; }
      
        public DersSecimiListeleViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            
            AffairsAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AffairsAnaSayfaViewModel(_mainVM));
            OgrenciEkleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciEkleViewModel(_mainVM));
            OgrenciListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedStudentPersonal = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var selections = _mainVM.Globals.StudentCourseSelectionRepository.GetAllSelections().Where(s => s.SemesterId == _mainVM.Globals.ActiveSemester.Id && !s.Confirmed);
            Selections = new ObservableCollection<StudentCourseSelection>(selections);
            
            DersSecimiGoruntuleCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.StudentCourseSelection scs)
                {
                    _mainVM.CurrentViewModel = new DersSecimiGoruntulemeViewModel(_mainVM, scs.Id);
                }
            });
            DersSecimiSilCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.StudentCourseSelection scs)
                {
                    _mainVM.Globals.StudentCourseSelectionRepository.DeleteSelection(scs.Id);
                    // Refresh page
                    _mainVM.CurrentViewModel = new DersSecimiListeleViewModel(_mainVM);
                }
            });
        }
    }
}