using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ICoursesRepository
    {
        void Update(AppUser user);  // Only updates EF that something has changed
        Task<PagedList<CourseDto>> GetCoursesPagedAsync(UserParams userParams);
        Task<IEnumerable<CourseDto>> GetCoursesAsync();
        Task<Course> GetCourseAsync(int id);
        void AddCourse(Course course);
        void DeleteCourse(Course course);
        
        Task<PagedList<SemesterDto>> GetSemestersPagedAsync(UserParams userParams);
        Task<IEnumerable<SemesterDto>> GetSemestersAsync();
        Task<SemesterDto> GetSemesterByIdAsync(int id);
        Task<Semester> GetSemesterAsync(int id);
        void AddSemester(Semester semester);
        void DeleteSemester(Semester semester);
        void ProcessClassList(UserFile userFile, SemesterDto semester);

        Task<PagedList<UserFileDto>> GetPaginatedUserFileClasslistBySemesterIdAsync(AppUser user, int semesterId, UserParams userParams);
        

    }
}