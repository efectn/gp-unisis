using System;
using System.Collections.Generic;
using System.Linq;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel
{
    public class AddCourseViewModel
    {
        private readonly CourseRepository _courseRepository;
        private readonly CourseGroupsRepository _courseGroupRepository;
        private readonly LecturerRepository _lecturerRepository;
        private readonly DepartmentRepository _departmentRepository;

        public AddCourseViewModel(CourseRepository courseRepository, CourseGroupsRepository courseGroupRepository, LecturerRepository lecturerRepository, DepartmentRepository departmentRepository )
        {
            _courseRepository = courseRepository;
            _courseGroupRepository = courseGroupRepository;
            _lecturerRepository = lecturerRepository;
            _departmentRepository = departmentRepository;
        }

        public void AddCourse()
        {

            Console.WriteLine("Mevcut bölümler:");
            var departments = _departmentRepository.GetAllDepartments();
            foreach (var dep in departments)
            {
                Console.WriteLine($"- ID: {dep.Id}, İsim: {dep.Name}");
            }

            Console.Write("Bölüm ID girin: ");
            if (!int.TryParse(Console.ReadLine(), out int departmentId))
            {
                Console.WriteLine("Geçersiz bölüm ID'si.");
                return;
            }

            var lecturers = _lecturerRepository.GetLecturersByDepartment(departmentId);
            if (lecturers.Count == 0)
            {
                Console.WriteLine("Bu bölümde akademisyen bulunamadı.");
                return;
            }

            Console.WriteLine("Bölüme ait akademisyenler:");
            foreach (var lecturer in lecturers)
            {
                Console.WriteLine($"- ID: {lecturer.Id}, İsim: {lecturer.FullName}");
            }

            Console.Write("Akademisyen ID (int): ");
            if (!int.TryParse(Console.ReadLine(), out int LecturerId))
            {
                Console.WriteLine("Geçersiz akademisyen ID'si.");
                return;
            }

            Console.WriteLine("Ders adı: ");
            var Name = Console.ReadLine();

            Console.WriteLine("Ders kodu: ");
            var Code = Console.ReadLine();

            Console.WriteLine("Kredi (sayı değeri): ");
            if (!int.TryParse(Console.ReadLine(), out int Credit))
            {
                Console.WriteLine("Geçersiz kredi.");
                return;
            }

            Console.WriteLine("Yarıyıl numarası girin: ");
            if (!int.TryParse(Console.ReadLine(), out int SemesterNumber))
            {
                Console.WriteLine("Geçersiz yarıyıl numarası.");
                return;
            }

            Console.WriteLine("Kontenjan: ");
            if (!int.TryParse(Console.ReadLine(), out int Quota))
            {
                Console.WriteLine("Geçersiz kontenjan değeri.");
                return;
            }

            Console.WriteLine("Zorunlu mu: (e/h)");
            var electiveSelection = Console.ReadLine().ToLower();
            bool IsElective = electiveSelection == "e";

            Console.WriteLine("Ders Açıklaması: ");
            var Description = Console.ReadLine();

            Console.WriteLine("Mevcut ders grupları: ");
            var courseGroups = _courseGroupRepository.GetAllCourseGroups();
            foreach (var group in courseGroups)
            {
                Console.WriteLine($"- ID: {group.Id}, İsim: {group.Name}");
            }

            Console.WriteLine("Ders grubu ID değerlerini aralarında virgül olacak şekilde yazın: ");
            var allGroups = Console.ReadLine();

            List<int> courseGroupIds = new List<int>();
            if (!string.IsNullOrEmpty(allGroups))
            {
                try
                {
                    courseGroupIds = allGroups.Split(',')
                                              .Select(s => int.Parse(s.Trim()))
                                              .ToList();
                }
                catch
                {
                    Console.WriteLine("Ders grubu ID'leri geçersiz formatta.");
                    return;
                }
            }

            if (string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(Code)
                || string.IsNullOrEmpty(electiveSelection)
                || string.IsNullOrEmpty(Description))
            {
                Console.WriteLine("Gerekli tüm alanlar dolu olmalı.");
                return;
            }

            try
            {
                var course = new Course
                {
                    Name = Name,
                    Code = Code,
                    Credit = Credit,
                    SemesterNumber = SemesterNumber,
                    Quota = Quota,
                    IsElective = IsElective,
                    Description = Description,
                    LecturerId = LecturerId,
                    CourseGroups = new List<CourseGroup>(),
                    IsConfirmed = false,
                };

                var allCourseGroups = _courseGroupRepository.GetAllCourseGroups();

                foreach (var groupId in courseGroupIds)
                {
                    var group = allCourseGroups.FirstOrDefault(g => g.Id == groupId);
                    if (group != null)
                    {
                        course.CourseGroups.Add(group);
                    }
                    else
                    {
                        Console.WriteLine($"Uyarı: {groupId} ID'li ders grubu bulunamadı.");
                    }
                }

                _courseRepository.AddCourse(course);
                Console.WriteLine("Ders başarıyla eklendi.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return;
            }
        }
    }
}
