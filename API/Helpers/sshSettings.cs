namespace API.Helpers
{
    public class sshSettings
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        
        public sshSettings()
        {
            
        }
        public sshSettings(string hostname, int port)
        {
            this.Port = port;
            this.Hostname = hostname;
            
        }
    }
}