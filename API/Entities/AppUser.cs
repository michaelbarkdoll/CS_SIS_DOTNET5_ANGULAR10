using System;
using System.Collections.Generic;
//using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        // public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        // public string UserName { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.

        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int DawgTag { get; set; }
        public string PersonalURL { get; set; }
        public string RequestedURL { get; set; }
        public string OldPersonalURL { get; set; }
        public int PageQuota { get; set; }
        public int TotalPagesPrinted { get; set; }
        public string PublicKeySSH1 { get; set; }
        public string PrivateKeySSH1 { get; set; }
        public string PublicKeySSH2 { get; set; }
        public string PrivateKeySSH2 { get; set; }
        public string PrimaryMajor { get; set; }
        public string SecondaryMajor { get; set; }
        public bool AccessPermitted { get; set; }
        public string EmailSIU { get; set; }
        public string PrimaryMajorProgram { get; set; }
        public string CLASS_LEVEL_BOAP { get; set; }
        public string EnrollmentStartTerm { get; set; }
        public int EnrollmentStartYear { get; set; }

        // List of PrintJob that are owned by current user
        public ICollection<PrintJob> PrintJobs { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserFile> UserFiles { get; set; }

        // List of users that like currently logged in user
        public ICollection<UserLike> LikedByUsers { get; set; }
        // List of users that currently logged in user has liked
        public ICollection<UserLike> LikedUsers { get; set; }
        // List of users that advised currently logged in user
        public ICollection<AppUserAdvisor> AdvisedByUsers { get; set; }
        // List of users that currently logged in user has advised
        public ICollection<AppUserAdvisor> AdvisedUsers { get; set; }
        
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }     //AppUserRole is acting as our join table
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
    }
}