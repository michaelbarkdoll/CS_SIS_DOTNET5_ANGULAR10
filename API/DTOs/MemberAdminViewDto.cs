using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class MemberAdminViewDto
    {
        public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        public string Username { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int DawgTag { get; set; }
        public int PageQuota { get; set; }
        public int TotalPagesPrinted { get; set; }
        public string PrimaryMajor { get; set; }
        public string SecondaryMajor { get; set; }
        public bool AccessPermitted { get; set; }
        public ICollection<PrintJob> PrintJobs { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserLike> LikedByUsers { get; set; }
        // List of users that currently logged in user has liked
        public ICollection<UserLike> LikedUsers { get; set; }
        // List of users that advised currently logged in user
        public ICollection<AppUserAdvisor> AdvisedByUsers { get; set; }
        // List of users that currently logged in user has advised
        public ICollection<AppUserAdvisor> AdvisedUsers { get; set; }
        public ICollection<UserFile> UserFiles { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}