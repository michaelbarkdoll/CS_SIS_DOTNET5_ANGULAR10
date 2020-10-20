using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRespistory : ILikesRespository
    {
        private readonly DataContext context;
        public LikesRespistory(DataContext context)
        {
            this.context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await this.context.Likes.FindAsync(sourceUserId, likedUserId);       // Search based off primary key
        }

        //public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            // Use Linq AsQueryable
            var users = this.context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = this.context.Likes.AsQueryable();

            // Current users outgoing liked list
            // List of all users that the currently logged in user has liked
            if(likesParams.Predicate == "liked") {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            // Current user likes obtained
            // List of all users that have liked the currently logged in user 
            if(likesParams.Predicate == "likedBy") {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            // here we dont use automapper
            // return await users.Select(user => new LikeDto
            // {
            //     Username = user.UserName,
            //     KnownAs = user.KnownAs,
            //     Age = user.DateOfBirth.CalculateAge(),
            //     PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
            //     City = user.City,
            //     Id = user.Id
            // }).ToListAsync();

            var likedUsers = users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await this.context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}