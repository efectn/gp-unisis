using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.ViewModel;

public class CourseApproveViewModel
{
    private readonly CourseRepository _courseRepository;

    public CourseApproveViewModel(CourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public void AwaitingApprovalCourses()
    {
        var awaitingCourses = _courseRepository.NonApprovedCourses();

        Console.WriteLine("Onaylanmamış dersler:");

        if(awaitingCourses.Count == 0)
        {
            Console.WriteLine("Onaylanmamış ders bulunamadı.");
            return;
        }

        foreach(var course in awaitingCourses)
        {
            Console.WriteLine($"ID : {course.Id} - Ders Adı : {course.Name} - Kod : {course.Code} - Akademisyen : {course.Lecturer.FullName}");
        }
    }

    public void ApproveCourse()
    {
        Console.WriteLine("Onaylamak istediğiniz dersin ID değerini girin : ");
        if(!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Girilen ID değerine ait ders bulunamadı.");
            return;
        }

        var course = _courseRepository.GetCourseById(courseId);
        if(course == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }

        if (course.IsConfirmed == true)
        {
            Console.WriteLine("Ders zaten onaylanmış.");
            return;
        }

        _courseRepository.ApproveCourse(course.Id);

        Console.WriteLine($"Ders {course.Name} başarıyla onaylandı");
    }

    public void RejectCourse()
    {
        Console.WriteLine("Reddetmek istediğiniz dersin ID değerini girin :");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Girilen ID değerine ait ders bulunamadı.");
            return;
        }

        var course = _courseRepository.GetCourseById(courseId);
        if (course == null)
        {
            Console.WriteLine("Ders bulunamadı.");
            return;
        }

        if (course.IsConfirmed == false)
        {
            Console.WriteLine("Ders zaten reddedilmiş.");
            return;
        }
        _courseRepository.RejectCourse(course.Id);

        Console.WriteLine($"Ders {course.Name} başarıyla reddedildi");
    }
}