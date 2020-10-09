using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    // We provide the methods that were going to support from different entitites
    // Repository is an abstraction of Entity Framework
    // Repository allows removing duplicate calls to Entity Framework for the same methods
    public interface IUserRepository
    {
        void Update(AppUser user);  // Only updates EF that something has changed
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMemberAsync(string username);
    }
}