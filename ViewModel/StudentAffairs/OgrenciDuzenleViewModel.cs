using gp_unisis.Commands;
using gp_unisis.Database.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Helpers;

namespace gp_unisis.ViewModel.StudentAffairs
{
    class OgrenciDuzenleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AffairsAnaSayfaCommand { get; set; }
        public ICommand DersSecimiGoruntuleCommand { get; set; }
        public ICommand DersSecimiListeleCommand { get; set; }
        public ICommand OgrenciEkleCommand { get; set; }
        public ICommand OgrenciListeleCommand { get; set; }
        public ICommand GuncelleCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public Database.Entities.Student Student { get; set; }
        public ObservableCollection<Semester> Semesters { get; set; }
        public int SelectedSemesterId { get; set; }
        public string NewPassword { get; set; }

        public OgrenciDuzenleViewModel(MainWindowViewModel mainVM, int id)
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

            var student = mainVM.Globals.StudentRepository.GetStudentById(id);
            Student = student;
            SelectedSemesterId = student.EntranceSemesterId.Value;
            Semesters = new ObservableCollection<Semester>(mainVM.Globals.SemesterRepository.GetAllSemesters());

            GuncelleCommand = new RelayCommand((_) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(NewPassword))
                    {
                        student.Password = MD5Helper.CreateMD5(NewPassword);
                    }
                    student.EntranceSemesterId = SelectedSemesterId;
                    _mainVM.Globals.StudentRepository.UpdateStudent(student);
                    MessageBox.Show("Öğrenci güncellendi!");
                    _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata meydana geldi!");
                }
            });

        }
    }
}