using gp_unisis.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using gp_unisis.Database.Entities;

namespace gp_unisis.ViewModel.StudentAffairs
{
    class OgrenciListeleViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainVM;
        public ICommand AffairsAnaSayfaCommand { get; set; }
        public ICommand DersSecimiListeleCommand { get; set; }
        public ICommand OgrenciEkleCommand { get; set; }
        public ICommand OgrenciSilCommand { get; set; }
        public ICommand OgrenciDuzenleCommand { get; set; }
        public ICommand MezunEtCommand { get; set; }

        public ICommand DersSecimiGoruntuleCommand { get; set; }


        public ICommand LogOutCommand { get; set; }
        public ObservableCollection<Database.Entities.Student> Students { get; set; }
      
        public OgrenciListeleViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            
            AffairsAnaSayfaCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new AffairsAnaSayfaViewModel(_mainVM));
            DersSecimiListeleCommand = new RelayCommand(param =>_mainVM.CurrentViewModel = new DersSecimiListeleViewModel(_mainVM));
            OgrenciEkleCommand = new RelayCommand(param => _mainVM.CurrentViewModel = new OgrenciEkleViewModel(_mainVM));
            LogOutCommand = new RelayCommand(param =>
            {
                _mainVM.Globals.LoggedStudentPersonal = null;
                _mainVM.CurrentViewModel = new LoginViewModel(_mainVM);
            });

            var students = _mainVM.Globals.StudentRepository.GetAllStudents();
            Students = new ObservableCollection<Database.Entities.Student>(students);
            
            OgrenciDuzenleCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Student scs)
                {
                    _mainVM.CurrentViewModel = new OgrenciDuzenleViewModel(_mainVM, scs.Id);
                }
            });

            MezunEtCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Student s)
                {
                    if (s.IsGraduated)
                    {
                        MessageBox.Show("Öğrenci zaten mezun olmuş!");
                    } else
                    {
                        var transcriptCourses = _mainVM.Globals.TranscriptRepository
                            ?.GetAllTranscripts()
                            ?.Where(tc => tc.StudentId == s.Id && !tc.HasFailed)
                            ?.ToList() ?? new List<Transcript>();

                        var courseGroups = _mainVM.Globals.CourseGroupRepository
                            ?.GetCourseGroupsByDepartmentId(s.DepartmentId)
                            ?.Where(cg => cg.EntranceSemesterId == s.EntranceSemesterId)
                            ?.ToList() ?? new List<CourseGroup>();

                        var transcriptCourseCodes = transcriptCourses.Select(tc => tc.CourseCode).ToList();
                        if (CanStudentGraduate(courseGroups, transcriptCourseCodes))
                        {
                            s.IsGraduated = true;
                            try
                            {
                                _mainVM.Globals.StudentRepository.UpdateStudent(s);
                                MessageBox.Show("Öğrenci mezun edildi!");
                            } catch (Exception e)
                            {
                                MessageBox.Show($"Hata: {e.Message}!");
                            }
                        } else
                        {
                            MessageBox.Show("Öğrenci mezun olması için gereken dersleri almamış!");
                        }
                    }
                }
            });

            OgrenciSilCommand = new RelayCommand(param =>
            {
                if (param is Database.Entities.Student scs)
                {
                    try
                    {
                        _mainVM.Globals.StudentRepository.DeleteStudent(scs.Id);
                        MessageBox.Show("Öğrenci başarıyla silindi!");
                        _mainVM.CurrentViewModel = new OgrenciListeleViewModel(_mainVM);
                    } catch (Exception e)
                    {
                        MessageBox.Show($"Hata: {e.Message}");
                    }
                }
            });
        }

        public bool CanStudentGraduate(List<CourseGroup> courseGroups, List<string> transcriptCourses)
        {
            foreach (var courseGroup in courseGroups)
            {
                int requiredCoursesCount = courseGroup.RequiredCoursesCount;
                int requiredCredits = courseGroup.RequiredCredits;

                foreach (var course in courseGroup.Courses ?? new List<Course>())
                {
                    if (transcriptCourses.Contains(course.Code))
                    {
                        requiredCoursesCount--;
                        requiredCredits -= course.Credit;
                    }
                }

                if (requiredCoursesCount > 0 || requiredCredits > 0)
                    return false;
            }

            return true;
        }
    }
}
