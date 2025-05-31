using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gp_unisis.Globals;

namespace gp_unisis.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public Global Globals;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand ShowLoginPage { get; }

        public MainWindowViewModel(Global globals)
        {
            ShowLoginPage = new RelayCommand(showLoginPage);
            Globals = globals;

            CurrentViewModel = new LoginViewModel(this);
        }

        private void showLoginPage(object _)
        {
            CurrentViewModel = new LoginViewModel(this);
        }
    }

}
