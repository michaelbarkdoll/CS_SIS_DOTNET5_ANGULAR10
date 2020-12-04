namespace API.DTOs
{
    public class MemberRolesDto
    {
        public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        public string Username { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.
        public bool AccessPermitted { get; set; }
        
    }
}