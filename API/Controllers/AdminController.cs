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
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IFileRepoService fileRepoService;
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IFileRepoService fileRepoService)
        {
            this.fileRepoService = fileRepoService;
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
        [HttpGet("get-semester-classlist-files-paged", Name = "GetUserFilePagedClasslist")]
        // public async Task<ActionResult<IEnumerable<UserFileDto>>> GetUserFilePagedClasslist(string semesterId, [FromQuery] UserParams userParams) 
        public async Task<ActionResult<IEnumerable<UserFileDto>>> GetSemesterClasslistFilesPaged([FromQuery] UserParams userParams) 
        {
            var username = User.GetUsername();    // From Token
            var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var semesterId = userParams.SemesterId;

            var userFile = await this.unitOfWork.CoursesRepository.GetPaginatedUserFileClasslistBySemesterIdAsync(user, semesterId, userParams);

            Response.AddPaginationHeader(userFile.CurrentPage, userFile.PageSize, userFile.TotalCount, userFile.TotalPages);

            return Ok(userFile);
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

        [Authorize(Roles = "Admin")]
        [HttpPost("add-user-file-classlist")]
        public async Task<ActionResult<UserFileDto>> AddUserFileClasslist(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var fileresult = await fileRepoService.AddFileAsync(file);

            if (fileresult == null)
                return BadRequest("Unable to upload file");

            fileresult.isClassList = true;

            // user.UserFiles.Add(fileresult);

            if (user.UserFiles == null)
            {
                user.UserFiles = new List<UserFile>();
            }

            if (user.UserFiles != null) {
                user.UserFiles.Add(fileresult);
                // this.unitOfWork.UserRepository.Update(user);
                // System.Console.WriteLine("COUNT: " + user.UserFiles.Count().ToString());
            }
                

            

            // Submit printjob to cups here
            //SubmitToCups(fileresult)

            // Process classlist

            if (await unitOfWork.Complete())
            {
                return CreatedAtRoute("AdminGetUserNameFiles", new { username = user.UserName }, mapper.Map<UserFileDto>(fileresult));
                // return CreatedAtRoute("GetUserNameFiles", new { username = user.UserName }, mapper.Map<UserFileDto>(fileresult));
            }

            return BadRequest("Problem adding userfile");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("process-user-file-classlist/{publicId}")]
        // public async Task<ActionResult> ProcessUserFileClasslist(string publicId, SemesterDto semester)
        // public async Task<ActionResult> ProcessUserFileClasslist(string publicId, int semesterId)
        //[FromQuery] string roles
        public async Task<ActionResult> ProcessUserFileClasslist(string publicId, [FromQuery] int semesterId)
        // public async Task<ActionResult> ProcessUserFileClasslist(UserFileDto userFileDto)
        {
            // System.Console.WriteLine($"public Id: {publicId} semesterID: {semesterId}");

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            // if (user.UserFiles == null)
            // {
            //     // user.UserFiles = new List<UserFile>();
            //      System.Console.WriteLine("!!!!!Something is wrong!!!!!");
            //      return BadRequest();
            // }
            // System.Console.WriteLine("COUNT2: " + user.UserFiles.Count().ToString());

            var userFile = await unitOfWork.AdminRepository.GetUserFileAsync(user, publicId);
            if (userFile.Equals(null))
            {
                return BadRequest();
            }
            
            var semester = await unitOfWork.CoursesRepository.GetSemesterByIdAsync(semesterId);
            if (semester.Equals(null))
            {
                return BadRequest();
            }

            userFile.semesterId = semesterId;
            await unitOfWork.Complete();

            unitOfWork.CoursesRepository.ProcessClassList(userFile, semester);

            return Ok();
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("batch-add-user-file-classlist/{publicId}")]
        public async Task<ActionResult> BatchProcessUserFileClasslist(string publicId, [FromQuery] int semesterId)
        {

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var userFile = await unitOfWork.AdminRepository.GetUserFileAsync(user, publicId);
            if (userFile.Equals(null))
            {
                return BadRequest();
            }
            
            var semester = await unitOfWork.CoursesRepository.GetSemesterByIdAsync(semesterId);
            if (semester.Equals(null))
            {
                return BadRequest();
            }

            if(!await unitOfWork.AdminRepository.BatchCreateUsersFromCSV(userFile, semester))
            {
                return BadRequest();
            }

            await unitOfWork.Complete();

            return Ok();
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("batch-allow-users-login-from-classlist/{publicId}")]
        public async Task<ActionResult> BatchAllowUsersLoginFromClasslist(string publicId)
        {

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var userFile = await unitOfWork.AdminRepository.GetUserFileAsync(user, publicId);
            if (userFile.Equals(null))
            {
                return BadRequest();
            }

            if(!await unitOfWork.AdminRepository.BatchAllowLoginFromCSV(userFile))
            {
                return BadRequest();
            }   

            await unitOfWork.Complete();

            return Ok();
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("batch-disable-users-login-from-classlist/{publicId}")]
        public async Task<ActionResult> BatchDisableUsersLoginFromClasslist(string publicId)
        {

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var userFile = await unitOfWork.AdminRepository.GetUserFileAsync(user, publicId);
            if (userFile.Equals(null))
            {
                return BadRequest();
            }

            if(!await unitOfWork.AdminRepository.BatchDisableLoginFromCSV(userFile))
            {
                return BadRequest();
            }   

            if (await this.unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to update user.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("batch-update-majors-from-classlist/{publicId}")]
        public async Task<ActionResult> BatchUpdateMajorsFromClasslist(string publicId)
        {

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var userFile = await unitOfWork.AdminRepository.GetUserFileAsync(user, publicId);
            if (userFile.Equals(null))
            {
                return BadRequest();
            }

            if(!await unitOfWork.AdminRepository.BatchUpdateMajorFromCSV(userFile)) {
                return BadRequest();
            }

            await unitOfWork.Complete();

            return Ok();
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user-files/{username}", Name = "AdminGetUserNameFiles")]
        public async Task<ActionResult<MemberFileDto>> GetUserNameFiles(string username) 
        {
            //var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            return await this.unitOfWork.UserRepository.GetMemberFilesAsync(username);
        }
    }
}