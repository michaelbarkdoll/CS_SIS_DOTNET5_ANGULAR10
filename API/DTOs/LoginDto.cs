namespace API.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Factor { get; set; }
        public string Passcode { get; set; }
    }
}