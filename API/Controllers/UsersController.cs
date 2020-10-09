using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        //public UsersController(DataContext context)
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() 
        {
            // var users = await this.userRepository.GetUsersAsync();
            // //var usersToReturn = this.mapper.Map<IEnumerable<<MapTO>>>(FROM);
            // var usersToReturn = this.mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            var users = await this.userRepository.GetMembersAsync();
            return Ok(users);

            //return users;
            //return await _context.Users.ToListAsync();
        }

        //api/users/3
        //[Authorize]
        [HttpGet("{username}")]
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
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await this.userRepository.GetUserByUsernameAsync(username); 

            // Use Automapper to make from memberUpdateDto to AppUser
            this.mapper.Map(memberUpdateDto, user);

            this.userRepository.Update(user);

            if(await this.userRepository.SaveAllAsync())
                return NoContent();
            
            return BadRequest("Failed to update user.");
        }
    }
}