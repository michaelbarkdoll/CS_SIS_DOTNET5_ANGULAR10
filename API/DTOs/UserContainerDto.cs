namespace API.DTOs
{
    public class UserContainerDto
    {
        public int Id { get; set; }
        public string ContainerId { get; set; }
        public string Image { get; set; }
        public string Command { get; set; }
        public int InternalPort { get; set; }
        public int ExternalPort { get; set; }
        
        public string JobOwner { get; set; }
        public string ContainerHost { get; set; }
        public string ContainerStatus { get; set; }
        

        // Fully define the relationship   
        // public AppUser AppUser { get; set; }    // If we delete a user delete all PrintJob associated with the user
        // public int AppUserId { get; set; }      // AppUser can't be nullable; All PrintJob must be related to an AppUser
    }
}