using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IDockerRepository
    {
        Task<MemberContainerDto> GetMemberContainersAsync(int id);
        void DeleteContainer(UserContainer container);
        Task<UserContainer> GetContainerAsync(int id);
        // Task<UserContainerDto> CreateMemberContainersAsync(string username, UserContainerDto userContainerDto);
    }
}