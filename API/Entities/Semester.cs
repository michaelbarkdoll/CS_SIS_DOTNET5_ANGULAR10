using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Semesters")]
    public class Semester
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Term { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}