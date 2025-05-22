using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gp_unisis.Database.Repositories;

public class CourseLetterGradeIntervalRepository
{
    private readonly ApplicationDbContext _context;

    public CourseLetterGradeIntervalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void AddCourseLetterGradeInterval(CourseLetterGradeInterval interval)
    {
        if (interval == null)
            throw new ArgumentNullException(nameof(interval));

        _context.CourseLetterGradeIntervals.Add(interval);
        _context.SaveChanges();
    }

    public void UpdateCourseLetterGradeInterval(CourseLetterGradeInterval interval)
    {
        if (interval == null)
            throw new ArgumentNullException(nameof(interval));

        var existing = _context.CourseLetterGradeIntervals
            .FirstOrDefault(i => i.Id == interval.Id);

        if (existing == null)
            throw new InvalidOperationException($"Interval with ID {interval.Id} does not exist.");

        // Update properties
        existing.CourseId = interval.CourseId;
        existing.SemesterId = interval.SemesterId;
        existing.IsBellCurve = interval.IsBellCurve;

        existing.AAStart = interval.AAStart;
        existing.AAEnd = interval.AAEnd;
        existing.BAStart = interval.BAStart;
        existing.BAEnd = interval.BAEnd;
        existing.BBStart = interval.BBStart;
        existing.BBEnd = interval.BBEnd;
        existing.CBStart = interval.CBStart;
        existing.CBEnd = interval.CBEnd;
        existing.CCStart = interval.CCStart;
        existing.CCEnd = interval.CCEnd;
        existing.DCStart = interval.DCStart;
        existing.DCEnd = interval.DCEnd;
        existing.DDStart = interval.DDStart;
        existing.DDEnd = interval.DDEnd;
        existing.FDStart = interval.FDStart;
        existing.FDEnd = interval.FDEnd;

        _context.SaveChanges();
    }

    public void DeleteCourseLetterGradeInterval(int id)
    {
        var existing = _context.CourseLetterGradeIntervals.FirstOrDefault(i => i.Id == id);

        if (existing == null)
            throw new InvalidOperationException($"Interval with ID {id} does not exist.");

        _context.CourseLetterGradeIntervals.Remove(existing);
        _context.SaveChanges();
    }

    public CourseLetterGradeInterval GetIntervalById(int id)
    {
        return _context.CourseLetterGradeIntervals
            .Include(i => i.Course)
            .Include(i => i.Semester)
            .FirstOrDefault(i => i.Id == id);
    }

    public List<CourseLetterGradeInterval> GetAllIntervals()
    {
        return _context.CourseLetterGradeIntervals
            .Include(i => i.Course)
            .Include(i => i.Semester)
            .ToList();
    }

    public List<CourseLetterGradeInterval> GetIntervalsByCourseAndSemester(int courseId, int semesterId)
    {
        return _context.CourseLetterGradeIntervals
            .Where(i => i.CourseId == courseId && i.SemesterId == semesterId)
            .Include(i => i.Course)
            .Include(i => i.Semester)
            .ToList();
    }
}