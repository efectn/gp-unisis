using gp_unisis.Views.Admin;
using gp_unisis.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gp_unisis.Database;
using gp_unisis.Database.Entities;
using gp_unisis.Globals;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Views
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var db = new ApplicationDbContext();
        if (db.Database.EnsureCreated())
        {
            // Add courses to semesters randomly
            for (int i = 1; i <= 50; i++)
            {
                // select a random semester
                var semesters = new List<Semester>();
                for (int j = 1; j < 25; j++)
                {
                    var rnd = new Random();
                    var semesterId = rnd.Next(1, 5);

                    if (!semesters.Any(s => s.Id == semesterId))
                    {
                        semesters.Add(db.Semesters.FirstOrDefault(s => s.Id == semesterId));
                    }

                    var course = db.Courses.FirstOrDefault(c => c.Id == i);
                    if (course != null)
                    {
                        course.Semesters = semesters;
                        db.SaveChanges();
                    }
                }
            }

            string[] days = ["Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"];
            // Add vize and final exams to courses for each semester
            for (int i = 1; i <= 50; i++)
            {
                var course = db.Courses.Include(c => c.Semesters).FirstOrDefault(c => c.Id == i);
                if (course != null)
                {
                    var semesters = course.Semesters.ToList();
                    if (semesters.Count == 0)
                    {
                        continue;
                    }

                    foreach (var semester in semesters)
                    {
                        // Add exams for course
                        var exam = new Exam
                        {
                            Name = "Vize",
                            ExamCoefficient = 40,
                            DurationMinutes = 60,
                            CourseId = course.Id,
                            SemesterId = semester.Id,
                            ExamDate = DateTime.Now.AddDays(30),
                            IsExamCalculated = false
                        };
                        db.Exams.Add(exam);

                        var finalExam = new Exam
                        {
                            Name = "Final",
                            ExamCoefficient = 60,
                            DurationMinutes = 120,
                            CourseId = course.Id,
                            SemesterId = semester.Id,
                            ExamDate = DateTime.Now.AddDays(60),
                            IsExamCalculated = false
                        };
                        db.Exams.Add(finalExam);

                        // Also add course schedule entries
                        var courseScheduleEntry = new CourseScheduleEntry
                        {
                            CourseId = course.Id,
                            SemesterId = semester.Id,
                            Day = days[new Random().Next(0, 7)],
                            StartTime = new TimeSpan(new Random().Next(8, 18), new Random().Next(0, 60), 0),
                            EndTime = new TimeSpan(new Random().Next(18, 24), new Random().Next(0, 60), 0),
                        };
                        db.CourseScheduleEntries.Add(courseScheduleEntry);

                        db.SaveChanges();
                    }
                }
            }

            // Add lecturers to departments according to courses
            for (int i = 1; i <= 50; i++)
            {
                var course = db.Courses.Include(c => c.Semesters).FirstOrDefault(c => c.Id == i);
                if (course != null)
                {
                    var lecturerId = course.LecturerId;
                    var lecturer = db.Lecturers.FirstOrDefault(l => l.Id == lecturerId);

                    if (lecturer != null)
                    {
                        var departmentId = course.DepartmentId;
                        var department = db.Departments.FirstOrDefault(d => d.Id == departmentId);
                        if (department != null)
                        {
                            if (department.Lecturers == null)
                            {
                                department.Lecturers = new List<Database.Entities.Lecturer>();
                            }
                            department.Lecturers.Add(lecturer);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        var globals = new Global(db);

        DataContext = new MainWindowViewModel(globals);
        }
    }
}
