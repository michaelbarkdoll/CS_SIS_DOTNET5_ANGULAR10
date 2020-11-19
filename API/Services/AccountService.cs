using API.Helpers;
using API.Interfaces;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly sshSettings sshServer;
        private SshClient client;

        public AccountService(IOptions<sshSettings> config)
        {
            this.sshServer = new sshSettings(config.Value.Hostname, config.Value.Port);
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password) {         
            this.client = this.setupConnection(sshServer.Hostname, sshServer.Port, username, password);

            var retCode = await this.RunCommandAsync("id");

            this.client.Disconnect();
            
            if (retCode != 0)
                return false;

            return true;
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