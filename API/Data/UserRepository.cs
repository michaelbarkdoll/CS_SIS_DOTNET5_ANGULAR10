using System.Collections.Generic;
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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            // return await context.Users
            //     .Where(x => x.UserName == username)
            //     .Select(user => new MemberDto 
            //     {
            //         Id = user.Id,            // Manually map each field... we'll instead use automapper
            //         Username = user.UserName
            //     }).SingleOrDefaultAsync();  // This is where we exec database query

            return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync();  // This is where we exec database query

        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await context.Users
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .ToListAsync();  // This is where we exec database query
            //throw new System.NotImplementedException();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameASync(string username)
        {
            return await context.Users
                .Include(p => p.Photos)     // Eager loading
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(p => p.Photos)     // Eager loading
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;    // Greater than 0 changes
        }

        public void Update(AppUser user)
        {
            // Lets EF update and add a flag to the entity to let it know that yep thats been modified
            context.Entry(user).State = EntityState.Modified;
        }
    }
}