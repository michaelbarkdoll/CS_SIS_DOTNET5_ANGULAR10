export interface Course {
    id: number;
    startDate: string;
    endDate: string;
    ta: string;
    fullCourseDescription: string;
    courseTitle?: string;
    college?: string;
    department?: string;
    crn?: string;
    building?: string;
    classFormat?: string;
    classMeetingTimes?: string;
    beginTime1: number;
    endTime1: number;
    beginTime2: number;
    endTime2: number;
    beginTime3: number;
    endTime3: number;
    courseStatus?: string;
    courseCredits: number;
    withdrawlDropDate: string;
    prerequisites?: string;
    requiredSeqNum: number;
    site?: string;
    courseStudents?: string;
    instructors?: string;
    semester?: string;
    semesterId: number;
    /*
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TA { get; set; }
    public string FullCourseDescription { get; set; }
    public string CourseTitle { get; set; }
    public string College { get; set; }
    public string Department { get; set; }
    public string CRN { get; set; }
    public string Building { get; set; }
    public string ClassFormat { get; set; }
    public string ClassMeetingTimes { get; set; }
    public int BeginTime1 { get; set; }
    public int EndTime1 { get; set; }
    public int BeginTime2 { get; set; }
    public int EndTime2 { get; set; }
    public int BeginTime3 { get; set; }
    public int EndTime3 { get; set; }
    public string CourseStatus { get; set; }
    public int CourseCredits { get; set; }
    public DateTime WithdrawlDropDate { get; set; }
    public string Prerequisites { get; set; }
    public int RequiredSeqNum { get; set; }
    public string Site { get; set; }
    public ICollection<CourseStudent> CourseStudents { get; set; }
    public ICollection<Instructor> Instructors { get; set; }

    public Semester Semester { get; set; }
    public int SemesterId { get; set; } 
    */
}