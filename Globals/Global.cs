using gp_unisis.Database;
using gp_unisis.Database.Entities;
using gp_unisis.Database.Repositories;

namespace gp_unisis.Globals
{
    public class Global
    {
        public int Test;

        // Repository nesneleri için instance değişkenler
        public FacultyRepository FacultyRepository { get; }
        public DepartmentRepository DepartmentRepository { get; }
        public SemesterRepository SemesterRepository { get; }
        public AdminRepository AdminRepository { get; }
        public LecturerRepository LecturerRepository { get; }
        public CourseGroupsRepository CourseGroupRepository { get; }
        public CourseRepository CourseRepository { get; }
        public StudentRepository StudentRepository { get; }
        public CourseScheduleEntryRepository CourseScheduleRepository { get; }
        public ExamRepository ExamRepository { get; }
        public GradeRepository GradeRepository { get; }
        public StudentCourseSelectionRepository StudentCourseSelectionRepository { get; }
        public StudentPersonalRepository StudentPersonalRepository { get; }
        public AnnouncementRepository AnnouncementRepository { get; }
        public TranscriptRepository TranscriptRepository { get; }
        public CourseLetterGradeIntervalRepository CourseLetterGradeIntervalRepository { get; }
        public ApplicationDbContext ApplicationDbContext { get; }
        public Student LoggedUser { get; set; }
        public int ActiveSemesterId { get; set; }
        public Semester ActiveSemester { get; set; }
        
        public Lecturer LoggedLecturer { get; set; }
        public StudentPersonal LoggedStudentPersonal { get; set; }


        // Constructor: Tüm repository'leri başlatır
        public Global(ApplicationDbContext db)
        {
            Test = 1;
            FacultyRepository = new FacultyRepository(db);
            DepartmentRepository = new DepartmentRepository(db);
            SemesterRepository = new SemesterRepository(db);
            AdminRepository = new AdminRepository(db);
            LecturerRepository = new LecturerRepository(db);
            CourseGroupRepository = new CourseGroupsRepository(db);
            CourseRepository = new CourseRepository(db);
            StudentRepository = new StudentRepository(db);
            CourseScheduleRepository = new CourseScheduleEntryRepository(db);
            ExamRepository = new ExamRepository(db);
            GradeRepository = new GradeRepository(db);
            StudentCourseSelectionRepository = new StudentCourseSelectionRepository(db);
            StudentPersonalRepository = new StudentPersonalRepository(db);
            AnnouncementRepository = new AnnouncementRepository(db);
            TranscriptRepository = new TranscriptRepository(db);
            CourseLetterGradeIntervalRepository = new CourseLetterGradeIntervalRepository(db);
            ApplicationDbContext = db;
                    }
    }
}
