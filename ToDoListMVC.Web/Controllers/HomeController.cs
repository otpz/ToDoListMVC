using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using ToDoListMVC.Entity.ViewModels.TaskJobs;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;
using ToDoListMVC.Web.Models;

namespace ToDoListMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task<IActionResult> Index()
        {
            int loggedInUser = 0;

            if (User.Identity.IsAuthenticated)
            {
                loggedInUser = _user.GetLoggedInUserId();
            }

            var model = new UserHomeViewModel{ Id = loggedInUser };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
