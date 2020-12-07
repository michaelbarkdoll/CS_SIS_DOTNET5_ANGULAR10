using System;
using API.Entities;

namespace API.DTOs
{
    public class UserFileDto
    {
        public int Id { get; set; }
        public string Url { get; set; } 
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool isThesis { get; set; }
        public bool isProject { get; set; }
        public bool isOther { get; set; }
        public bool isPrintJob { get; set; }
        public string PublicId { get; set; }
        //public string FilePath { get; set; }
        public string FileName { get; set; }
        //public string StorageFileName { get; set; }
        
        // Fully Define the relationship
        //public AppUser AppUser { get; set; }
        //public int AppUserId { get; set; }      // AppUser can't be nullable; All photos must be related to an AppUser
    }
}