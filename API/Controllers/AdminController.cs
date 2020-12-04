using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Helpers;
using API.Extensions;
using API.Interfaces;
using API.DTOs;
using AutoMapper;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            //return Ok("Only admins can see this");
            var users = await this.userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    u.AccessPermitted,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        // [Authorize(Policy = "RequireAdminRole")]
        // [HttpGet("users-paged-with-roles")]
        // public async Task<ActionResult<MemberAdminRoleView>> GetUsersPagedWithRoles([FromQuery]UserParams userParams)
        // {
        //     var usersRoleList = await this.unitOfWork.AdminRepository.GetUsersPaginatedRoleList(userParams);

        //     Response.AddPaginationHeader(usersRoleList.CurrentPage, usersRoleList.PageSize, usersRoleList.TotalCount, usersRoleList.TotalPages);

        //     return Ok(usersRoleList);
        // }

        // [Authorize(Policy = "RequireAdminRole")]
        // [HttpGet("users-paged-with-role-id")]
        // public async Task<ActionResult<MemberAdminRoleView>> GetUsersPagedWithRoleId([FromQuery]UserParams userParams)
        // {
        //     var usersRoleList = await this.unitOfWork.AdminRepository.GetUsersPaginatedRoleList(userParams);

        //     Response.AddPaginationHeader(usersRoleList.CurrentPage, usersRoleList.PageSize, usersRoleList.TotalCount, usersRoleList.TotalPages);

        //     return Ok(usersRoleList);
        // }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await this.userManager.FindByNameAsync(username);
            var userRoles = await this.userManager.GetRolesAsync(user);

            if (user == null)
                return NotFound("Could not find user");

            var result = await this.userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await this.userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove from roles");

            return Ok(await this.userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-users-paged-accesslist", Name = "GetUsersPagedAccessList")]
        public async Task<ActionResult<MemberAdminViewDto>> GetUsersPagedAccessList([FromQuery] UserParams userParams)
        {
            userParams.CurrentUserName = User.GetUsername();    // From Token

            var userAccessList = await this.unitOfWork.AdminRepository.GetUsersPaginatedAccessList(userParams);

            Response.AddPaginationHeader(userAccessList.CurrentPage, userAccessList.PageSize, userAccessList.TotalCount, userAccessList.TotalPages);

            return Ok(userAccessList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-access")]
        public async Task<ActionResult> UpdateUserAccess(MemberRolesDto memberUpdateDto)
        {
            // Get username From token (can't trust client)
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var username = User.GetUsername();
            var username = memberUpdateDto.Username;

            var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            // Use Automapper to make from memberUpdateDto to AppUser
            this.mapper.Map(memberUpdateDto, user);

            this.unitOfWork.UserRepository.Update(user);

            if (await this.unitOfWork.Complete())
                return NoContent();

            return BadRequest("Failed to update user.");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }
    }
}