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
        // Task<PagedList<MemberAdminRoleView>> GetUsersPaginatedRoleList(UserParams userParams);
    }
}