using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public LikesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

        }
        // api/likes/{username}
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await this.unitOfWork.LikesRespository.GetUserWithLikes(sourceUserId);

            if (likedUser == null)
                return NotFound();
            if (sourceUser.UserName == username)
                return BadRequest("You cannot like yourself");

            var userLike = await this.unitOfWork.LikesRespository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null)
                return BadRequest("You've already liked this user");
            // Could add a toggle here to remove the like
            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await this.unitOfWork.Complete())
            {
                //Unit of Work
                return Ok();
            }

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await this.unitOfWork.LikesRespository.GetUserLikes(likesParams);
            //var users = await this.unitOfWork.LikesRespository.GetUserLikes(predicate, User.GetUserId());

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}