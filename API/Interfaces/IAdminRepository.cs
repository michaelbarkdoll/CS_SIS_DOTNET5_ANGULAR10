using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IAdminRepository
    {
        Task<PagedList<MemberAdminViewDto>> GetUsersPaginatedAccessList(UserParams userParams);
        Task<UserFile> GetUserFileAsync(AppUser user, string publicId);
        Task<bool> BatchCreateUsersFromCSV(UserFile userFile, SemesterDto semester);
        Task<bool> BatchAllowLoginFromCSV(UserFile userFile);
        Task<bool> BatchDisableLoginFromCSV(UserFile userFile);
        Task<bool> BatchUpdateMajorFromCSV(UserFile userFile);
    }
}