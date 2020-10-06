using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext context;
        public BuggyController(DataContext context)
        {
            this.context = context;
        }

        // API URL: api/buggy/auth
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() 
        {
            return "secret text";
        }

        // API URL: api/buggy/not-found
        [Authorize]
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound() 
        {
            var thing = this.context.Users.Find(-1);

            if (thing == null) return NotFound();

            return Ok(thing);
        }

        // API URL: api/buggy/auth
        [Authorize]
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError() 
        {
            var thing = this.context.Users.Find(-1);
            
            var thingToReturn = thing.ToString();   // null to ToString generates null reference exception

            return thingToReturn;
        }

        // API URL: api/buggy/auth
        [Authorize]
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() 
        {
            return BadRequest("This was not a good request");
        }

    }
}