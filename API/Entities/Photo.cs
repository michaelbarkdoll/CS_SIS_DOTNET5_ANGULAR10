using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int PublicId { get; set; }
        // Fully define the relationship   
        public AppUser AppUser { get; set; }     // If we delete a user delete all photos associated with the user
        public int AppUserId { get; set; }      // AppUser can't be nullable; All photos must be related to an AppUser
    }
}