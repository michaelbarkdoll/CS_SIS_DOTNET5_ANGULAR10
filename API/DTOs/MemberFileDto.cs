using System;
using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class MemberFileDto
    {
        public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        public string Username { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.
        public string PhotoUrl { get; set; }

        //public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }
        //public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public ICollection<UserFileDto> UserFiles { get; set; }
    }
}