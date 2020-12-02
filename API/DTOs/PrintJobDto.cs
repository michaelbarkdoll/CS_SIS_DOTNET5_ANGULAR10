namespace API.DTOs
{
    public class PrintJobDto
    {
        public int Id { get; set; }
        public int JobNumber { get; set; }
        public string JobOwner { get; set; }
        public string JobName { get; set; }
        public string JobStatus { get; set; }
        public string PrinterName { get; set; }
        public int NumberOfPages { get; set; }
        public int PagesPrinted { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int DawgTag { get; set; }

        // Fully define the relationship   
        // public AppUser AppUser { get; set; }    // If we delete a user delete all PrintJob associated with the user
        // public int AppUserId { get; set; }      // AppUser can't be nullable; All PrintJob must be related to an AppUser

        // public Printer Printer { get; set; }    // If we delete a Printer delete all PrintJob associated with the Printer
        // public int PrinterId { get; set; }      // Printer can't be nullable; All PrintJob must be related to an Printer

    }
}