using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class ExamLetterGradeIntervalRepository
{
    private readonly ApplicationDbContext _context;

    public ExamLetterGradeIntervalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddGradeInterval(ExamLetterGradeInterval examLetterGradeInterval, int courseId)
    {
        if (examLetterGradeInterval == null)
            throw new ArgumentNullException(nameof(examLetterGradeInterval));

        if (courseId == 0)
            throw new ArgumentException("CourseId must be a valid ");

        examLetterGradeInterval.CourseId = courseId;

        examLetterGradeInterval.AAStart = 0;
        examLetterGradeInterval.AAEnd = 0;

        examLetterGradeInterval.BAStart = 0;
        examLetterGradeInterval.BAEnd = 0;

        examLetterGradeInterval.BBStart = 0;
        examLetterGradeInterval.BBEnd = 0;

        examLetterGradeInterval.CBStart = 0;
        examLetterGradeInterval.CBEnd = 0;

        examLetterGradeInterval.CCStart = 0;
        examLetterGradeInterval.CCEnd = 0;

        examLetterGradeInterval.DCStart = 0;
        examLetterGradeInterval.DCEnd = 0;

        examLetterGradeInterval.DDStart = 0;
        examLetterGradeInterval.DDEnd = 0;

        examLetterGradeInterval.FDStart = 0;
        examLetterGradeInterval.FDEnd = 0;

        _context.examLetterGradeIntervals.Add(examLetterGradeInterval);
        _context.SaveChanges();
    }
}