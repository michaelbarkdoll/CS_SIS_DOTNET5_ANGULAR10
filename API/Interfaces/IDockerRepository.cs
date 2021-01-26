using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IDockerRepository
    {
        Task<MemberContainerDto> GetMemberContainersAsync(int id);
        void DeleteContainer(UserContainer container);
        Task<UserContainer> GetContainerAsync(int id);
        // Task<UserContainerDto> CreateMemberContainersAsync(string username, UserContainerDto userContainerDto);

        Task<PagedList<UserContainerDto>> GetMemberContainerJobsAsync(UserParams userParams);
    }
}