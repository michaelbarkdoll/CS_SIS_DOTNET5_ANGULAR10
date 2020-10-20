using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRespository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        // List of users that have "been liked" or "liked by"
        //Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}