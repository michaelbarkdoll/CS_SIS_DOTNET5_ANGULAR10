using API.Helpers;
using API.Interfaces;
using DuoSecurity.Auth.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly sshSettings sshServer;
        private SshClient client;
        private readonly DuoAuthConfig duoAuthConfig;

        public AccountService(IOptions<sshSettings> config, IOptions<DuoSettings> duoConfig)
        {
            this.sshServer = new sshSettings(config.Value.Hostname, config.Value.Port, 
                                    config.Value.Www2host, config.Value.Www2port,
                                    config.Value.Www2user, config.Value.Www2passwd,
                                    config.Value.Pc00host, config.Value.Pc00port,
                                    config.Value.Pc00user, config.Value.Pc00passwd,
                                    config.Value.Pc01host, config.Value.Pc01port,
                                    config.Value.Pc01user, config.Value.Pc01passwd);
            this.duoAuthConfig = new DuoAuthConfig(duoConfig.Value.Host, duoConfig.Value.IntegrationKey, duoConfig.Value.SecretKey);
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password) {         
            this.client = this.setupConnection(sshServer.Hostname, sshServer.Port, username, password);

            var retCode = await this.RunCommandAsync("id");

            this.client.Disconnect();
            
            if (retCode != 0)
                return false;

            return true;
        }

        public async Task<bool> AuthenticateUserTwoFactorAsync(string username, string factor, string passcode) {
            dynamic stuff;
            // var config = new DuoAuthConfig("host.com", "integrationKey", "secretKey");

            // Instantiate Client
            // var client = new DuoAuthClient(config);
            var client = new DuoAuthClient(duoAuthConfig);
            

            /*
            // Check that Duo is online...
            var response = await client.PingAsync();
            try {
                //{"response": {"time": 1608327739}, "stat": "OK"}
                dynamic stuff = JsonConvert.DeserializeObject(response.OriginalJson);
                string status = stuff.stat;

                if(status.Equals("OK")) {
                    // Proceed only if server is up here...
                }
            }
            catch(Exception ex) {

            }
            */
            if(!String.IsNullOrEmpty(factor)) {
                if(factor.Equals("Push")) {
                    var auth = await client.AuthPushByUsernameAsync(username);
                    stuff = JsonConvert.DeserializeObject(auth.OriginalJson);
                    if(validTwoFactor(stuff))
                        return true;
                }
                if(factor.Equals("Phone")) {
                    var auth = await client.AuthPhoneByUsernameAsync(username);
                    stuff = JsonConvert.DeserializeObject(auth.OriginalJson);
                    if(validTwoFactor(stuff))
                        return true;
                }
                if(factor.Equals("Passcode")) {
                    var auth = await client.AuthPasscodeByUsernameAsync(username, passcode);
                    stuff = JsonConvert.DeserializeObject(auth.OriginalJson);
                    if(validTwoFactor(stuff))
                        return true;
                }
                if(factor.Equals("SMS")) {
                    var auth = await client.AuthSmsByUsernameAsync(username);
                    return false;
                }
            }

            

            return false;

        }
        public bool validTwoFactor(dynamic stuff) {
            try {
                // {"response": {"result": "deny", "status": "deny", "status_msg": "Incorrect passcode. Please try again."}, "stat": "OK"}
                // dynamic stuff = JsonConvert.DeserializeObject(auth1.OriginalJson);
                string status = stuff.response.status;

                if(status.Equals("allow")) {
                    // System.Console.WriteLine("*********************************************");
                    // System.Console.WriteLine(status);    // Prints "allow"
                    // System.Console.WriteLine("*********************************************");
                    return true;
                }
                if(status.Equals("deny")) {
                    System.Console.WriteLine(status);   // Prints "deny"
                    return false;
                }
                
            }
            catch(Exception ex) {

            }
            return false;
        }

        public async Task<bool> IssueUserKeysAsync(string username, string publicKey, string privateKey) {  

            var client = this.setupConnection(sshServer.Pc00host, sshServer.Pc00port, sshServer.Pc00user, sshServer.Pc00passwd);
            // this.client = this.setupConnection(sshServer.Hostname, sshServer.Port, username, password);

            System.Console.WriteLine($"{username} {publicKey} {privateKey}");
            string command = $"sudo -H -u {username} bash -c 'mkdir -p ~/.ssh; echo \"{publicKey}\" > ~/.ssh/authorized_keys; chmod 644 ~/.ssh/authorized_keys; echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
            var retCode = await this.RunClientCommandAsync(client, command);

            client.Disconnect();
            
            if (retCode != 0)
                return false;

            client = this.setupConnection(sshServer.Pc01host, sshServer.Pc01port, sshServer.Pc01user, sshServer.Pc01passwd);
            // this.client = this.setupConnection(sshServer.Hostname, sshServer.Port, username, password);

            System.Console.WriteLine($"{username} {publicKey} {privateKey}");
            command = $"sudo -H -u {username} bash -c 'mkdir -p ~/.ssh; echo \"{publicKey}\" > ~/.ssh/authorized_keys; chmod 644 ~/.ssh/authorized_keys; echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
            retCode = await this.RunClientCommandAsync(client, command);

            client.Disconnect();
            
            if (retCode != 0)
                return false;

            return true;
        }
        public async Task<bool> IssueUserWebKeysAsync(string username, string publicKey, string privateKey) {  

            var client = this.setupConnection(sshServer.Www2host, sshServer.Www2port, sshServer.Www2user, sshServer.Www2passwd);
            // this.client = this.setupConnection(sshServer.Hostname, sshServer.Port, username, password);

            // var retCode = await this.RunCommandAsync("id");
            // var retCode = await this.RunCommandAsync("sudo -H -u siu851164679 bash -c 'echo \"test\" > ~/test'");
            System.Console.WriteLine($"{username} {publicKey} {privateKey}");
            // string command = $"sudo -H -u {username} bash -c 'echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";

            // string command = $"sudo -H -u {username} bash -c 'mkdir -p ~/.ssh; echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
            // string command = $"sudo -H -u {username} bash -c 'mkdir -p ~/.ssh; echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
            string command = $"sudo -H -u {username} bash -c 'mkdir -p ~/.ssh; echo \"{publicKey}\" > ~/.ssh/authorized_keys; chmod 644 ~/.ssh/authorized_keys; echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'";
            // var retCode = await this.RunClientCommandAsync(client, $"sudo -H -u {username} bash -c 'echo \"{publicKey}\" > ~/.ssh/id_rsa.pub; chmod 644 ~/.ssh/id_rsa.pub; echo \"{privateKey}\" > ~/.ssh/id_rsa; chmod 600 ~/.ssh/id_rsa'");
            var retCode = await this.RunClientCommandAsync(client, command);

            client.Disconnect();
            
            if (retCode != 0)
                return false;

            return true;
        }

        public async Task<int> RunClientCommandAsync(SshClient client, string command) {
            // System.Console.WriteLine(command);
            int retCode = -1;
            try {
                using (var cmd = client.CreateCommand(command))
                {
                    var cmdResult = await Task.FromResult<string>(cmd.Execute());
                    
                    // Console.WriteLine("Command>" + cmd.CommandText);
                    // Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                    // Console.WriteLine("Result Value = {0}", cmd.Result);
                    // if (cmd.ExitStatus != 0)
                    // {
                    //     Console.WriteLine("Command failed!!!!");
                    // }

                    retCode = cmd.ExitStatus;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception");
                Console.WriteLine(ex);
            }
            finally {
            }

            return retCode;
            // return Task.FromResult<int>(retCode);
        }

        public async Task<int> RunCommandAsync(string command) {
            int retCode = -1;
            try {
                using (var cmd = this.client.CreateCommand(command))
                {
                    var cmdResult = await Task.FromResult<string>(cmd.Execute());
                    
                    // Console.WriteLine("Command>" + cmd.CommandText);
                    // Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                    // Console.WriteLine("Result Value = {0}", cmd.Result);
                    // if (cmd.ExitStatus != 0)
                    // {
                    //     Console.WriteLine("Command failed!!!!");
                    // }

                    retCode = cmd.ExitStatus;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception");
                Console.WriteLine(ex);
            }
            finally {
            }

            return retCode;
            // return Task.FromResult<int>(retCode);
        }

        // public async Task<SshClient> setupConnectionAsync(string host, int port, string username, string password)
        public SshClient setupConnection(string host, int port, string username, string password)
        {
            ConnectionInfo ConnNfo = new ConnectionInfo(host, port, username,
                   new AuthenticationMethod[]{
                        // Pasword based Authentication
                        new PasswordAuthenticationMethod(username, password),

                        /*
                        // Key Based Authentication (using keys in OpenSSH Format)
                        new PrivateKeyAuthenticationMethod("username",new PrivateKeyFile[]{
                            new PrivateKeyFile(@"..\openssh.key","passphrase")
                        }), 
                        */
                    }
                );

            var sshclient = new SshClient(ConnNfo);

            try
            {
                sshclient.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception");
                Console.WriteLine(ex);
            }
            
            // return Task.FromResult<SshClient>(sshclient);
            return sshclient;

        }
    }
}