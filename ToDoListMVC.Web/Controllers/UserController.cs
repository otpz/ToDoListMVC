using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListMVC.Entity.ViewModels.TaskJobs;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ITaskJobService taskJobService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public UserController(IUserService userService, ITaskJobService taskJobService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.taskJobService = taskJobService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        [HttpGet]
        [Route("user/index/{userId?}")]
        public async Task<IActionResult> Index(int userId)
        {
            var loggedInUserId = _user.GetLoggedInUserId();
            if (loggedInUserId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await userService.GetUserProfileByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var userProfileMap = mapper.Map<UserProfileViewModel>(user);
            ViewBag.IsOwnProfile = loggedInUserId == user.Id;
            return View(userProfileMap);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskJobAddViewModel taskJobAddViewModel)
        {
            string title = await taskJobService.CreateTaskJobAsync(taskJobAddViewModel);

            if (title != null)
                ViewBag.IsOwnProfile = true;
            else
                ViewBag.IsOwnProfile = false;

            return RedirectToAction("index", "user", new {userId = _user.GetLoggedInUserId()});
        }

        [HttpGet]
        public async Task<ActionResult> Update(int taskJobId)
        {
            if (taskJobId == 0)
            {
                return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
            }
            string title = await taskJobService.DisableTaskJob(taskJobId);

            if (title != null)
                ViewBag.IsOwnProfile = true;
            else
                ViewBag.IsOwnProfile = false;

            return RedirectToAction("index", "user", new {userId = _user.GetLoggedInUserId() });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int taskJobId)
        {
            if (taskJobId == 0)
            {
                return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
            }
            string title = await taskJobService.SafeDeleteTaskJob(taskJobId);

            if (title != null)
                ViewBag.IsOwnProfile = true;
            else
                ViewBag.IsOwnProfile = false;

            return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
        }

    }
}
