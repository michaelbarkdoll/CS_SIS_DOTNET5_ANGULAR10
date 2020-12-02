using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        //private readonly DataContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;

        //public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)   // Inject token service into account controller
        // public AccountController(UserManager<AppUser> userManger, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)   // Inject token service into account controller
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper,
            IAccountService accountService)   // Inject token service into account controller
        {
            this.mapper = mapper;
            this.accountService = accountService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            //this.context = context;

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

            //this.context.Users.Add(user);       // Just tracking in EF
            //await this.context.SaveChangesAsync();  // Saves changes to Entity Framework

            // Creates user and saves into DB
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            //return user;
            return new UserDto
            {
                Username = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("loginv2")]
        //public async Task<ActionResult<AppUser>> Register(string username, string password) 
        //public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        public async Task<ActionResult<UserDto>> LoginV2(LoginDto loginDto)
        {
            // Authenticate local admin superuser
            if (loginDto.Username.Equals("admin") ){
                if (await UserExists(loginDto.Username)) {
                    // Proceed with login
                    var user = await this.userManager.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

                    if (user == null)
                        return Unauthorized("Invalid username");

                    var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                    if (!result.Succeeded)
                        return Unauthorized();
                
                    return new UserDto
                    {
                        Username = user.UserName,
                        Token = await this.tokenService.CreateToken(user),
                        PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                        KnownAs = user.KnownAs,
                        Gender = user.Gender
                    };
                }
            }   

            // Authenticate other users via ssh
            if( ! await this.accountService.AuthenticateUserAsync(loginDto.Username, loginDto.Password)) {
                // System.Console.WriteLine($"{loginDto.Username} {loginDto.Password}");
                return BadRequest("Invalid login");
            }

            // User already exists in DB
            if (await UserExists(loginDto.Username)) {
                // Proceed with login
                var user = await this.userManager.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

                if (user == null)
                    return Unauthorized("Invalid username");

                // Here the password is checked against the 10000 salt iteration version (use a different password auth e.g., ldap here)
                // var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                
                //await signInManager.SignInAsync(user, true, "ldap");

                // await signInManager.SignOutAsync();
                // await signInManager.SignInAsync(user, true, "ssh");
                
                // Testing                
                // await signInManager.SignInAsync(user, true);
                // await signInManager.SignOutAsync();

                
                // Currently storing ssh users password as a fake password inside MS Identity. (Remove this later when more local accounts are permitted)
                var result = await signInManager.CheckPasswordSignInAsync(user, "Pa$$w0rd", false);
                
                if (!result.Succeeded)
                    return Unauthorized();
                

                //return user;

                return new UserDto
                {
                    Username = user.UserName,
                    Token = await this.tokenService.CreateToken(user),
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender
                };
            }
            // User is not previously in DB
            else {
                //var user = mapper.Map<AppUser>(loginDto);
                var user = new AppUser {
                    UserName = loginDto.Username,
                    KnownAs = loginDto.Username
                };
                user.UserName = loginDto.Username.ToLower();

                // Creates user and saves into DB
                loginDto.Password = "Pa$$w0rd";     // Hard coded for ssh authentication
                var result = await userManager.CreateAsync(user, loginDto.Password);

                if(!result.Succeeded)
                    return BadRequest(result.Errors);

                var roleResult = await userManager.AddToRoleAsync(user, "Member");

                if(!roleResult.Succeeded)
                    return BadRequest(roleResult.Errors);

                //return user;
                return new UserDto
                {
                    Username = user.UserName,
                    Token = await this.tokenService.CreateToken(user),
                    KnownAs = user.UserName,
                    Gender = "Male"     // Fix this later
                    // KnownAs = user.KnownAs,
                    // Gender = user.Gender
                };
            }

            // return BadRequest("Username is taken"); // 400 status
        }

        [HttpPost("login")]
        //public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            //var user = await this.context.Users.FirstOrDefaultAsync
            //var user = await this.context.Users
            var user = await this.userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

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

            // Here the password is checked against the 10000 salt iteration version (use a different password auth e.g., ldap here)
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            //await signInManager.SignInAsync(user, true, "ldap");
            

            if (!result.Succeeded)
                return Unauthorized();

            //return user;
            return new UserDto
            {
                Username = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };


        }
        private async Task<bool> UserExists(string username)
        {
            return await this.userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
            //return await this.context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}