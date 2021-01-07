namespace API.Helpers
{
    public class DuoSettings
    {
        public string Host { get; set; }
        public string IntegrationKey { get; set; }
        public string SecretKey { get; set; }
        public DuoSettings()
        {
        }
        public DuoSettings(string host, string integrationKey, string secretKey)
        {
            this.Host = host;
            this.IntegrationKey = integrationKey;
            this.SecretKey = secretKey;
        }

        
    }
}