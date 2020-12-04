using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public AdminRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }
/*
        public async Task<PagedList<MemberAdminRoleView>> GetUsersPaginatedRoleList(UserParams userParams)
        {
            
            var query5 = context.Users.AsQueryable();


            List<MemberAdminRoleView> data = new List<MemberAdminRoleView>();

            var query2 = (from a in context.UserRoles
                          join b in context.Users on a.UserId equals b.Id
                          select new AppUserRole { User = b, Role = a.Role });
            
            

            var query = context.UserRoles.AsQueryable();


            if(userParams.SearchUserID != 0) {
                query = query.Where(u => u.UserId.Equals(userParams.SearchUserID));
            }

            query2 = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                // _ => query.OrderBy(u => u.Id)
                // _ => query.OrderByDescending(u => u.UserId)     // Default case (oldest first)
                _ => query2.OrderByDescending(u => u.User.Id)     // Default case (oldest first)
            };

            query5 = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                // _ => query.OrderBy(u => u.Id)
                // _ => query.OrderByDescending(u => u.UserId)     // Default case (oldest first)
                _ => query5.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };


            foreach (var user in query2) {
                System.Console.WriteLine($"{user.User.Id} {user.User.UserName} {user.Role.Name} {user.Role.Id}");
                data.Add(new MemberAdminRoleView(user.User.Id, user.Role.Id, user.User.UserName));
            }

            var query3 = data.AsQueryable();


            //Working
            // return await PagedList<MemberAdminRoleView>.CreateAsync(query2.ProjectTo<MemberAdminRoleView>(mapper
            //     .ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);


            return await PagedList<MemberAdminRoleView>.CreateAsync(query5.ProjectTo<MemberAdminRoleView>(mapper
                .ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }
*/
        public async Task<PagedList<MemberAdminViewDto>> GetUsersPaginatedAccessList(UserParams userParams)
        {
            var query = context.Users.AsQueryable();

            // // Filter first
            if(!string.IsNullOrEmpty(userParams.SearchUser)) {
                query = query.Where(u => u.UserName.Contains(userParams.SearchUser.ToString()));
            }

            // query = userParams.PrintStatus switch // New C# 8 switch expressions, no need for breaks 
            // {
            //     "Held" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     "Queued" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     "Cancelled" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     "Completed" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
            //     _ => query.OrderBy(u => u.Id)               // Default case (show everything)
            // };
            
            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                // _ => query.OrderBy(u => u.Id)
                _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap AppUser to MemberAdminViewDto
            return await PagedList<MemberAdminViewDto>.CreateAsync(query.ProjectTo<MemberAdminViewDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }

        
    }
}