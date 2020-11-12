using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAccountService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<int> RunCommandAsync(string command);
    }
}