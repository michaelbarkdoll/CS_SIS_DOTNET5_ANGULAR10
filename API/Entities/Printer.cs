using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Printers")]
    public class Printer
    {
        public int Id { get; set; }
        public string PrinterName { get; set; }
        public string URL { get; set; }
        public int Port { get; set; } = 631;
        public string SshUsername { get; set; }
        public string SshPassword { get; set; }
        public string SshHostname { get; set; }
        public string SshPublicKey { get; set; }

        // List of PrintJob that are owned by current Printer
        public ICollection<PrintJob> PrintJobs { get; set; }

    }
}