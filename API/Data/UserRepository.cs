using System;
using System.Collections.Generic;
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

        //public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();
                            
            // Filter first
            query = query.Where(u => u.UserName != userParams.CurrentUserName);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                "created" => query.OrderByDescending(u => u.Created),   // created case
                _ => query.OrderByDescending(u => u.LastActive)     // Default case
            };

            // Project Automap to MemberDto
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);


            // return await context.Users
            //     .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            //     .ToListAsync();  // This is where we exec database query

            //throw new System.NotImplementedException();
        }

        public async Task<PagedList<MemberDto>> GetMembersURLRequestsAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();
                            
            // Filter first
            // query = query.Where(u => (u.RequestedURL != null) || (u.RequestedURL.ToString() != "") );
            // query = query.Where(u => u.RequestedURL != null);
            query = query.Where(u => !string.IsNullOrEmpty(u.RequestedURL));
            

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                "created" => query.OrderByDescending(u => u.Created),   // created case
                _ => query.OrderByDescending(u => u.LastActive)     // Default case
            };

            // Project Automap to MemberDto
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IEnumerable<PrinterDto>> GetPrintersAsync()
        {
            var query = context.Printers.AsQueryable();
            
            return await context.Printers
                 .ProjectTo<PrinterDto>(mapper.ConfigurationProvider)
                 .ToListAsync();  // This is where we exec database query
        }
        
        // public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        // {
        //     var query = context.Users
        //         .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        //         .AsNoTracking();    // Turns off tracking in EF
        //     return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);

        // }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByIdPrintJobAsync(int id)
        {
            return await context.Users
                .Include(p => p.Photos)
                .Include(p => p.PrintJobs)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .Include(p => p.Photos)     // Eager loading
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<AppUser> GetUserByUsernamePrintJobAsync(string username)
        {
            return await context.Users
                .Include(p => p.PrintJobs)
                .Include(p => p.Photos)     // Eager loading
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(p => p.Photos)     // Eager loading
                .ToListAsync();
        }

        // public async Task<bool> SaveAllAsync()
        // {
        //     return await context.SaveChangesAsync() > 0;    // Greater than 0 changes
        // }

        public void Update(AppUser user)
        {
            // Lets EF update and add a flag to the entity to let it know that yep thats been modified
            context.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberFileDto> GetMemberFilesAsync(string username)
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
                .Include(f => f.UserFiles)
                .ProjectTo<MemberFileDto>(mapper.ConfigurationProvider) // Use automapper
                //.ProjectTo<MemberDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync();  // This is where we exec database query

        }

        public async Task<string> GetUserGender(string username)
        {
            return await this.context.Users
                .Where(x => x.UserName == username)
                .Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public async Task<MemberPrintJobDto> GetMemberPrintJobsAsync(string username)
        {
            return await context.Users
                .Where(x => x.UserName == username)
                .Include(p => p.PrintJobs)
                .Include(p => p.Photos)
                .ProjectTo<MemberPrintJobDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync();  // This is where we exec database query
            // throw new NotImplementedException();
        }

        public async Task<MemberSshKeysDto> GetMemberSshKeysAsync(string username) {
            return await context.Users
                .Where(x => x.UserName == username)
                .Include(p => p.Photos)
                .ProjectTo<MemberSshKeysDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync();  // This is where we exec database query
        }

        public async Task<MemberSshKeysDto> GetMemberSshKeysAsync(int id) {
            return await context.Users
                .Include(p => p.Photos)
                .ProjectTo<MemberSshKeysDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}