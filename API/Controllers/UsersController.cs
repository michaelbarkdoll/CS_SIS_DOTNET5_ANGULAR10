using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // Since UserController derives from BaseApiController which is-a BaseController, then we don't need these attributes,
    // since they've been inherited.
    //[ApiController]
    //[Route("api/[controller]")]

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly IFileRepoService fileRepoService;

        //public UsersController(DataContext context)
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService, IFileRepoService fileRepoService, DataContext context)
        //public UsersController(IunitOfWork.UserRepository unitOfWork.UserRepository, IMapper mapper, IPhotoService photoService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoService = photoService;
            this.fileRepoService = fileRepoService;
            this._context = context;
        }

        // api/users
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        //[AllowAnonymous]
        // public ActionResult<IEnumerable<AppUser>> GetUsers() 
        // {
        //     var users = _context.Users.ToList();

        //     return users;
        // }
        //public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() 
        //public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() 

        // Now it will match user string params in the api
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams) 
        {
            // var users = await this.unitOfWork.UserRepository.GetUsersAsync();
            // //var usersToReturn = this.mapper.Map<IEnumerable<<MapTO>>>(FROM);
            // var usersToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            //var users = await this.unitOfWork.UserRepository.GetMembersAsync();

            // var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var gender = await this.unitOfWork.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUserName = User.GetUsername();    // From Token
            // userParams.CurrentUserName = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = gender == "male" ? "female" : "male";

            var users = await this.unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        //api/users/3
        //[Authorize]
        //[HttpGet("{username}")]
        //[Authorize(Roles = "Member")]
        [HttpGet("{username}", Name = "GetUser")]
        // public ActionResult<AppUser> GetUser(int id) 
        // {
        //     var user = _context.Users.Find(id);

        //     return user;
        // }
        //public async Task<ActionResult<AppUser>> GetUser(string username) 
        public async Task<ActionResult<MemberDto>> GetUser(string username) 
        {
            //var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            return await this.unitOfWork.UserRepository.GetMemberAsync(username);
            
            


            //return await _context.Users.FindAsync(id);
        }

        //[Authorize(Roles = "Member")]
        [HttpGet("user-files", Name = "GetUserByTokenFiles")]
        public async Task<ActionResult<MemberFileDto>> GetUserByTokenFiles() 
        {
            var username = User.GetUsername();
            //var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            return await this.unitOfWork.UserRepository.GetMemberFilesAsync(username);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user-files/{username}", Name = "GetUserNameFiles")]
        public async Task<ActionResult<MemberFileDto>> GetUserNameFiles(string username) 
        {
            //var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            return await this.unitOfWork.UserRepository.GetMemberFilesAsync(username);
        }

        // Update a resource on our server
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // Get username From token (can't trust client)
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();

            var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username); 

            // Use Automapper to make from memberUpdateDto to AppUser
            this.mapper.Map(memberUpdateDto, user);

            this.unitOfWork.UserRepository.Update(user);

            if(await this.unitOfWork.Complete())
                return NoContent();
            
            return BadRequest("Failed to update user.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-url")]
        public async Task<ActionResult> UpdateUserURLRequest(MemberUpdateUrlDto memberUpdateDto)
        {
            // Get username From token (can't trust client)
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var username = User.GetUsername();
            var username = memberUpdateDto.Username;

            var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username); 

            // Use Automapper to make from memberUpdateDto to AppUser
            this.mapper.Map(memberUpdateDto, user);

            this.unitOfWork.UserRepository.Update(user);

            if(await this.unitOfWork.Complete())
                return NoContent();
            
            return BadRequest("Failed to update user.");
        }

        // api/users/url-requests
        [Authorize(Roles = "Admin")]
        [HttpGet("url-requests")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersURLRequests([FromQuery]UserParams userParams) 
        {
            userParams.CurrentUserName = User.GetUsername();    // From Token

            var users = await this.unitOfWork.UserRepository.GetMembersURLRequestsAsync(userParams);
            // var users = await this.unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await photoService.AddPhotoAsync(file);

            if(result.Error != null)
                return BadRequest(result.Error.Message);

            var photo = new Photo 
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await unitOfWork.Complete())
            {
                // return mapper.Map<PhotoDto>(photo);

                // Now we're return the route to get the photos and the photo object
                //return CreatedAtRoute("GetUser", mapper.Map<PhotoDto>(photo));
                return CreatedAtRoute("GetUser", new {username = user.UserName}, mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photos");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId ) {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain)
                return BadRequest("This is already your main photo");
            
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null)
                currentMain.IsMain = false;

            photo.IsMain = true;

            if (await unitOfWork.Complete())
                return NoContent();

            return BadRequest("Failed to set main photo");
            
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId) {
            // Pull user from JWT Bearer token
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null)
                return NotFound();
            
            if (photo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null) 
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }   

            user.Photos.Remove(photo);

            if (await unitOfWork.Complete())
                return Ok();
            
            return BadRequest("Failed to delete the photo");

        }

        
        [HttpPost("add-user-file")]
        //public async Task<ActionResult<PhotoDto>> AddUserFile(IFormFile file)
        public async Task<ActionResult<UserFileDto>> AddUserFile(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var fileresult = await fileRepoService.AddFileAsync(file);

            if(fileresult == null)
                return BadRequest("Unable to upload file");

            fileresult.isOther = true;
            
            if( user.UserFiles == null) {
                user.UserFiles = new List<UserFile>();
            }

            if( user.UserFiles != null)
                user.UserFiles.Add(fileresult);

            if (await unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUserFiles", new {username = user.UserName}, mapper.Map<UserFileDto>(fileresult));
            }

            return BadRequest("Problem adding userfile");
        }

        [HttpPost("add-user-file-print")]
        //public async Task<ActionResult<PhotoDto>> AddUserFile(IFormFile file)
        public async Task<ActionResult<UserFileDto>> AddUserFilePrint(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var fileresult = await fileRepoService.AddFileAsync(file);

            if(fileresult == null)
                return BadRequest("Unable to upload file");

            fileresult.isPrintJob = true;
            
            if( user.UserFiles == null) {
                user.UserFiles = new List<UserFile>();
            }

            if( user.UserFiles != null)
                user.UserFiles.Add(fileresult);

                // Submit printjob to cups here
                //SubmitToCups(fileresult)

            if (await unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUserFiles", new {username = user.UserName}, mapper.Map<UserFileDto>(fileresult));
            }

            return BadRequest("Problem adding userfile");
        }


        [HttpGet("get-user-files/{username}", Name = "GetUserFiles")]
        public async Task<ActionResult<MemberFileDto>> GetUserFiles(string username) 
        {
            //var user = await this.unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            //return await this.unitOfWork.UserRepository.GetMemberAsync(username);
            return await this.unitOfWork.UserRepository.GetMemberFilesAsync(username);
            


            //return await _context.Users.FindAsync(id);
        }
        
    }
}