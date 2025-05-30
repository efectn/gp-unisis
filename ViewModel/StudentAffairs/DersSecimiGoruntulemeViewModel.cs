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

namespace gp_unisis.ViewModel.StudentAffairs
{
    class DersSecimiGoruntulemeViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AffairsAnaSayfaCommand { get; set; }
        public ICommand DersSecimiListeleCommand { get; set; }
        public ICommand OgrenciEkleCommand { get; set; }
        public ICommand OgrenciListeleCommand { get; set; }
        public Database.Entities.Student Student { get; set; }
        public ObservableCollection<Course> Dersler { get; set; }
        public string Credit { get; set; }
        public ICommand OnaylaCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

      
        public DersSecimiGoruntulemeViewModel(MainWindowViewModel mainVM, int id)
        {
            _mainVM = mainVM;
            
            AffairsAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AffairsAnaSayfaViewModel(_mainVM));
            DersSecimiListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSecimiListeleViewModel(_mainVM));
            OgrenciEkleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciEkleViewModel(_mainVM));
            OgrenciListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedStudentPersonal = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var scs = mainVM.Globals.StudentCourseSelectionRepository.GetSelectionById(id);
            Student = scs.Student;
            Dersler = new ObservableCollection<Course>(scs.Courses);
            Credit = scs.Courses.Sum(x => x.Credit) + " / 40";

            OnaylaCommand = new RelayCommand((_) =>
            {
                scs.Confirmed = true;
                try
                {
                    _mainVM.Globals.StudentCourseSelectionRepository.UpdateSelection(scs);
                    MessageBox.Show("Ders kaydı başarıyla onaylandı!");
                    _mainVM.CurrentViewModel = new DersSecimiListeleViewModel(_mainVM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata meydana geldi!");
                }
            });
        }
    }
}