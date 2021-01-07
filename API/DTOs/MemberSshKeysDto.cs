using System;

namespace API.DTOs
{
    public class MemberSshKeysDto
    {
        public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        public string Username { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.
        public string PhotoUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PersonalURL { get; set; }
        public string RequestedURL { get; set; }
        public string OldPersonalURL { get; set; }
        public string PublicKeySSH1 { get; set; }
        public string PrivateKeySSH1 { get; set; }
        public string PublicKeySSH2 { get; set; }
        public string PrivateKeySSH2 { get; set; }

        // public ICollection<PhotoDto> Photos { get; set; }
        // // public ICollection<UserFileDto> UserFiles { get; set; }
        // // public ICollection<PrintJobDto> UserPrintJobs { get; set; }
        // public ICollection<PrintJobDto> PrintJobs { get; set; }
    }
}