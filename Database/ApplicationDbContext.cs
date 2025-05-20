using gp_unisis.Database.Entities;

namespace gp_unisis.Database;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Transcript> Transcripts { get; set; }
    public DbSet<CourseGroup> CourseGroups { get; set; }
    public DbSet<CourseScheduleEntry> CourseScheduleEntries { get; set; }
    public DbSet<StudentCourseSelection> StudentCourseSelections { get; set; }
    public DbSet<Exam> Exams { get; set; }

    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public ApplicationDbContext()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=database.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed the data
        Seed.AdminSeeder.Seed(modelBuilder);
        Seed.FacultySeeder.Seed(modelBuilder);
        Seed.DepartmentSeeder.Seed(modelBuilder);
        Seed.LecturerSeeder.Seed(modelBuilder);
        Seed.SemesterSeeder.Seed(modelBuilder);
        Seed.CourseSeeder.Seed(modelBuilder);
        Seed.StudentSeeder.Seed(modelBuilder);
    }
}