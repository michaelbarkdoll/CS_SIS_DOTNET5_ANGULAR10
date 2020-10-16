using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get context after action is executed
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            //var username = resultContext.HttpContext.User.GetUsername();
            var userid = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            //var user = await repo.GetUserByUsernameAsync(username);
            var user = await repo.GetUserByIdAsync(userid);
            user.LastActive = DateTime.Now;

            await repo.SaveAllAsync();
        }
    }
}