using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;

namespace ToDoListMVC.WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int loggedInUserId = 0;

            if (User.Identity.IsAuthenticated)
            {
                loggedInUserId = _user.GetLoggedInUserId();
            }
            var model = new UserHomeViewModel { Id = loggedInUserId };
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Error()
        {
            return Ok();
        }
    }
}
