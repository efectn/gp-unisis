using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gp_unisis.ViewModel.StudentAffairs
{
    class AffairsAnaSayfaViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AnasayfaCommand { get; set; }
        public ICommand DersSecimiListeleCommand { get; set; }
        public ICommand OgrenciEkleCommand { get; set; }
        public ICommand OgrenciListeleCommand { get; set; }
        public ICommand DersSecimiGoruntuleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }
        public string Name { get; set; }
      
        public AffairsAnaSayfaViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            
            AnasayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AffairsAnaSayfaViewModel(_mainVM));
            DersSecimiListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSecimiListeleViewModel(_mainVM));
            OgrenciEkleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciEkleViewModel(_mainVM));
            OgrenciListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedStudentPersonal = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var user = _mainVM.Globals.LoggedStudentPersonal;
            if (user == null)
            {
                return;
            }
            
            Name = "İsim: " + user.Name;
        }
    }
}