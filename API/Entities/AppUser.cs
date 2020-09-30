namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        public string UserName { get; set; }    // ASP.NET Core Identity use a username with uppercase 'UserName' so we stick with convention.

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
    }
}