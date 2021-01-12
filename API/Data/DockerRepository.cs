using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DockerRepository : IDockerRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public DockerRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void DeleteContainer(UserContainer container)
        {
            this.context.UserContainers.Remove(container);
        }

        public async Task<UserContainer> GetContainerAsync(int id)
        {
            return await this.context.UserContainers
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MemberContainerDto> GetMemberContainersAsync(int id)
        {
            return await context.Users
                .Where(x => x.Id == id)
                .Include(p => p.UserContainers)
                // .Include(p => p.PrintJobs)
                .ProjectTo<MemberContainerDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync();  // This is where we exec database query
                // .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}