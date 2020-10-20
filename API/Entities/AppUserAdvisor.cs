namespace API.Entities
{
    public class AppUserAdvisor
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser AdvisedUser { get; set; }
        public int AdvisedUserId { get; set; }
        // public bool SeniorThesis { get; set; }
        // public bool GradThesis { get; set; }
        // public bool GradProject { get; set; }
        
    }
}