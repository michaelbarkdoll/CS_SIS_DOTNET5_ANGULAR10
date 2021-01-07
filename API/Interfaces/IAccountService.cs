using System.Threading.Tasks;
using Renci.SshNet;

namespace API.Interfaces
{
    public interface IAccountService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<bool> AuthenticateUserTwoFactorAsync(string username, string factor, string passcode);
        Task<bool> IssueUserKeysAsync(string username, string publicKey, string privateKey);
        Task<bool> IssueUserWebKeysAsync(string username, string publicKey, string privateKey);
        Task<int> RunCommandAsync(string command);
        Task<int> RunClientCommandAsync(SshClient client, string command);
    }
}