using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Instructors")]
    public class Instructor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        // Fully define the relationship   
        public Course Course { get; set; }    // If we delete a Course delete all Instructor associated with the Course
        public int CourseId { get; set; }      // Instructor can't be nullable; All Instructor must be related to an CourseId
    }
}