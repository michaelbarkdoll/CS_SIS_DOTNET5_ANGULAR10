using System;

namespace API.DTOs
{
    public class MemberUpdateSshDto
    {
        public string Username { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.
        public string PersonalURL { get; set; }
        public string RequestedURL { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OldPersonalURL { get; set; }
        public string PublicKeySSH1 { get; set; }
        public string PrivateKeySSH1 { get; set; }
        public string PublicKeySSH2 { get; set; }
        public string PrivateKeySSH2 { get; set; }
    }
}