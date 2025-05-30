using gp_unisis.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace gp_unisis.Database.Repositories;

public class TranscriptRepository
{
    private readonly ApplicationDbContext _context;

    public TranscriptRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Transcript> GetAllTranscripts()
    {
        return _context.Transcripts
            .Include(t => t.Student)
            .Include(t => t.Course)
            .ThenInclude(t => t.Lecturer)
            .Include(t => t.Semester)
            .ToList();
    }

    public Transcript? GetTranscriptById(int id)
    {
        return _context.Transcripts
            .Include(t => t.Student)
            .Include(t => t.Course)
            .Include(t => t.Semester)
            .FirstOrDefault(t => t.Id == id);
    }

    public void AddTranscript(Transcript transcript)
    {
        if (transcript == null)
        {
            throw new ArgumentNullException(nameof(transcript));
        }

        if (transcript.StudentId == 0 || transcript.CourseId == 0 || transcript.SemesterId == 0)
        {
            throw new ArgumentException("StudentId, CourseId, and SemesterId must be set.");
        }

        var student = _context.Students.FirstOrDefault(s => s.Id == transcript.StudentId);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID {transcript.StudentId} does not exist.");
        }

        var course = _context.Courses.FirstOrDefault(c => c.Id == transcript.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {transcript.CourseId} does not exist.");
        }

        var semester = _context.Semesters.FirstOrDefault(s => s.Id == transcript.SemesterId);
        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {transcript.SemesterId} does not exist.");
        }

        _context.Transcripts.Add(transcript);
        _context.SaveChanges();
    }

    public void UpdateTranscript(Transcript transcript)
    {
        if (transcript == null)
        {
            throw new ArgumentNullException(nameof(transcript));
        }

        var existTranscript = _context.Transcripts.FirstOrDefault(t => t.Id == transcript.Id);
        if (existTranscript == null)
        {
            throw new InvalidOperationException($"Transcript with ID {transcript.Id} does not exist.");
        }

        if (transcript.StudentId == 0 || transcript.CourseId == 0 || transcript.SemesterId == 0)
        {
            throw new ArgumentException("StudentId, CourseId, and SemesterId must be set.");
        }

        var student = _context.Students.FirstOrDefault(s => s.Id == transcript.StudentId);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID {transcript.StudentId} does not exist.");
        }

        var course = _context.Courses.FirstOrDefault(c => c.Id == transcript.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException($"Course with ID {transcript.CourseId} does not exist.");
        }

        var semester = _context.Semesters.FirstOrDefault(s => s.Id == transcript.SemesterId);
        if (semester == null)
        {
            throw new InvalidOperationException($"Semester with ID {transcript.SemesterId} does not exist.");
        }
        
        Console.WriteLine("2regfth {0}", transcript.LetterGrade);

        existTranscript.StudentId = transcript.StudentId;
        existTranscript.CourseId = transcript.CourseId;
        existTranscript.SemesterId = transcript.SemesterId;
        existTranscript.LetterGrade = transcript.LetterGrade;
        existTranscript.HasFailed = transcript.HasFailed;

        _context.SaveChanges();
    }

    public void DeleteTranscript(int id)
    {
        var existTranscript = _context.Transcripts.FirstOrDefault(t => t.Id == id);
        if (existTranscript == null)
        {
            throw new InvalidOperationException($"Transcript with ID {id} does not exist.");
        }

        _context.Transcripts.Remove(existTranscript);
        _context.SaveChanges();
    }
}