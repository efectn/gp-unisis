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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using gp_unisis.Helpers;

namespace gp_unisis.ViewModel.StudentAffairs
{
    class OgrenciEkleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AffairsAnaSayfaCommand { get; set; }
        public ICommand DersSecimiListeleCommand { get; set; }
        public ICommand OgrenciDuzenleCommand { get; set; }
        public ICommand OgrenciListeleCommand { get; set; }

        public ICommand DersSecimiGoruntuleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }
        
        private Database.Entities.Student _student;
        public Database.Entities.Student Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }
        public ObservableCollection<Department> Departments { get; set; }
        public ICommand OgrenciEkleCommand { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }

        public ObservableCollection<Semester> Semesters { get; set; }
        public int SelectedSemesterId { get; set; }

        public OgrenciEkleViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            
            AffairsAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AffairsAnaSayfaViewModel(_mainVM));
            DersSecimiListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new DersSecimiListeleViewModel(_mainVM));
            OgrenciListeleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedStudentPersonal = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var departments = _mainVM.Globals.DepartmentRepository.GetAllDepartments();
            Departments = new ObservableCollection<Department>(departments);

            Semesters = new ObservableCollection<Semester>(_mainVM.Globals.SemesterRepository.GetAllSemesters());


            OgrenciEkleCommand = new RelayCommand((_) =>
            {
                // Check imputs are not empty
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) ||
                    string.IsNullOrEmpty(StudentNumber) || string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(NationalId))
                {
                    MessageBox.Show("Gerekli yerleri doldurun");
                }

                var student = new Database.Entities.Student
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    StudentNumber = StudentNumber,
                    Password = MD5Helper.CreateMD5(Password),
                    Email = Email,
                    NationalId = NationalId,
                    DateOfBirth = DateOfBirth,
                    DepartmentId = DepartmentId,
                    EntranceSemesterId = SelectedSemesterId,
                };

                try
                {
                    _mainVM.Globals.StudentRepository.AddStudent(student);
                    MessageBox.Show("Öğrenci başarıyla eklendi!");
                    _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}