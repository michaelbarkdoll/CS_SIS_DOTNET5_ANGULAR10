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
    public class PrinterRepository : IPrinterRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public PrinterRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<MemberPrintJobDto> GetMemberPrintJobsAsync(string username)
        {
            return await context.Users
                .Where(x => x.UserName == username)
                .Include(p => p.PrintJobs)
                .Include(p => p.Photos)
                .ProjectTo<MemberPrintJobDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync();  // This is where we exec database query
        }

        public async Task<IEnumerable<PrinterDto>> GetPrintersAsync()
        {
            var query = context.Printers.AsQueryable();

            return await context.Printers
                 .ProjectTo<PrinterDto>(mapper.ConfigurationProvider)
                 .ToListAsync();  // This is where we exec database query
        }

        public async Task<MemberPrintQuotaDto> GetMemberPrintQuotaAsync(int id) {
            return await context.Users
                .Include(p => p.Photos)
                .Include(p => p.PrintJobs)
                .ProjectTo<MemberPrintQuotaDto>(mapper.ConfigurationProvider) // Use automapper
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByIdPrintJobAsync(int id)
        {
            return await context.Users
                .Include(p => p.Photos)
                .Include(p => p.PrintJobs)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByUsernamePrintJobAsync(string username)
        {
            return await context.Users
                .Include(p => p.PrintJobs)
                .Include(p => p.Photos)     // Eager loading
                .SingleOrDefaultAsync(x => x.UserName == username);
        }


        public async Task<PagedList<PrintJobDto>> GetMemberPrintJobsAsync(UserParams userParams)
        {
            var query = context.PrintJobs.AsQueryable();

            // // Filter first
            query = query.Where(u => u.AppUser.UserName.Equals(userParams.CurrentUserName)); // Compare AppUser.Username to Token value passed in
            // query = query.Where(u => u.JobStatus == userParams.PrintStatus);
            // query = query.Where(u => u.UserName != userParams.CurrentUserName);
            // query = query.Where(u => u.Gender == userParams.Gender);

            // var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            // var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            // query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);


            query = userParams.PrintStatus switch // New C# 8 switch expressions, no need for breaks 
            {
                "Held" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Queued" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Cancelled" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Completed" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
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
            return await PagedList<PrintJobDto>.CreateAsync(query.ProjectTo<PrintJobDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<PrintJobDto>> GetMembersPrintJobsAsync(UserParams userParams)
        {
            var query = context.PrintJobs.AsQueryable();

            
                            
            // // Filter first
            if(!string.IsNullOrEmpty(userParams.SearchUser)) {
                // query = query.Where(u => u.JobOwner.Equals(userParams.SearchUser.ToString()));
                query = query.Where(u => u.JobOwner.Contains(userParams.SearchUser.ToString()));
            }

            if(!string.IsNullOrEmpty(userParams.SearchPrinter)) {
                query = query.Where(u => u.PrinterName.Contains(userParams.SearchPrinter.ToString()));
            }
            
            // query = query.Where(u => u.AppUser.UserName.Equals(userParams.CurrentUserName)); // Compare AppUser.Username to Token value passed in
            // query = query.Where(u => u.JobStatus == userParams.PrintStatus);
            // query = query.Where(u => u.UserName != userParams.CurrentUserName);
            // query = query.Where(u => u.Gender == userParams.Gender);

            // var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            // var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            // query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);


            query = userParams.PrintStatus switch // New C# 8 switch expressions, no need for breaks 
            {
                "Held" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Queued" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Cancelled" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Completed" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
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
            return await PagedList<PrintJobDto>.CreateAsync(query.ProjectTo<PrintJobDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);


            // return await context.Users
            //     .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            //     .ToListAsync();  // This is where we exec database query

            //throw new System.NotImplementedException();
        }

        public async Task<PagedList<PrinterDto>> GetPagedPrintersAsync(UserParams userParams)
        {
            // var query = context.PrintJobs.AsQueryable();
            var query = context.Printers.AsQueryable();

            // Project Automap to MemberDto
            return await PagedList<PrinterDto>.CreateAsync(query.ProjectTo<PrinterDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);

            // return await context.Printers
            //      .ProjectTo<PrinterDto>(mapper.ConfigurationProvider)
            //      .ToListAsync();  // This is where we exec database query

            /*
            // // Filter first
            if(!string.IsNullOrEmpty(userParams.SearchUser)) {
                query = query.Where(u => u.JobOwner.Contains(userParams.SearchUser.ToString()));
            }

            if(!string.IsNullOrEmpty(userParams.SearchPrinter)) {
                query = query.Where(u => u.PrinterName.Contains(userParams.SearchPrinter.ToString()));
            }

            query = userParams.PrintStatus switch // New C# 8 switch expressions, no need for breaks 
            {
                "Held" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Queued" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Cancelled" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                "Completed" => query.Where(u => u.JobStatus.Equals(userParams.PrintStatus)),   // created case
                _ => query.OrderBy(u => u.Id)               // Default case (show everything)
            };

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // _ => query.OrderBy(u => u.Id)
                _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap to MemberDto
            return await PagedList<PrintJobDto>.CreateAsync(query.ProjectTo<PrintJobDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);

            */

        }
    }
}