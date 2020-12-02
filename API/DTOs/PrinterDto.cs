namespace API.DTOs
{
    public class PrinterDto
    {
        public int Id { get; set; }
        public string PrinterName { get; set; }
        public string URL { get; set; }
        public int port { get; set; } = 631;
        public string SshUsername { get; set; }
        public string SshPassword { get; set; }
        public string SshHostname { get; set; }
        public string SshPublicKey { get; set; }

        // List of PrintJob that are owned by current Printer
        // public ICollection<PrintJob> PrintJobs { get; set; }
    }
}