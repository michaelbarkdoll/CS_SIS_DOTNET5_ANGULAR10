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
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly IFileRepoService fileRepoService;

        //public UsersController(DataContext context)
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService, IFileRepoService fileRepoService)
        //public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.photoService = photoService;
            this.fileRepoService = fileRepoService;
            //this._context = context;
        }

        // api/users
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
            // var users = await this.userRepository.GetUsersAsync();
            // //var usersToReturn = this.mapper.Map<IEnumerable<<MapTO>>>(FROM);
            // var usersToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            //var users = await this.userRepository.GetMembersAsync();

            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            //userParams.CurrentUserName = User.GetUsername();
            userParams.CurrentUserName = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";

            var users = await this.userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        //api/users/3
        //[Authorize]
        //[HttpGet("{username}")]
        [HttpGet("{username}", Name = "GetUser")]
        // public ActionResult<AppUser> GetUser(int id) 
        // {
        //     var user = _context.Users.Find(id);

        //     return user;
        // }
        //public async Task<ActionResult<AppUser>> GetUser(string username) 
        public async Task<ActionResult<MemberDto>> GetUser(string username) 
        {
            //var user = await this.userRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            return await this.userRepository.GetMemberAsync(username);
            
            


            //return await _context.Users.FindAsync(id);
        }

        // Update a resource on our server
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // Get username From token (can't trust client)
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();

            var user = await this.userRepository.GetUserByUsernameAsync(username); 

            // Use Automapper to make from memberUpdateDto to AppUser
            this.mapper.Map(memberUpdateDto, user);

            this.userRepository.Update(user);

            if(await this.userRepository.SaveAllAsync())
                return NoContent();
            
            return BadRequest("Failed to update user.");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

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

            if (await userRepository.SaveAllAsync())
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
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain)
                return BadRequest("This is already your main photo");
            
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null)
                currentMain.IsMain = false;

            photo.IsMain = true;

            if (await userRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to set main photo");
            
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId) {
            // Pull user from JWT Bearer token
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

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

            if (await userRepository.SaveAllAsync())
                return Ok();
            
            return BadRequest("Failed to delete the photo");

        }

        
        [HttpPost("add-user-file")]
        //public async Task<ActionResult<PhotoDto>> AddUserFile(IFormFile file)
        public async Task<ActionResult<UserFileDto>> AddUserFile(IFormFile file)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

            var fileresult = await fileRepoService.AddFileAsync(file);

            if(fileresult == null)
                return BadRequest("Unable to upload file");

            fileresult.isOther = true;
            
            if( user.UserFiles == null) {
                user.UserFiles = new List<UserFile>();
            }

            if( user.UserFiles != null)
                user.UserFiles.Add(fileresult);

            if (await userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUserFiles", new {username = user.UserName}, mapper.Map<UserFileDto>(fileresult));
            }

            return BadRequest("Problem adding userfile");
        }


        [HttpGet("get-user-files/{username}", Name = "GetUserFiles")]
        public async Task<ActionResult<MemberFileDto>> GetUserFiles(string username) 
        {
            //var user = await this.userRepository.GetUserByUsernameAsync(username);
            //return this.mapper.Map<MemberDto>(user);

            //return await this.userRepository.GetMemberAsync(username);
            return await this.userRepository.GetMemberFilesAsync(username);
            


            //return await _context.Users.FindAsync(id);
        }
        
    }
}