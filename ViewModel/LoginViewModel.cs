using gp_unisis.ViewModel.Admin;
using gp_unisis.ViewModel.Lecturer;
using gp_unisis.ViewModel.Student;
using gp_unisis.ViewModel.StudentAffairs;
using gp_unisis.Views.Admin;
using gp_unisis.Views.Lecturer;
using gp_unisis.Views.StudentAffairs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gp_unisis.Globals;
using gp_unisis.Helpers;


namespace gp_unisis.ViewModel
{
    enum RoleType
    {
        None,
        Ogrenci,
        Idari,
        Admin,
        Ogretmen
    }

    class LoginViewModel : ViewModelBase
    {
        public ObservableCollection<string> Usernames { get; set; }
        public string Selected { get; set; }

        public string SelectedEmail { get; set; }
        public string SelectedPassword { get; set; }

        private RoleType _selectedRole;
        private MainWindowViewModel _mainVM;

        public RoleType SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));
                }
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            LoginCommand = new Commands.RelayCommand(execute);
        }

        public void execute(object parameter)
        {
            if (SelectedRole == RoleType.Ogrenci)
            {
                var student = _mainVM.Globals.StudentRepository.GetAllStudents().Where(x => x.Email == SelectedEmail).Where(x => x.Password == MD5Helper.CreateMD5(SelectedPassword).ToLower()).FirstOrDefault();
                if (student == null)
                {
                    MessageBox.Show("Kullanıcı adı veya parola hatalı");
                }
                else
                {
                    _mainVM.Globals.LoggedUser = student;
                    _mainVM.CurrentViewModel = new StudentDashboardViewModel(_mainVM);
                }
            }

            if (SelectedRole == RoleType.Ogretmen)
            {
                var lecturer = _mainVM.Globals.LecturerRepository.GetAllLecturers().Where(x => x.Email == SelectedEmail && x.Password == MD5Helper.CreateMD5(SelectedPassword).ToLower()).FirstOrDefault();
                if (lecturer == null)
                {
                    MessageBox.Show("Kullanıcı adı veya parola hatalı");
                }
                else
                {
                    _mainVM.Globals.LoggedLecturer = _mainVM.Globals.LecturerRepository.GetLecturerById(7);
                    _mainVM.CurrentViewModel = new AkademisyenAnaSayfaViewModel(_mainVM);
                }
            }

            if (SelectedRole == RoleType.Admin)
            {
                var admin = _mainVM.Globals.AdminRepository.GetAllAdminsName().Where(x => x.Email == SelectedEmail && x.Password == MD5Helper.CreateMD5(SelectedPassword).ToLower()).FirstOrDefault();
                if (admin == null)
                {
                    MessageBox.Show("Kullanıcı adı veya parola hatalı");
                }
                else
                {
                    _mainVM.CurrentViewModel = new AdminAnaSayfaViewModel(_mainVM);
                }
            }

            if (SelectedRole == RoleType.Idari)
            {
                var studentAffair = _mainVM.Globals.StudentPersonalRepository.GetAllStudentPersonalsName()
                    .Where(x => x.Email == SelectedEmail && x.Password == MD5Helper.CreateMD5(SelectedPassword).ToLower())
                    .FirstOrDefault();
                if (studentAffair == null)
                {
                    MessageBox.Show("Kullanıcı adı veya parola hatalı");
                }
                else
                {
                    _mainVM.Globals.LoggedStudentPersonal =
                        _mainVM.Globals.StudentPersonalRepository.GetAllStudentPersonalsName().First();
                    _mainVM.CurrentViewModel = new AffairsAnaSayfaViewModel(_mainVM);
                }
            }
        }
    }
 }
