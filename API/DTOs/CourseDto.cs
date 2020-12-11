using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TA { get; set; }
        public string FullCourseDescription { get; set; }
        // public Semester Semester { get; set; }
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
        
        //public Semester Semester { get; set; }
        public int SemesterId { get; set; } 
    }
}