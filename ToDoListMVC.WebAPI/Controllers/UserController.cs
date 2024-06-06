using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListMVC.Entity.ViewModels.TaskJobs;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Helpers.PdfGenerator;
using ToDoListMVC.Service.Services.Abstractions;
using ToDoListMVC.Service.Services.Concretes;

namespace ToDoListMVC.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ITaskJobService taskJobService;
        private readonly IPdfGenerator pdfGenerator;
        private readonly ClaimsPrincipal _user;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper, ITaskJobService taskJobService, IPdfGenerator pdfGenerator)
        {
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.taskJobService = taskJobService;
            this.pdfGenerator = pdfGenerator;
            _user = httpContextAccessor.HttpContext.User;
        }

        [HttpGet("{userId?}")]
        public async Task<IActionResult> Index(int userId)
        {
            var loggedInUserId = _user.GetLoggedInUserId();
            if (loggedInUserId == 0)
            {
                return Unauthorized(new {unauthorizedResult = "Lütfen giriş yapınız."});
            }

            var user = await userService.GetUserProfileWithTaskByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new {notFoundResult = "Kullanıcı bulunamadı."});
            }

            var userProfileMap = mapper.Map<UserProfileViewModel>(user);
            bool isOwnProfile = loggedInUserId == user.Id;

            return Ok(new {successResult = "Success", userProfileMap, isOwnProfile = isOwnProfile});
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskJobAddViewModel taskJobAddViewModel)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (!ModelState.IsValid)
            {
                return BadRequest(new {validationErrorResult = "Validasyon hatası meydana geldi.", userId = loggedInUserId});
            }
            string title = await taskJobService.CreateTaskJobAsync(taskJobAddViewModel);

            bool isOwnProfile = true;

            if (title == null)
            {
                isOwnProfile = false;
                return BadRequest(new { errorResult = "İşlem başarısız.", isOwnProfile = isOwnProfile, userId = loggedInUserId });
            }

            return Ok(new { successResult = "İşlem başarısız.", isOwnProfile = isOwnProfile, userId = loggedInUserId});
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userService.GetUserInfoAsync();
            var userSettingsMap = mapper.Map<UserSettingsViewModel>(user);
            return Ok(new {successResult = "Başarılı", userSettingsMap});
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserSettingsViewModel userSettingsViewModel)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (loggedInUserId != userSettingsViewModel.Id)
            {
                return BadRequest(new {errorResult = "Bir hata oluştu", loggedInUserId});
            }
            string email = await userService.UpdateUserProfileAsync(userSettingsViewModel);

            return Ok(new {successResult = "Başarılı", userId = loggedInUserId, userEmail = email});
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePdf()
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            string result = await pdfGenerator.GenerateUserDataPdfById(loggedInUserId);
            if (result == null)
            {
                return BadRequest(new {errorResult= "Bir hata oluştu", userId = loggedInUserId});
            }

            return Ok(new {successResult = "PDF masaüstünde başarıyla oluşturuldu.", loggedInUserId , result});
        }

        [HttpGet]
        public async Task<ActionResult> Update(int taskJobId)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (taskJobId == 0)
            {
                return NotFound(new {notFoundResult = "Görev bulunamadı.", userId = loggedInUserId});
            }
            string title = await taskJobService.DisableTaskJob(taskJobId);
            bool isOwnProfile = true;

            if (title == null)
            {
                isOwnProfile = false;
                return BadRequest(new { errorResult = "İşlem başarısız.", isOwnProfile = isOwnProfile, userId = loggedInUserId});
            }

            return Ok(new {result = $"{title} adlı görev başarıyla güncellendi.", userId = loggedInUserId, isOwnProfile = isOwnProfile });
        }


        [HttpGet]
        public async Task<ActionResult> Delete(int taskJobId)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (taskJobId == 0)
            {
                return NotFound(new {notFoundError = "Görev bulunamadı", userId = loggedInUserId});
            }

            string title = await taskJobService.SafeDeleteTaskJob(taskJobId);

            bool isOwnProfile = true;

            if (title == null)
            {
                isOwnProfile = false;
                return BadRequest(new { errorResult = "İşlem başarısız.", isOwnProfile = isOwnProfile, userId = loggedInUserId });
            }

            return Ok(new { result = $"{title} adlı görev başarıyla güncellendi.", userId = loggedInUserId , isOwnProfile = isOwnProfile });
        }
    }
}
