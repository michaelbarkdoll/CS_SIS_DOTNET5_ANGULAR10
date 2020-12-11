using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("CourseStudents")]
    public class CourseStudent
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Course Course { get; set; }     // If we delete a Course delete all CourseStudents associated with the course
        public int CourseId { get; set; }      // AppUser can't be nullable; All coursestudents must be related to an Course
    }
}