using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user) 
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;      // UniqueName
            //return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user) 
        {
            //return user.FindFirst(ClaimTypes.Name)?.Value;      // UniqueName
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);    // Unique Id
        }
    }
}