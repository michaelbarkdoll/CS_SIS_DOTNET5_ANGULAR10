using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public CoursesRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void Update(AppUser user)
        {
            // Lets EF update and add a flag to the entity to let it know that yep thats been modified
            context.Entry(user).State = EntityState.Modified;
        }

        // public async Task<IEnumerable<Course>> GetCoursesAsync()
        // {
        //     return await context.Courses
        //         // .Where(x => x.UserName == username)
        //         // .ProjectTo<MemberDto>(mapper.ConfigurationProvider) // Use automapper
        //         // .SingleOrDefaultAsync();  // This is where we exec database query
        // }

        public async Task<PagedList<CourseDto>> GetCoursesPagedAsync(UserParams userParams)
        {
            var query = context.Courses.AsQueryable();
                            
            // Filter first
            // query = query.Where(u => u.UserName != userParams.CurrentUserName);
            // query = query.Where(u => u.Gender == userParams.Gender);

            // var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            // var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            // query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                // _ => query.OrderBy(u => u.Id)
                _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap to MemberDto
            return await PagedList<CourseDto>.CreateAsync(query.ProjectTo<CourseDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }


        public void AddCourse(Course course)
        {
            this.context.Courses.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            this.context.Courses.Remove(course);
        }

        public async Task<PagedList<SemesterDto>> GetSemestersPagedAsync(UserParams userParams)
        {
            var query = context.Semesters.AsQueryable();

            // Filter first
            if(!String.IsNullOrWhiteSpace(userParams.SearchYear)
                && !userParams.SearchYear.Equals("All")) 
            {
                if(Int32.TryParse(userParams.SearchYear, out int searchYear)) {
                    query = query.Where(u => u.Year == searchYear);
                }
            }

            if(!String.IsNullOrWhiteSpace(userParams.SearchTerm) && !userParams.SearchTerm.Equals("All")) 
            {
                query = query.Where(u => u.Term == userParams.SearchTerm);
            }
            
            // query = query.Where(u => u.Gender == userParams.Gender);

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                _ => query.OrderBy(u => u.Id)
                // _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap to MemberDto
            return await PagedList<SemesterDto>.CreateAsync(query.ProjectTo<SemesterDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }

        public void AddSemester(Semester semester)
        {
            this.context.Semesters.Add(semester);
        }

        public void DeleteSemester(Semester semester)
        {
            this.context.Semesters.Remove(semester);
        }

        public async Task<Semester> GetSemesterAsync(int id)
        {
            return await this.context.Semesters
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await this.context.Courses
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        /*
        var query = context.Printers.AsQueryable();
            
        return await context.Printers
                .ProjectTo<PrinterDto>(mapper.ConfigurationProvider)
                .ToListAsync();  // This is where we exec database query
        */
        public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
        {
            return await this.context.Courses
                .ProjectTo<CourseDto>(mapper.ConfigurationProvider)
                .ToListAsync();  // This is where we exec database query
        }

        public async Task<IEnumerable<SemesterDto>> GetSemestersAsync()
        {
            return await this.context.Semesters
                .ProjectTo<SemesterDto>(mapper.ConfigurationProvider)
                .ToListAsync();  // This is where we exec database query
        }

        public async Task<PagedList<UserFileDto>> GetPaginatedUserFileClasslistBySemesterIdAsync(AppUser user, int semesterId, UserParams userParams)
        {
            // return await this.context.Semesters

            // return await this.context.Semesters
            //     .ProjectTo<SemesterDto>(mapper.ConfigurationProvider)
            //     .ToListAsync();  // This is where we exec database query

            var query = context.UserFiles.AsQueryable();

            // Filter first
            query = query.Where(u => u.AppUserId == user.Id);
            query = query.Where(u => u.semesterId == semesterId);

            // if(Int32.TryParse(semesterId, out int filterId)) {
            //     query = query.Where(u => u.semesterId == filterId);
            // }
            
            // if(!String.IsNullOrWhiteSpace(userParams.SearchYear)
            //     && !userParams.SearchYear.Equals("All")) 
            // {
            //     if(Int32.TryParse(userParams.SearchYear, out int searchYear)) {
            //         query = query.Where(u => u.Year == searchYear);
            //     }
            // }

            // if(!String.IsNullOrWhiteSpace(userParams.SearchTerm) && !userParams.SearchTerm.Equals("All")) 
            // {
            //     query = query.Where(u => u.Term == userParams.SearchTerm);
            // }
            
            // query = query.Where(u => u.Gender == userParams.Gender);

            query = userParams.OrderBy switch // New C# 8 switch expressions, no need for breaks 
            {
                // "Pending" => query.OrderBy(u => u.JobStatus),   // created case
                // "Held" => query.OrderBy(u => u.JobStatus),   // created case
                // "Completed" => query.OrderBy(u => u.JobStatus),   // created case
                _ => query.OrderBy(u => u.Id)
                // _ => query.OrderByDescending(u => u.Id)     // Default case (oldest first)
            };

            // Project Automap to MemberDto
            return await PagedList<UserFileDto>.CreateAsync(query.ProjectTo<UserFileDto>(mapper
                .ConfigurationProvider).AsNoTracking(), 
                    userParams.PageNumber, userParams.PageSize);
        }

        public async Task<SemesterDto> GetSemesterByIdAsync(int id)
        {
            var semester = await this.context.Semesters
                .ProjectTo<SemesterDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            return semester;
        }

        public void ProcessClassList(UserFile userFile, SemesterDto semester) {

            // System.Console.WriteLine($"userFile StorageFileName: {userFile.StorageFileName} semester id: {semester.Id}");
            
            //using (var reader = new StreamReader("path\\to\\file.csv"))
            using (var reader = new StreamReader(userFile.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                var records = csv.GetRecords<ClassListDto>();

                foreach(var record in records.ToList()) {
                    var props = record.GetType().GetProperties();
                    // var sb = new StringBuilder();
                    foreach (var p in props)
                    {
                        System.Console.WriteLine(p.Name + ": " + p.GetValue(record, null));
                        // sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
                    }

                    System.Console.WriteLine("!!!" + record.MAJOR);
                    // return sb.ToString();
                }

            }
        }
    }

}