using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
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
        private readonly IToastNotification toastNotification;
        private readonly ClaimsPrincipal _user;

        public UserController(IUserService userService, ITaskJobService taskJobService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification)
        {
            this.userService = userService;
            this.taskJobService = taskJobService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.toastNotification = toastNotification;
            _user = httpContextAccessor.HttpContext.User;
        }

        [HttpGet]
        [Route("user/index/{userId?}")]
        public async Task<IActionResult> Index(int userId)
        {
            var loggedInUserId = _user.GetLoggedInUserId();
            if (loggedInUserId == 0)
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
            if (!ModelState.IsValid)
            {
                toastNotification.AddErrorToastMessage($"Validasyon hatası meydana geldi", new ToastrOptions { Title = "Hata" });
                ModelState.AddModelError("", "Görev bilgilerini tamamlayınız.");
                return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
            }
            string title = await taskJobService.CreateTaskJobAsync(taskJobAddViewModel);

            if (title != null)
            {
                toastNotification.AddSuccessToastMessage($"{title} adlı görev başarıyla eklendi", new ToastrOptions { Title = "Başarılı" });
                ViewBag.IsOwnProfile = true;
            }
            else
            {
                toastNotification.AddErrorToastMessage($"Bir hata meydana geldi", new ToastrOptions { Title = "Hata" });
                ViewBag.IsOwnProfile = false;
            }

            return RedirectToAction("index", "user", new {userId = _user.GetLoggedInUserId()});
        }

        [HttpGet]
        public async Task<ActionResult> Update(int taskJobId)
        {
            if (taskJobId == 0)
            {
                toastNotification.AddErrorToastMessage("Görev bulunamadı", new ToastrOptions { Title = "Hata" });
                return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
            }
            string title = await taskJobService.DisableTaskJob(taskJobId);

            if (title != null)
                ViewBag.IsOwnProfile = true;
            else
                ViewBag.IsOwnProfile = false;
            toastNotification.AddSuccessToastMessage($"{title} adlı görev tamamlandı", new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("index", "user", new {userId = _user.GetLoggedInUserId() });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int taskJobId)
        {
            if (taskJobId == 0)
            {
                toastNotification.AddErrorToastMessage($"Görev bulunamadı", new ToastrOptions { Title = "Hata" });
                return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
            }
            string title = await taskJobService.SafeDeleteTaskJob(taskJobId);

            if (title != null)
                ViewBag.IsOwnProfile = true;
            else
                ViewBag.IsOwnProfile = false;
            toastNotification.AddSuccessToastMessage($"{title} adlı görev silindi", new ToastrOptions { Title = "Başarılı" });
            return RedirectToAction("index", "user", new { userId = _user.GetLoggedInUserId() });
        }

    }
}
