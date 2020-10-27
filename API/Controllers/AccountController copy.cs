using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController2 : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        public AccountController2(DataContext context, ITokenService tokenService, IMapper mapper)   // Inject token service into account controller
        {
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.context = context;

        }

        [HttpPost("register")]
        //public async Task<ActionResult<AppUser>> Register(string username, string password) 
        //public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                return BadRequest("Username is taken"); // 400 status
            
            var user = mapper.Map<AppUser>(registerDto);

            // using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            // user.PasswordSalt = hmac.Key;

            // var user = new AppUser
            // {
            //     UserName = registerDto.Username.ToLower(),
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //     PasswordSalt = hmac.Key
            // };

            this.context.Users.Add(user);       // Just tracking in EF
            await this.context.SaveChangesAsync();  // Saves changes to Entity Framework

            //return user;
            return new UserDto
            {
                Username = user.UserName,
                //Token = this.tokenService.CreateToken(user),
                Token = await this.tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        //public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            //var user = await this.context.Users.FirstOrDefaultAsync
            var user = await this.context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null)
                return Unauthorized("Invalid username");

            /*
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }
            */

            //return user;
            return new UserDto
            {
                Username = user.UserName,
                //Token = this.tokenService.CreateToken(user),
                Token = await this.tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };


        }
        private async Task<bool> UserExists(string username)
        {
            return await this.context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}