using System.Security.Claims;

namespace ToDoListMVC.Service.Extensions
{
    public static class LoggedInUserExtension
    {
        public static int GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            return Int32.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
