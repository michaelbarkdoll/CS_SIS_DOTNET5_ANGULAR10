namespace API.Helpers
{
    public class sshSettings
    {
        public string Hostname { get; set; }
        public int Port { get; set; }


        public string Www2host { get; set; }
        public int Www2port { get; set; }
        public string Www2user { get; set; }
        public string Www2passwd { get; set; }
        public string Pc00host { get; set; }
        public int Pc00port { get; set; }
        public string Pc00user { get; set; }
        public string Pc00passwd { get; set; }
        public string Pc01host { get; set; }
        public int Pc01port { get; set; }
        public string Pc01user { get; set; }
        public string Pc01passwd { get; set; }

        public sshSettings(string hostname, int port, string www2host, int www2port, string www2user, string www2passwd,
                            string pc00host, int pc00port, string pc00user, string pc00passwd,
                            string pc01host, int pc01port, string pc01user, string pc01passwd)
        {
            this.Pc01passwd = pc01passwd;
            this.Pc01user = pc01user;
            this.Pc01port = pc01port;
            this.Pc01host = pc01host;
            this.Pc00passwd = pc00passwd;
            this.Pc00user = pc00user;
            this.Pc00port = pc00port;
            this.Pc00host = pc00host;
            this.Www2passwd = www2passwd;
            this.Www2user = www2user;
            this.Www2host = www2host;
            this.Www2port = www2port;
            this.Port = port;
            this.Hostname = hostname;

        }

        public sshSettings()
        { }
    }
}