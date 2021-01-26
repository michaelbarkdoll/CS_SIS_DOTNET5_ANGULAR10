using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
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

        public async Task<PagedList<UserContainerDto>> GetMemberContainerJobsAsync(UserParams userParams)
        {
            // var query = context.PrintJobs.AsQueryable();
            var query = context.UserContainers.AsQueryable();

            // // Filter first
            query = query.Where(u => u.AppUserId == userParams.SearchUserID);

            // if(!string.IsNullOrEmpty(userParams.SearchUser)) {
            //     query = query.Where(u => u.JobOwner.Contains(userParams.SearchUser.ToString()));
            // }

            // if(!string.IsNullOrEmpty(userParams.SearchPrinter)) {
            //     query = query.Where(u => u.PrinterName.Contains(userParams.SearchPrinter.ToString()));
            // }
            
            // query = query.Where(u => u.AppUser.UserName.Equals(userParams.CurrentUserName)); // Compare AppUser.Username to Token value passed in
            // query = query.Where(u => u.JobStatus == userParams.PrintStatus);
            // query = query.Where(u => u.UserName != userParams.CurrentUserName);
            // query = query.Where(u => u.Gender == userParams.Gender);

            // var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            // var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            // query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);


            query = userParams.JobStatus switch // New C# 8 switch expressions, no need for breaks 
            {
                "Held" => query.Where(u => u.ContainerStatus.Equals(userParams.JobStatus)),   // created case
                "Queued" => query.Where(u => u.ContainerStatus.Equals(userParams.JobStatus)),   // created case
                "Cancelled" => query.Where(u => u.ContainerStatus.Equals(userParams.JobStatus)),   // created case
                "Completed" => query.Where(u => u.ContainerStatus.Equals(userParams.JobStatus)),   // created case
                _ => query.OrderBy(u => u.Id)               // Default case (show everything)
            };

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                // _ => query.OrderBy(u => u.Id)
                _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap to MemberDto
            return await PagedList<UserContainerDto>.CreateAsync(query.ProjectTo<UserContainerDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }
    }
}